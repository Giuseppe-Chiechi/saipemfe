//CHIAMATE API CENTRALIZZATE STEP1;;
using Microsoft.Extensions.Logging;
using SaipemE_PTW.Shared.Http;
using SaipemE_PTW.Shared.Models.PWT; //2025-11-10: per AttachmentTypeDto

namespace SaipemE_PTW.Services.Http
{
    /// <summary>
    /// Data: 2025-11-05 - Esempio di servizio API generica (typed client).
    /// Tratta endpoint REST classici.
    /// </summary>
    public sealed class ApiService
    {
        private readonly SafeHttpClient _safe;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient http, ILogger<ApiService> logger, ILogger<SafeHttpClient> safeLogger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _safe = new SafeHttpClient(http ?? throw new ArgumentNullException(nameof(http)), safeLogger ?? throw new ArgumentNullException(nameof(safeLogger)));
        }

        // Data: 2025-11-05 - GET demo
        public Task<HttpResult<string>> GetPingAsync(CancellationToken ct = default) => _safe.GetAsync<string>("api/ping", ct);

        // Data: 2025-11-05 - POST demo
        public Task<HttpResult<object>> PostEchoAsync(object payload, CancellationToken ct = default)
            => _safe.PostAsync<object, object>("api/echo", payload, ct);

        // Data: 2025-11-05 - PUT demo
        public Task<HttpResult<object>> PutResourceAsync(string id, object payload, CancellationToken ct = default)
            => _safe.PutAsync<object, object>($"api/resources/{Uri.EscapeDataString(id)}", payload, ct);

        // Data: 2025-11-05 - DELETE demo
        public Task<HttpResult<bool>> DeleteResourceAsync(string id, CancellationToken ct = default)
            => _safe.DeleteAsync($"api/resources/{Uri.EscapeDataString(id)}", ct);

        // Data: 2025-11-05 - PATCH demo
        public Task<HttpResult<object>> PatchResourceAsync(string id, object patch, CancellationToken ct = default)
            => _safe.PatchAsync<object, object>($"api/resources/{Uri.EscapeDataString(id)}", patch, ct);

        // Data: 2025-11-05 - HEAD demo
        public Task<HttpResult<bool>> HeadResourcesAsync(CancellationToken ct = default)
            => _safe.HeadAsync("api/resources", ct);

        // Data: 2025-11-05 - OPTIONS demo
        public Task<HttpResult<string[]>> OptionsResourcesAsync(CancellationToken ct = default)
            => _safe.OptionsAsync("api/resources", ct);

        // Data: 2025-11-10 - API sicura: AttachmentTypes (JWT automatico via handler). Parametro lang validato lato server.
        public Task<HttpResult<AttachmentTypeDto[]>> GetAttachmentTypesAsync(string lang, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(lang)) lang = "it"; //2025-11-10: default sicuro
            lang = lang.Trim().ToLowerInvariant();
            var url = $"api/attachment-types?lang={Uri.EscapeDataString(lang)}";
            return _safe.GetAsync<AttachmentTypeDto[]>(url, ct);
        }
    }
}
