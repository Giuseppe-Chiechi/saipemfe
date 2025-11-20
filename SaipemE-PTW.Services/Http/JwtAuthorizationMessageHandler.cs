using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SaipemE_PTW.Services.Auth;

namespace SaipemE_PTW.Services.Http
{
    /// <summary>
    /// Data: 2025-11-05 - DelegatingHandler che inserisce automaticamente il Bearer JWT nelle richieste.
    /// Sicurezza: legge token da storage sicuro via ITokenStorageService; non logga mai il token; rimuove header Authorization se token nullo.
    /// </summary>
    public sealed class JwtAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly ITokenStorageService _tokenStorage;
        private readonly ILogger<JwtAuthorizationMessageHandler> _logger;

        public JwtAuthorizationMessageHandler(ITokenStorageService tokenStorage, ILogger<JwtAuthorizationMessageHandler> logger)
        {
            _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // Data: 2025-11-05 - Pulisce eventuale header Authorization pre-esistente
                request.Headers.Authorization = null;

                var token = await _tokenStorage.GetTokenAsync();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    _logger.LogDebug("[JwtAuthHandler] Nessun token presente: chiamata senza Authorization");
                }

                // Data: 2025-11-05 - Imposta header di sicurezza comuni
                if (!request.Headers.Contains("X-Requested-With"))
                    request.Headers.Add("X-Requested-With", "XMLHttpRequest");

                return await base.SendAsync(request, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[JwtAuthHandler] Richiesta cancellata dal token di cancellazione");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[JwtAuthHandler] Errore durante l'inserimento del token JWT");
                throw;
            }
        }
    }
}
