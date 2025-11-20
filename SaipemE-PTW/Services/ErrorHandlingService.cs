using Microsoft.AspNetCore.Components;
using SaipemE_PTW.Services;

namespace SaipemE_PTW.Services
{
    // Data: 2025-10-16 - Servizio centralizzato per gestione errori globali (non UI): log + redirect pagina errore
    public interface IErrorHandlingService
    {
        void HandleGlobal(Exception ex, string message, IDictionary<string, object?>? properties = null);
    }

    // Data: 2025-10-16 - Implementazione che registra log con Serilog e naviga verso /error con correlation id
    public sealed class ErrorHandlingService(ILoggingService logger, NavigationManager nav) : IErrorHandlingService
    {
        private readonly ILoggingService _logger = logger;
        private readonly NavigationManager _nav = nav;

        public void HandleGlobal(Exception ex, string message, IDictionary<string, object?>? properties = null)
        {
            var id = _logger.Error(ex, message, properties);
            // Data: 2025-10-16 - Navigazione sicura verso pagina errore con correlation id
            var uri = $"/error?id={Uri.EscapeDataString(id)}";
            _nav.NavigateTo(uri, forceLoad: false);
        }
    }
}
