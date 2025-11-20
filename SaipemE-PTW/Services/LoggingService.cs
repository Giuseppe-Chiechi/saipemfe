using Serilog;
using Serilog.Events;
using System.Collections.Concurrent;

namespace SaipemE_PTW.Services
{
    // Data: 2025-10-16 - Interfaccia per logging applicativo sicuro con Serilog
    public interface ILoggingService
    {
        void Info(string message, IDictionary<string, object?>? properties = null);
        void Warning(string message, IDictionary<string, object?>? properties = null);
        string Error(Exception ex, string message, IDictionary<string, object?>? properties = null);
        string GetBufferedLog();
        void ClearBuffer();
        string? LastErrorId { get; }
    }

    // Data: 2025-10-16 - Implementazione per Blazor WebAssembly: scrive su console browser e mantiene buffer testo per download
    public sealed class LoggingService : ILoggingService
    {
        private static bool _configured;
        private static readonly object _sync = new();
        private readonly ConcurrentQueue<string> _buffer = new();
        private const int MaxBufferedLines = 2000; // prevenzione OOM - Data: 2025-10-16

        public string? LastErrorId { get; private set; }

        public LoggingService()
        {
            // Data: 2025-10-16 - Configurazione Serilog una volta sola, sicura per thread
            if (!_configured)
            {
                lock (_sync)
                {
                    if (!_configured)
                    {
                        Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .Enrich.WithProperty("App", "SaipemE-PTW")
                            .Enrich.FromLogContext()
                            .WriteTo.BrowserConsole(restrictedToMinimumLevel: LogEventLevel.Information)
                            .CreateLogger();
                        _configured = true;
                    }
                }
            }
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        public void Info(string message, IDictionary<string, object?>? properties = null)
        {
            // Data: 2025-10-16 - Logging informativo con proprietà opzionali, prevenzione injection log
            if (properties is not null)
            {
                using (LogContextPush(properties))
                {
                    Log.Information(Sanitize(message));
                }
            }
            else
            {
                Log.Information(Sanitize(message));
            }
            Buffer($"INFO | {DateTimeOffset.UtcNow:o} | {message}");
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        public void Warning(string message, IDictionary<string, object?>? properties = null)
        {
            if (properties is not null)
            {
                using (LogContextPush(properties))
                {
                    Log.Warning(Sanitize(message));
                }
            }
            else
            {
                Log.Warning(Sanitize(message));
            }
            Buffer($"WARN | {DateTimeOffset.UtcNow:o} | {message}");
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        public string Error(Exception ex, string message, IDictionary<string, object?>? properties = null)
        {
            // Data: 2025-10-16 - Genera correlation id per tracciare l'errore in modo sicuro
            var errorId = Guid.NewGuid().ToString("N");
            LastErrorId = errorId;

            var safeMsg = Sanitize(message);
            var safeEx = Sanitize(ex?.Message ?? string.Empty);

            if (properties is null) properties = new Dictionary<string, object?>();
            properties["CorrelationId"] = errorId;
            properties["ExceptionType"] = ex?.GetType().FullName;

            using (LogContextPush(properties))
            {
                // Nota: in WASM scriviamo su console; dettagli completi sono disponibili in DevTools
                Log.Error(ex, safeMsg + (string.IsNullOrWhiteSpace(safeEx) ? string.Empty : $" | Ex: {safeEx}"));
            }

            Buffer($"ERRO | {DateTimeOffset.UtcNow:o} | {safeMsg} | Id={errorId} | {safeEx}");
            return errorId;
        }

        /// <summary>
        /// GetBufferedLog
        /// </summary>
        /// <returns></returns>
        public string GetBufferedLog()
        {
            // Data: 2025-10-16 - Restituisce log in-memory come testo scaricabile
            return string.Join(Environment.NewLine, _buffer.ToArray());
        }

        public void ClearBuffer()
        {
            while (_buffer.TryDequeue(out _)) { }
        }

        private static string Sanitize(string input)
        {
            // Data: 2025-10-16 - Sanificazione minima per prevenire log injection e XSS quando stampato in UI
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
        }

        private void Buffer(string line)
        {
            _buffer.Enqueue(line);
            while (_buffer.Count > MaxBufferedLines && _buffer.TryDequeue(out _)) { }
        }

        private static IDisposable LogContextPush(IDictionary<string, object?> props)
        {
            // Data: 2025-10-16 - Push multiplo di proprietà nel LogContext
            var disposables = new List<IDisposable>(props.Count);
            foreach (var kv in props)
            {
                disposables.Add(Serilog.Context.LogContext.PushProperty(kv.Key, kv.Value));
            }
            return new AggregateDisposable(disposables);
        }

        // Data: 2025-10-16 - Helper per disposable aggregato
        private sealed class AggregateDisposable : IDisposable
        {
            private readonly IEnumerable<IDisposable> _items;
            public AggregateDisposable(IEnumerable<IDisposable> items) => _items = items;
            public void Dispose()
            {
                foreach (var d in _items) d.Dispose();
            }
        }
    }
}
