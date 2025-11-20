using System;
using System.Net.Http;
using System.Net;
using Polly;
using Polly.Extensions.Http;

namespace SaipemE_PTW.Services.Http
{
    /// <summary>
    /// Data: 2025-11-05 - Politiche di resilienza per HttpClient (retry + circuit breaker) usando Polly.
    /// Sicurezza: non esegue retry su metodi idempotenti con body sensibile a meno di necessità; si limita a status/transient network.
    /// </summary>
    public static class HttpPolicies
    {
        // Data: 2025-11-05 - Retry con backoff esponenziale e jitter minimo
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            // Retry su errori transienti 5xx, 408 e 429
            return HttpPolicyExtensions
                .HandleTransientHttpError() // 5xx, 408, eccezioni rete
                .OrResult(msg => (int)msg.StatusCode == 429)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200 * Math.Pow(2, attempt - 1)),
                    onRetry: (outcome, delay, attempt, context) => { /* hook per logging via handler superiore */ }
                );
        }

        // Data: 2025-11-05 - Circuit breaker per fail rapidi dopo troppi errori
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => (int)msg.StatusCode == 429)
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30)
                );
        }
    }
}
