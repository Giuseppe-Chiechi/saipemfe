using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SaipemE_PTW.Shared.Http;

namespace SaipemE_PTW.Services.Http
{
    /// <summary>
    /// Data: 2025-11-05 - Typed client per Durable Functions (HTTP triggered & orchestrations).
    /// Suggerimento: statusQueryGetUri viene gestito come GET contino rispettando retry lato UI.
    /// </summary>
    public sealed class DurableFunctionsService
    {
        private readonly SafeHttpClient _safe;
        private readonly ILogger<DurableFunctionsService> _logger;

        public DurableFunctionsService(HttpClient http, ILogger<DurableFunctionsService> logger, ILogger<SafeHttpClient> safeLogger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _safe = new SafeHttpClient(http ?? throw new ArgumentNullException(nameof(http)), safeLogger ?? throw new ArgumentNullException(nameof(safeLogger)));
        }

        // Data: 2025-11-05 - Avvia una orchestrazione
        public Task<HttpResult<StartOrchestrationResponse>> StartOrchestrationAsync(object input, CancellationToken ct = default)
            => _safe.PostAsync<object, StartOrchestrationResponse>("orchestrators/start", input, ct);

        // Data: 2025-11-05 - Polling stato orchestrazione
        public Task<HttpResult<DurableStatusDto>> GetStatusAsync(string statusQueryGetUri, CancellationToken ct = default)
            => _safe.GetAsync<DurableStatusDto>(statusQueryGetUri, ct);

        // Data: 2025-11-05 - Altri metodi dimostrativi
        public Task<HttpResult<string>> GetPingAsync(CancellationToken ct = default)
            => _safe.GetAsync<string>("df/ping", ct);

        public Task<HttpResult<string[]>> OptionsPingAsync(CancellationToken ct = default)
            => _safe.OptionsAsync("df/ping", ct);
    }

    /// <summary>
    /// Data: 2025-11-05 - DTO semplificato risposta start orchestrator (subset tipico Azure Functions Durable)
    /// </summary>
    public sealed class StartOrchestrationResponse
    {
        public string? Id { get; set; }
        public string? StatusQueryGetUri { get; set; }
        public string? SendEventPostUri { get; set; }
        public string? TerminatePostUri { get; set; }
        public string? PurgeHistoryDeleteUri { get; set; }
    }

    /// <summary>
    /// Data: 2025-11-05 - DTO stato orchestrazione (semplificato)
    /// </summary>
    public sealed class DurableStatusDto
    {
        public string? RuntimeStatus { get; set; }
        public object? Output { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
    }
}
