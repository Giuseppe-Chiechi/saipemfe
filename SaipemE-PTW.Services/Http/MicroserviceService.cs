using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SaipemE_PTW.Shared.Http;

namespace SaipemE_PTW.Services.Http
{
    /// <summary>
    /// Data: 2025-11-05 - Esempio di typed client verso microservizio.
    /// </summary>
    public sealed class MicroserviceService
    {
        private readonly SafeHttpClient _safe;
        private readonly ILogger<MicroserviceService> _logger;

        public MicroserviceService(HttpClient http, ILogger<MicroserviceService> logger, ILogger<SafeHttpClient> safeLogger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _safe = new SafeHttpClient(http ?? throw new ArgumentNullException(nameof(http)), safeLogger ?? throw new ArgumentNullException(nameof(safeLogger)));
        }

        public Task<HttpResult<string>> GetStatusAsync(CancellationToken ct = default) => _safe.GetAsync<string>("micro/status", ct);
        public Task<HttpResult<object>> PostWorkAsync(object payload, CancellationToken ct = default) => _safe.PostAsync<object, object>("micro/work", payload, ct);
        public Task<HttpResult<object>> PutWorkAsync(string id, object payload, CancellationToken ct = default) => _safe.PutAsync<object, object>($"micro/work/{Uri.EscapeDataString(id)}", payload, ct);
        public Task<HttpResult<bool>> DeleteWorkAsync(string id, CancellationToken ct = default) => _safe.DeleteAsync($"micro/work/{Uri.EscapeDataString(id)}", ct);
        public Task<HttpResult<object>> PatchWorkAsync(string id, object patch, CancellationToken ct = default) => _safe.PatchAsync<object, object>($"micro/work/{Uri.EscapeDataString(id)}", patch, ct);
        public Task<HttpResult<bool>> HeadWorkAsync(CancellationToken ct = default) => _safe.HeadAsync("micro/work", ct);
        public Task<HttpResult<string[]>> OptionsWorkAsync(CancellationToken ct = default) => _safe.OptionsAsync("micro/work", ct);
    }
}
