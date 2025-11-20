using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using SaipemE_PTW;
using SaipemE_PTW.Authentication.Mock;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using Microsoft.Extensions.Http;
using FluentValidation;
using SaipemE_PTW.Services;
using SaipemE_PTW.Services.Administrator;
using SaipemE_PTW.Services.Auth; // Data: 2025-10-20 - Registrazione servizi Auth mock
using SaipemE_PTW.Services.Common;
using SaipemE_PTW.Services.Dashboard;
using SaipemE_PTW.Services.Http;
using Microsoft.Extensions.DependencyInjection;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://your-api-id/access_as_user");

    // Mappa i ruoli dai claims di Entra ID
    options.UserOptions.RoleClaim = "roles";
});

// Data: 2025-01-15 - Registrazione servizio storage token per JWT
// Usa ProtectedLocalStorage via JS interop per storage sicuro del token
builder.Services.AddScoped<ITokenStorageService, ProtectedLocalStorageTokenStore>();

// Data: 2025-01-15 - Registrazione servizio autenticazione mock OIDC
// Genera JWT fittizi per sviluppo/test, usa ITokenStorageService per persistenza
builder.Services.AddScoped<IAuthService, MockOidcAuthService>();

// Data: 2025-11-05 - Registrazione handler JWT + handler Polly
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();
builder.Services.AddTransient<PollyRetryCircuitHandler>();

// Data: 2025-11-05 - Base address da configurazione
var apiBase = builder.Configuration["ApiBaseUrl"] ?? builder.Configuration["BaseUrl"] ?? builder.HostEnvironment.BaseAddress;
var microBase = builder.Configuration["MicroBaseUrl"] ?? apiBase;
var durableBase = builder.Configuration["DurableBaseUrl"] ?? apiBase;

// Data: 2025-01-15 - HttpClient di base con handler
// FIX: Cambiato da AddHttpClients (non esiste) a AddHttpClient
builder.Services.AddHttpClient<SafeHttpClient>(http =>
{
    http.BaseAddress = new Uri(apiBase);
})
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>()
    .AddHttpMessageHandler<PollyRetryCircuitHandler>();


// Data: 2025-11-05 - Typed client API
builder.Services.AddHttpClient<ApiService>(http =>
{
  http.BaseAddress = new Uri(apiBase);
})
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>()
    .AddHttpMessageHandler<PollyRetryCircuitHandler>();


// 2025-01-15 - MudBlazor - Registrazione servizi MudBlazor per componenti UI
// Include DialogService, SnackbarService, e altri servizi necessari
builder.Services.AddMudServices();

// 2025-01-15 - Mock Auth - Configurazione autenticazione: Mock o Entra ID
// Controlla MockAuthConfig.UseMockAuthentication per commutare tra le due modalità
if (MockAuthConfig.UseMockAuthentication)
{
  // 2025-01-15 - Mock Auth - MODALITÀ MOCK: Usa autenticazione simulata per sviluppo/test
    // Registra il provider mock come servizio singleton
    builder.Services.AddSingleton<MockAuthenticationStateProvider>();
    builder.Services.AddSingleton<AuthenticationStateProvider>(sp => 
        sp.GetRequiredService<MockAuthenticationStateProvider>());
    
    // 2025-01-15 - Mock Auth - Aggiunge i servizi di autorizzazione necessari
    builder.Services.AddAuthorizationCore();
    
    Console.WriteLine("⚠️ MOCK AUTHENTICATION ENABLED - Solo per sviluppo/test");
}
else
{
  //  // 2025-01-13 - EntraId - MODALITÀ PRODUZIONE: Usa Microsoft Entra ID
  //// Registra i servizi necessari per l'autenticazione tramite Azure AD
  //  builder.Services.AddMsalAuthentication(options =>
  //  {
  //  // 2025-01-13 - EntraId - Binding della configurazione da appsettings.json
  //      builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
        
  //      // 2025-01-13 - EntraId - Configurazione scope predefinito per accesso utente
  //   // Lo scope "User.Read" permette di leggere il profilo dell'utente autenticato
  //  options.ProviderOptions.DefaultAccessTokenScopes.Add("User.Read");
        
  // // 2025-01-13 - EntraId - Configurazione cache per i token
  //      // Abilita la cache dei token per migliorare le performance e ridurre le chiamate di autenticazione
  //      options.ProviderOptions.Cache.CacheLocation = "localStorage";
  //  });
  
  //  Console.WriteLine("✓ MICROSOFT ENTRA ID AUTHENTICATION ENABLED");
}

// Data: 2025-11-05 - Typed client Microservizi
builder.Services.AddHttpClient<MicroserviceService>(http =>
{
    http.BaseAddress = new Uri(microBase);
})
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>()
    .AddHttpMessageHandler<PollyRetryCircuitHandler>();

// Data: 2025-11-05 - Typed client Durable Functions
builder.Services.AddHttpClient<DurableFunctionsService>(http =>
{
    http.BaseAddress = new Uri(durableBase);
})
.AddHttpMessageHandler<JwtAuthorizationMessageHandler>()
.AddHttpMessageHandler<PollyRetryCircuitHandler>();

// Data: 2025-10-16 - Registrazione servizi per logging (Serilog) e gestione errori globali
// Data: 2025-01-19 - LoggingService cambiato a singleton per compatibilità con LanguageService singleton
builder.Services.AddSingleton<ILoggingService, LoggingService>();
builder.Services.AddScoped<IErrorHandlingService, ErrorHandlingService>();

// Data: 2025-01-19 - LoggerService con HttpClientFactory e BaseAddress apiBase (fix relative URI)
builder.Services.AddHttpClient<ILoggerService, LoggerService>(http =>
{
 http.BaseAddress = new Uri(apiBase);
})
    .AddHttpMessageHandler<JwtAuthorizationMessageHandler>()
    .AddHttpMessageHandler<PollyRetryCircuitHandler>();

// Data: 2025-01-19 - Registrazione servizio localizzazione WASM (singleton, dizionari in-memory)
// FIX: Usa LocalizationServiceWasm invece di LocalizationService per compatibilità Blazor WASM
builder.Services.AddSingleton<ILocalizationService, LocalizationServiceWasm>();

// Data: 2025-01-19 - Registrazione servizio multilingua (singleton per condivisione stato globale)
builder.Services.AddSingleton<ILanguageService, LanguageService>();

// Data: 2025-01-19 - Registrazione servizio mock dashboard per dati grafici MudChart
// Fornisce dati fittizi per sviluppo UI, thread-safe con logging integrato
builder.Services.AddScoped<IDashboardService, DashboardService_Mock>();

builder.Services.AddScoped<SaipemE_PTW.Services.Workflow.PWT.IPermessoLavoroService, SaipemE_PTW.Services.Workflow.PWT.PermessoLavoroService>();



builder.Services.AddValidatorsFromAssemblyContaining<SaipemE_PTW.Validators.Workflow.PWT.CaldoGenerico.Validator>();

builder.Services.AddScoped<IUtentiInterniService, UtentiInterniService>();
builder.Services.AddScoped<IUtentiEsterniService, UtentiEsterniService>();
builder.Services.AddScoped<SaipemE_PTW.Services.Workflow.Common.ICronologiaPermessoLavoroService, SaipemE_PTW.Services.Workflow.Common.CronologiaPermessoLavoroService>();
builder.Services.AddScoped<IMenuService, MenuService>();

await builder.Build().RunAsync();
