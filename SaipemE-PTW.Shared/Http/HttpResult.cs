using System.Net;

namespace SaipemE_PTW.Shared.Http
{
    /// <summary>
    /// Data: 2025-11-05 - Risultato standardizzato per chiamate HTTP lato client.
    /// Sicurezza: non esporre contenuti sensibili in proprietà Message/Errors in UI pubblica.
    /// </summary>
    public sealed class HttpResult<T>
    {
        public bool Success { get; init; }
        public HttpStatusCode StatusCode { get; init; }
        public string? Message { get; init; }
        public T? Data { get; init; }

        public static HttpResult<T> Ok(T data, HttpStatusCode code = HttpStatusCode.OK) => new()
        {
            Success = true,
            StatusCode = code,
            Data = data
        };

        public static HttpResult<T> Fail(string? message, HttpStatusCode code) => new()
        {
            Success = false,
            StatusCode = code,
            Message = message
        };
    }
}
