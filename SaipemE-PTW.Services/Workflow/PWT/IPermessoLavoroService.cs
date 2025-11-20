using SaipemE_PTW.Shared.Models.PWT;

namespace SaipemE_PTW.Services.Workflow.PWT
{
    // Data: 2025-10-21 - Interfaccia servizio Permessi di Lavoro (mock microservizio)
    // Scopo: definire contratti async per recuperare dati PermessoLavoroModel in Blazor WASM in modo sicuro
    public interface IPermessoLavoroService
    {
        /// <summary>
        /// Recupera elenco permessi di lavoro con supporto cancellazione e logging.
        /// </summary>
        /// <param name="ct">CancellationToken per interrompere in sicurezza la chiamata simulata</param>
        /// <returns>Elenco in sola lettura di PermessoLavoroModel</returns>
        Task<IEnumerable<PermessoLavoroModel>> GetPermessiAsync(CancellationToken ct = default);

        /// <summary>
        /// Recupera un singolo permesso per identificativo sicuro.
        /// </summary>
        /// <param name="pdl">Identificativo del permesso (sanificato)</param>
        /// <param name="ct">CancellationToken per interrompere la chiamata</param>
        /// <returns>PermessoLavoroModel oppure null se non trovato</returns>
        Task<PermessoLavoroModel?> GetPermessoByIdAsync(string pdl, CancellationToken ct = default);
    }
}
