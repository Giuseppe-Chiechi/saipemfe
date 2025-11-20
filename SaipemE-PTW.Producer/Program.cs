using System.Collections.Concurrent;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SaipemE_PTW.Producer.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer; //2025-11-10: aggiunta per JWT
using Microsoft.IdentityModel.Tokens; //2025-11-10: validazione token
using Serilog; //2025-11-10: logging strutturato
using SaipemE_PTW.Services.Auth; //2025-11-10: parametri validazione mock
using SaipemE_PTW.Shared.Models.PWT; //2025-11-10: modelli AttachmentType
using SaipemE_PTW.Producer.Services; //2025-11-10: service application AttachmentTypes
using SaipemE_PTW.Shared.Models.Logger; // Added for LogMessage
using Microsoft.AspNetCore.OutputCaching; // Added for OutputCache

var builder = WebApplication.CreateBuilder(args);

// OUTPUT CACHE: TTL letto da configurazione appsettings (Caching:AttachmentTypes:Seconds).
/// Se non presente, fallback a 300 secondi.
var attachmentTypesTtlSeconds = builder.Configuration.GetValue<int?>("Caching:AttachmentTypes:Seconds") ??300;

//2025-11-10: Configurazione Serilog sicura (no dati sensibili)
Log.Logger = new LoggerConfiguration()
 .Enrich.FromLogContext()
 .WriteTo.Console()
 .WriteTo.File("logs/producer-api-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit:14)
 .MinimumLevel.Information()
 .CreateLogger();
builder.Host.UseSerilog(); //2025-11-10: integra Serilog in host

//2025-11-10: DbContext (connection string sicura, Trusted_Connection per ambiente dev locale)
var connectionString = builder.Configuration.GetConnectionString("Default")
 ?? "Server=localhost;Database=SaipemPWT;Trusted_Connection=True;TrustServerCertificate=True;";
builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseSqlServer(connectionString));

// OUTPUT CACHE: registrazione policy "AttachmentTypesCache".
// - Expire: durata entry in cache (TTL)
// - SetVaryByQuery("lang"): chiavi cache diverse per ogni lang (es. it/en)
builder.Services.AddOutputCache(options =>
{
 options.AddPolicy("AttachmentTypesCache", b => b
 .Expire(TimeSpan.FromSeconds(attachmentTypesTtlSeconds))
 .SetVaryByQuery("lang"));
});

//2025-11-10: Registrazione service application per AttachmentTypes
builder.Services.AddScoped<IAttachmentTypeService, AttachmentTypeService>();

//2025-11-10: Swagger/OpenAPI (non esporre segreti, solo doc)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
 c.SwaggerDoc("v1", new() { Title = "SaipemE-PTW Producer", Version = "v1" });
});

//2025-11-10: CORS (in produzione restringere origin, header, metodi)
builder.Services.AddCors(o =>
{
 o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

//2025-11-10: Autenticazione JWT (mock dev) - usare provider reale in produzione
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
 //2025-11-10: Parametri di validazione (issuer/audience disabilitati in dev)
 options.TokenValidationParameters = AuthConstants.GetValidationParameters();
 options.Events = new JwtBearerEvents
 {
 OnAuthenticationFailed = ctx =>
 {
 Log.Warning(ctx.Exception, "[JWT] Autenticazione fallita");
 return Task.CompletedTask;
 },
 OnTokenValidated = ctx =>
 {
 Log.Debug("[JWT] Token valido per subject={Subject}", ctx.Principal?.Identity?.Name);
 return Task.CompletedTask;
 }
 };
 });

//2025-11-10: Autorizzazione (policy custom eventualmente)
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
 //2025-11-10: Swagger solo dev
 app.UseSwagger();
 app.UseSwaggerUI(o =>
 {
 o.SwaggerEndpoint("/swagger/v1/swagger.json", "Producer v1");
 o.RoutePrefix = "swagger"; // /swagger
 });
}

// OUTPUT CACHE: il middleware deve essere registrato prima del mapping endpoint
// per poter intercettare e memorizzare le risposte.
app.UseOutputCache();

//2025-11-10: Middleware ordine corretto
app.UseCors();
app.UseAuthentication(); //2025-11-10: prima di Authorization
app.UseAuthorization();

var jsonOpts = new JsonSerializerOptions(JsonSerializerDefaults.Web);

//2025-11-10: Root redirect sicuro
app.MapGet("/", () => Results.Redirect("/swagger"));

// ========== API ==========
//app.MapGet("/api/ping", () => Results.Ok("pong"))
// .WithName("ApiPing").WithTags("API");

//app.MapPost("/api/echo", (JsonElement payload) => Results.Json(payload, jsonOpts))
// .WithName("ApiEcho").WithTags("API");

//app.MapPut("/api/resources/{id}", (string id, JsonElement resource) =>
// Results.Json(new { id, resource }, jsonOpts))
// .WithName("ApiPutResource").WithTags("API");

