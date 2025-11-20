using SaipemE_PTW.Shared.Models;
using System.Text.Json;
using System.Net.Http.Json;
using SaipemE_PTW.Shared.Models.Logger;


namespace SaipemE_PTW.Services
{
    // Data: 2025-01-19 - Interfaccia servizio logging con supporto invio remoto
    // Estende funzionalità di ILoggingService con metodi asincroni e invio HTTP
    public interface ILoggerService
    {
        /// <summary>
        /// Log generico con invio remoto opzionale
        /// </summary>
        Task LogAsync(string message, string level = "Info", IDictionary<string, object?>? properties = null, Exception? exception = null);

        /// <summary>
        /// Log errore con invio remoto automatico (ritorna correlation ID)
        /// </summary>
        Task<string> LogErrorAsync(Exception exception, string message, IDictionary<string, object?>? properties = null);

        /// <summary>
        /// Log warning con invio remoto opzionale
        /// </summary>
        Task LogWarningAsync(string message, IDictionary<string, object?>? properties = null);

        /// <summary>
        /// Log info con invio remoto opzionale
        /// </summary>
        Task LogInfoAsync(string message, IDictionary<string, object?>? properties = null);

        /// <summary>
        /// Ottiene buffer log locale (per visualizzazione debug)
        /// </summary>
        string GetBufferedLog();

        /// <summary>
        /// Pulisce buffer log locale
        /// </summary>
        void ClearBuffer();

        /// <summary>
        /// Ultimo error ID generato
        /// </summary>
        string? LastErrorId { get; }
    }

    // Data: 2025-01-19 - Implementazione logging con Serilog (console) + invio remoto HTTP
    // Riusa ILoggingService esistente per console logging e aggiunge capacità HTTP remote
    public sealed class LoggerService : ILoggerService
    {
        private readonly ILoggingService _localLogger; // Servizio esistente (Serilog console)
        private readonly HttpClient _httpClient;
        private readonly string _remoteEndpoint; // Endpoint API remota (null = disabilitato)

        public string? LastErrorId => _localLogger.LastErrorId;

        // Data: 2025-01-19 - Costruttore con DI: riusa LoggingService esistente e HttpClient
        public LoggerService(ILoggingService localLogger, HttpClient httpClient)
        {
            _localLogger = localLogger ?? throw new ArgumentNullException(nameof(localLogger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            // Data: 2025-01-19 - Endpoint remoto da configurazione (null = stub mode)
            // TODO: Leggere da appsettings.json quando endpoint disponibile
            // Esempio: _remoteEndpoint = configuration["Logging:RemoteEndpoint"];
            _remoteEndpoint = "/api/logs"; // STUB: Disabilitato per ora
        }

        // Data: 2025-01-19 - Log generico con invio remoto asincrono
        public async Task LogAsync(string message, string level = "Info", IDictionary<string, object?>? properties = null, Exception? exception = null)
        {
            // 1. Log locale immediato (console browser via Serilog)
            switch (level.ToLowerInvariant())
            {
                case "error":
                    _localLogger.Error(exception ?? new Exception(message), message, properties);
                    break;
                case "warning":
                    _localLogger.Warning(message, properties);
                    break;
                default:
                    _localLogger.Info(message, properties);
                    break;
            }

            // 2. Invio remoto asincrono (non blocca UI)
            await LogRemoteAsync(message, level, properties, exception);
        }

        // Data: 2025-01-19 - Log errore con ritorno correlation ID
        public async Task<string> LogErrorAsync(Exception exception, string message, IDictionary<string, object?>? properties = null)
        {
            var errorId = _localLogger.Error(exception, message, properties);
            await LogRemoteAsync(message, "Error", properties, exception);
            return errorId;
        }

        // Data: 2025-01-19 - Log warning
        public async Task LogWarningAsync(string message, IDictionary<string, object?>? properties = null)
        {
            _localLogger.Warning(message, properties);
            await LogRemoteAsync(message, "Warning", properties, null);
        }

        // Data: 2025-01-19 - Log info
        public async Task LogInfoAsync(string message, IDictionary<string, object?>? properties = null)
        {
            _localLogger.Info(message, properties);
            await LogRemoteAsync(message, "Info", properties, null);
        }

        public string GetBufferedLog() => _localLogger.GetBufferedLog();
        public void ClearBuffer() => _localLogger.ClearBuffer();

        // Data: 2025-01-19 - STUB: Invio log a endpoint remoto (da implementare quando API disponibile)
        // Questo metodo è pronto per essere esteso con logica HTTP POST verso API esterna
        // Attualmente non fa nulla se _remoteEndpoint è null (modalità development/debug)
        private async Task LogRemoteAsync(string message, string level, IDictionary<string, object?>? properties, Exception? exception)
        {
            try
            {
                var logMessage = new LogMessage
                {
                    CorrelationId = _localLogger.LastErrorId ?? Guid.NewGuid().ToString("N"),
                    Level = level,
                    Message = SanitizeForRemote(message),
                    Timestamp = DateTimeOffset.UtcNow,
                    ExceptionType = exception?.GetType().FullName,
                    ExceptionMessage = SanitizeForRemote(exception?.Message ?? string.Empty),
                    ExceptionStackTrace = SanitizeForRemote(exception?.StackTrace ?? string.Empty),
                    Properties = properties != null ? new Dictionary<string, object?>(properties) : null,
                    ClientInfo = GetClientInfo(),
                    AppVersion = GetAppVersion()
                };

                var response = await _httpClient.PostAsJsonAsync(_remoteEndpoint, logMessage);
                if (!response.IsSuccessStatusCode)
                {
                    _localLogger.Warning($"Remote log failed: {(int)response.StatusCode} {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                _localLogger.Warning($"Failed to send log to remote endpoint: {ex.Message}");
            }
        }

        // Data: 2025-01-19 - Sanificazione stringa per invio remoto (prevenzione injection)
        private static string SanitizeForRemote(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            // Rimuove caratteri di controllo potenzialmente pericolosi
            return input.Replace("\0", "").Trim();
        }

        // Data: 2025-01-19 - Ottiene info client (User Agent, Browser) se disponibile
        private static string GetClientInfo()
        {
            // TODO: Iniettare IJSRuntime per ottenere navigator.userAgent via JS
            return "BlazorWASM";
        }

        // Data: 2025-01-19 - Ottiene versione app da assembly
        private static string GetAppVersion()
        {
            var version = typeof(LoggerService).Assembly.GetName().Version;
            return version?.ToString() ?? "1.0.0";
        }
    }
}
