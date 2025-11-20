using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace SaipemE_PTW.Services.Http
{
    /// <summary>
    /// Data: 2025-11-05 - DelegatingHandler che applica retry + circuit-breaker (Polly) ai client tipizzati.
    /// Nota: usa le policy definite in HttpPolicies.
    /// </summary>
    public sealed class PollyRetryCircuitHandler : DelegatingHandler
    {
        private readonly IAsyncPolicy<HttpResponseMessage> _policy;

        public PollyRetryCircuitHandler()
        {
            var retry = HttpPolicies.GetRetryPolicy();
            var breaker = HttpPolicies.GetCircuitBreakerPolicy();
            _policy = Policy.WrapAsync(breaker, retry);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _policy.ExecuteAsync(ct => base.SendAsync(request, ct), cancellationToken);
        }
    }
}