//app.MapDelete("/api/resources/{id}", (string id) => Results.Ok(true))
// .WithName("ApiDeleteResource").WithTags("API");

///// HEAD /api/resources
//app.MapMethods("/api/resources", new[] { "HEAD" }, () => Results.Ok())
// .WithName("ApiHeadResources").WithTags("API");

///// OPTIONS /api/resources
//app.MapMethods("/api/resources", new[] { "OPTIONS" }, () =>
// Results.Json(new[] { "GET", "POST", "PUT", "DELETE", "PATCH", "HEAD", "OPTIONS" }, jsonOpts))
// .WithName("ApiOptionsResources").WithTags("API");

///// PATCH /api/resources/{id} (mock)
//app.MapMethods("/api/resources/{id}", new[] { "PATCH" }, (string id, JsonElement patchDoc) =>
// Results.Json(new { id, applied = patchDoc }, jsonOpts))
// .WithName("ApiPatchResource").WithTags("API");

/// 2025-11-10: Endpoint protetto AttachmentTypes - usa service applicativo, non DbContext
app.MapGet("/api/attachment-types", async (string lang, IAttachmentTypeService svc, ILoggerFactory lf, CancellationToken ct) =>
{
 // Validazione input: risposte400 non vengono cache-ate
 if (string.IsNullOrWhiteSpace(lang)) return Results.BadRequest(new { error = "lang required" });
 if (lang.Length >5) return Results.BadRequest(new { error = "lang invalid length" });
 lang = lang.Trim().ToLowerInvariant();

 var logger = lf.CreateLogger("AttachmentTypesEndpoint");

 // IMPORTANTE: questo log è eseguito solo quando la cache è MISS per la coppia (route + lang).
 // Se la risposta è servita da cache (HIT), il delegate non viene rieseguito.
 logger.LogInformation("[AttachmentTypes] MISS cache per lang={Lang}", lang);

 try
 {
 var result = await svc.GetAsync(lang, ct);
 //200 OK: il payload viene memorizzato in cache secondo la policy.
 return Results.Ok(result);
 }
 catch (OperationCanceledException)
 {
 logger.LogWarning("[AttachmentTypes] Richiesta cancellata");
 return Results.StatusCode(499); //499 non cache-ato
 }
 catch (Exception ex)
 {
 logger.LogError(ex, "[AttachmentTypes] Errore interno");
 return Results.Problem("Internal server error", statusCode:500); //500 non cache-ato
 }
})
.RequireAuthorization() // L'endpoint rimane protetto con JWT
.CacheOutput("AttachmentTypesCache") // Associa la policy di OutputCache (TTL + VaryByQuery lang)
.WithName("GetAttachmentTypes")
.WithTags("AttachmentTypes");

// ========== MICRO ==========
//app.MapGet("/micro/status", () => Results.Ok("ok"))
// .WithName("MicroStatus").WithTags("Micro");

//app.MapPost("/micro/work", (JsonElement command) =>
// Results.Json(new { id = "job-1", state = "created", command }, jsonOpts))
// .WithName("MicroPostWork").WithTags("Micro");

//app.MapPut("/micro/work/{id}", (string id, JsonElement state) =>
// Results.Json(new { id, state, updated = true }, jsonOpts))
// .WithName("MicroPutWork").WithTags("Micro");

//app.MapDelete("/micro/work/{id}", (string id) => Results.Ok(true))
// .WithName("MicroDeleteWork").WithTags("Micro");

//app.MapMethods("/micro/work", new[] { "HEAD" }, () => Results.Ok())
// .WithName("MicroHeadWork").WithTags("Micro");

//app.MapMethods("/micro/work", new[] { "OPTIONS" }, () =>
// Results.Json(new[] { "GET", "POST", "PUT", "DELETE", "PATCH", "HEAD", "OPTIONS" }, jsonOpts))
// .WithName("MicroOptionsWork").WithTags("Micro");

//app.MapMethods("/micro/work/{id}", new[] { "PATCH" }, (string id, JsonElement patch) =>
// Results.Json(new { id, patched = patch }, jsonOpts))
// .WithName("MicroPatchWork").WithTags("Micro");

// ========== DURABLE FUNCTIONS (mock) ==========
//var store = new ConcurrentDictionary<string, DurableStatusDto>();

//app.MapGet("/df/ping", () => Results.Ok("pong-df"))
// .WithName("DurablePing").WithTags("Durable");

//app.MapMethods("/df/ping", new[] { "OPTIONS" }, () =>
// Results.Json(new[] { "GET", "OPTIONS" }, jsonOpts))
// .WithName("DurableOptionsPing").WithTags("Durable");

//app.MapPost("/orchestrators/start", (HttpContext ctx, JsonElement input) =>
//{
// var id = Guid.NewGuid().ToString("N");
// var baseUrl = $"{ctx.Request.Scheme}://{ctx.Request.Host}";

