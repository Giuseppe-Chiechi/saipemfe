using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.Logger
{
    // Data: 2025-01-19 - DTO per invio log verso endpoint remoto (HTTP)
    // Utilizzato per serializzazione JSON verso API esterna di logging
    public class LogMessage
    {
        /// <summary>
        /// Correlation ID univoco per tracciare l'errore
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Livello di log: "Info", "Warning", "Error"
        /// </summary>
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// Messaggio descrittivo dell'evento
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp UTC dell'evento
        /// </summary>
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Tipo di eccezione (se presente)
        /// </summary>
        public string? ExceptionType { get; set; }

        /// <summary>
        /// Messaggio dell'eccezione (se presente)
        /// </summary>
        public string? ExceptionMessage { get; set; }

        /// <summary>
        /// Stack trace dell'eccezione (se presente)
        /// </summary>
        public string? ExceptionStackTrace { get; set; }

        /// <summary>
        /// Proprietà custom aggiuntive (serializzate come JSON)
        /// </summary>
        public Dictionary<string, object?>? Properties { get; set; }

        /// <summary>
        /// Informazioni sul client (es. UserAgent, Browser)
        /// </summary>
        public string? ClientInfo { get; set; }

        /// <summary>
        /// Versione dell'applicazione
        /// </summary>
        public string AppVersion { get; set; } = "1.0.0";
    }
}
