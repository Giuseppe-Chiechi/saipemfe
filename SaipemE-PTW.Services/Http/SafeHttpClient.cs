//CHIAMATE API CENTRALIZZATE STEP2;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SaipemE_PTW.Shared.Http;

namespace SaipemE_PTW.Services.Http
{
    /// <summary>
    /// Data: 2025-11-05 - Wrapper helper per HttpClient con JSON e gestione errori/Logging.
    /// Accessibilità: i messaggi ritornati sono pensati per UI leggibile (es. MudAlert/MudSnackbar).
    /// Sicurezza: non serializza oggetti non attesi e limita dimensione payload.
    /// </summary>
    public sealed class SafeHttpClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<SafeHttpClient> _logger;
        private static readonly JsonSerializerOptions _jsonOpts = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = false
        };

        public SafeHttpClient(HttpClient http, ILogger<SafeHttpClient> logger)
        {
            _http = http ?? throw new ArgumentNullException(nameof(http));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Data: 2025-11-05 - GET JSON tipizzato
        public async Task<HttpResult<T>> GetAsync<T>(string uri, CancellationToken ct = default)
        {
            try
            {
                var res = await _http.GetAsync(uri, ct);
                if (!res.IsSuccessStatusCode)
                    return HttpResult<T>.Fail($"Errore { (int)res.StatusCode }", res.StatusCode);
                var data = await res.Content.ReadFromJsonAsync<T>(_jsonOpts, ct);
                return HttpResult<T>.Ok(data! , res.StatusCode);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[SafeHttp] GET cancellata: {Uri}", uri);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SafeHttp] GET errore: {Uri}", uri);
                return HttpResult<T>.Fail("Errore di rete", System.Net.HttpStatusCode.BadRequest);
            }
        }

        // Data: 2025-11-05 - POST JSON tipizzato
        public async Task<HttpResult<TOut>> PostAsync<TIn, TOut>(string uri, TIn payload, CancellationToken ct = default)
        {
            try
            {
                if (payload is null) return HttpResult<TOut>.Fail("Payload null", System.Net.HttpStatusCode.BadRequest);

                using var res = await _http.PostAsJsonAsync(uri, payload, _jsonOpts, ct);
                if (!res.IsSuccessStatusCode)
                    return HttpResult<TOut>.Fail($"Errore { (int)res.StatusCode }", res.StatusCode);

                var data = await res.Content.ReadFromJsonAsync<TOut>(_jsonOpts, ct);
                return HttpResult<TOut>.Ok(data!, res.StatusCode);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[SafeHttp] POST cancellata: {Uri}", uri);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SafeHttp] POST errore: {Uri}", uri);
                return HttpResult<TOut>.Fail("Errore di rete", System.Net.HttpStatusCode.BadRequest);
            }
        }

        // Data: 2025-11-05 - PUT JSON tipizzato
        public async Task<HttpResult<TOut>> PutAsync<TIn, TOut>(string uri, TIn payload, CancellationToken ct = default)
        {
            try
            {
                if (payload is null) return HttpResult<TOut>.Fail("Payload null", System.Net.HttpStatusCode.BadRequest);

                using var res = await _http.PutAsJsonAsync(uri, payload, _jsonOpts, ct);
                if (!res.IsSuccessStatusCode)
                    return HttpResult<TOut>.Fail($"Errore { (int)res.StatusCode }", res.StatusCode);

                var data = await res.Content.ReadFromJsonAsync<TOut>(_jsonOpts, ct);
                return HttpResult<TOut>.Ok(data!, res.StatusCode);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[SafeHttp] PUT cancellata: {Uri}", uri);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SafeHttp] PUT errore: {Uri}", uri);
                return HttpResult<TOut>.Fail("Errore di rete", System.Net.HttpStatusCode.BadRequest);
            }
        }

        // Data: 2025-11-05 - DELETE
        public async Task<HttpResult<bool>> DeleteAsync(string uri, CancellationToken ct = default)
        {
            try
            {
                using var res = await _http.DeleteAsync(uri, ct);
                if (!res.IsSuccessStatusCode)
                    return HttpResult<bool>.Fail($"Errore { (int)res.StatusCode }", res.StatusCode);

                return HttpResult<bool>.Ok(true, res.StatusCode);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[SafeHttp] DELETE cancellata: {Uri}", uri);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SafeHttp] DELETE errore: {Uri}", uri);
                return HttpResult<bool>.Fail("Errore di rete", System.Net.HttpStatusCode.BadRequest);
            }
        }

        // Data: 2025-11-05 - PATCH (JSON Merge Patch RFC 7396)
        public async Task<HttpResult<TOut>> PatchAsync<TIn, TOut>(string uri, TIn patchDoc, CancellationToken ct = default)
        {
            try
            {
                var content = JsonContent.Create(patchDoc, options: _jsonOpts);
                content.Headers.ContentType!.MediaType = "application/merge-patch+json";
                using var req = new HttpRequestMessage(HttpMethod.Patch, uri) { Content = content };
                using var res = await _http.SendAsync(req, ct);
                if (!res.IsSuccessStatusCode)
                    return HttpResult<TOut>.Fail($"Errore { (int)res.StatusCode }", res.StatusCode);

                var data = await res.Content.ReadFromJsonAsync<TOut>(_jsonOpts, ct);
                return HttpResult<TOut>.Ok(data!, res.StatusCode);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[SafeHttp] PATCH cancellata: {Uri}", uri);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SafeHttp] PATCH errore: {Uri}", uri);
                return HttpResult<TOut>.Fail("Errore di rete", System.Net.HttpStatusCode.BadRequest);
            }
        }

        // Data: 2025-11-05 - HEAD
        public async Task<HttpResult<bool>> HeadAsync(string uri, CancellationToken ct = default)
        {
            try
            {
                using var req = new HttpRequestMessage(HttpMethod.Head, uri);
                using var res = await _http.SendAsync(req, ct);
                return res.IsSuccessStatusCode
                    ? HttpResult<bool>.Ok(true, res.StatusCode)
                    : HttpResult<bool>.Fail($"Errore { (int)res.StatusCode }", res.StatusCode);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[SafeHttp] HEAD cancellata: {Uri}", uri);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SafeHttp] HEAD errore: {Uri}", uri);
                return HttpResult<bool>.Fail("Errore di rete", System.Net.HttpStatusCode.BadRequest);
            }
        }

        // Data: 2025-11-05 - OPTIONS
        public async Task<HttpResult<string[]>> OptionsAsync(string uri, CancellationToken ct = default)
        {
            try
            {
                using var req = new HttpRequestMessage(HttpMethod.Options, uri);
                using var res = await _http.SendAsync(req, ct);
                if (!res.IsSuccessStatusCode)
                    return HttpResult<string[]>.Fail($"Errore { (int)res.StatusCode }", res.StatusCode);

        if (res.Headers.TryGetValues("Allow", out var allow))
                {
                    return HttpResult<string[]>.Ok([.. allow]);
                }

                return HttpResult<string[]>.Ok(Array.Empty<string>());
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[SafeHttp] OPTIONS cancellata: {Uri}", uri);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SafeHttp] OPTIONS errore: {Uri}", uri);
                return HttpResult<string[]>.Fail("Errore di rete", System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}