// var now = DateTimeOffset.UtcNow;
// store[id] = new DurableStatusDto
// {
// RuntimeStatus = "Running",
// Output = new { received = input },
// CreatedTime = now.UtcDateTime,
// LastUpdatedTime = now.UtcDateTime
// };

// _ = Task.Run(async () =>
// {
// await Task.Delay(1500);
// store.AddOrUpdate(id,
// _ => new DurableStatusDto { RuntimeStatus = "Completed", Output = new { result = "ok" }, CreatedTime = now.UtcDateTime, LastUpdatedTime = DateTime.UtcNow },
// (_, current) =>
// {
// current.RuntimeStatus = "Completed";
// current.Output = new { result = "ok" };
// current.LastUpdatedTime = DateTime.UtcNow;
// return current;
// });
// });

// var response = new StartOrchestrationResponse
// {
// Id = id,
// StatusQueryGetUri = $"{baseUrl}/orchestrators/instances/{id}/status",
// SendEventPostUri = $"{baseUrl}/orchestrators/instances/{id}/raiseEvent/{{eventName}}",
// TerminatePostUri = $"{baseUrl}/orchestrators/instances/{id}/terminate",
// PurgeHistoryDeleteUri = $"{baseUrl}/orchestrators/instances/{id}/purge"
// };

// return Results.Json(response, jsonOpts);
//})
//.WithName("DurableStart").WithTags("Durable");

//app.MapGet("/orchestrators/instances/{id}/status", (string id) =>
//{
// if (store.TryGetValue(id, out var status))
// return Results.Json(status, jsonOpts);

// return Results.NotFound(new { message = "Instance not found" });
//})
//.WithName("DurableStatus").WithTags("Durable");

//app.MapPost("/orchestrators/instances/{id}/raiseEvent/{eventName}", (string id, string eventName) =>
// Results.Ok(new { id, eventName, accepted = true }))
// .WithName("DurableRaiseEvent").WithTags("Durable");
//app.MapPost("/orchestrators/instances/{id}/terminate", (string id) =>
// Results.Ok(new { id, terminated = true }))
// .WithName("DurableTerminate").WithTags("Durable");
//app.MapDelete("/orchestrators/instances/{id}/purge", (string id) =>
// Results.Ok(new { id, purged = true }))
// .WithName("DurablePurge").WithTags("Durable");

// ========== Logs API (inserimento LogMessage ==========
app.MapPost("/api/logs", async (LogMessage log, AppDbContext db, ILoggerFactory lf, CancellationToken ct) =>
{
 var logger = lf.CreateLogger("LogsEndpoint");
 if (string.IsNullOrWhiteSpace(log.CorrelationId))
 {
 return Results.BadRequest(new { error = "CorrelationId is required" });
 }
 if (string.IsNullOrWhiteSpace(log.Level) || string.IsNullOrWhiteSpace(log.Message))
 {
 return Results.BadRequest(new { error = "Level and Message are required" });
 }
 try
 {
 log.Level = log.Level.Trim();
 log.Message = log.Message.Trim();
 log.AppVersion = string.IsNullOrWhiteSpace(log.AppVersion) ? "1.0.0" : log.AppVersion.Trim();
 if (log.Timestamp == default) log.Timestamp = DateTimeOffset.UtcNow;
 db.LogMessages.Add(log);
 await db.SaveChangesAsync(ct);
 logger.LogInformation("[Logs] Inserito log {CorrelationId} livello={Level}", log.CorrelationId, log.Level);
 return Results.Created($"/api/logs/{log.CorrelationId}", new { id = log.CorrelationId });
 }
 catch (DbUpdateException ex)
 {
 logger.LogWarning(ex, "[Logs] Conflitto inserimento log CorrelationId={CorrelationId}", log.CorrelationId);
 return Results.Conflict(new { error = "Log with same CorrelationId already exists", id = log.CorrelationId });
 }
 catch (OperationCanceledException)
 {
 logger.LogWarning("[Logs] Richiesta cancellata");
 return Results.StatusCode(499);
 }
 catch (Exception ex)
 {
 logger.LogError(ex, "[Logs] Errore interno");
 return Results.Problem("Internal server error", statusCode:500);
 }
})
.AllowAnonymous()
.WithName("CreateLogMessage")
.WithTags("Logs");

app.Run();

sealed class StartOrchestrationResponse
{
 public string? Id { get; set; }
 public string? StatusQueryGetUri { get; set; }
 public string? SendEventPostUri { get; set; }
 public string? TerminatePostUri { get; set; }
 public string? PurgeHistoryDeleteUri { get; set; }
}

sealed class DurableStatusDto
{
 public string? RuntimeStatus { get; set; }
 public object? Output { get; set; }
 public DateTime? CreatedTime { get; set; }
 public DateTime? LastUpdatedTime { get; set; }
}
