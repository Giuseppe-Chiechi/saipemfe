using SaipemE_PTW.Shared.Models.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaipemE_PTW.Services.Dashboard
{
    // Data: 2025-01-19 - Interfaccia servizio per dati dashboard con pattern async sicuro
    // Fornisce dati mock per grafici MudChart e statistiche
    /// <summary>
    /// Servizio per recupero dati dashboard e grafici
    /// Implementazione mock per sviluppo, sostituibile con servizio reale
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Recupera statistiche principali dashboard
        /// </summary>
        /// <returns>DTO con conteggi principali validati</returns>
        Task<DashboardStatsDto> GetDashboardStatsAsync();

        /// <summary>
        /// Recupera dati per grafico andamento storico
        /// </summary>
        /// <returns>Dataset con trend temporali</returns>
        Task<ChartDataDto> GetHistoricalTrendDataAsync();

        /// <summary>
        /// Recupera dati distribuzione per area impianto
        /// </summary>
        /// <returns>Dataset con distribuzione personale</returns>
        Task<ChartDataDto> GetAreaDistributionDataAsync();

        /// <summary>
        /// Recupera dati pianificazione settimanale permessi
        /// </summary>
        /// <returns>Dataset con permessi programmati</returns>
        Task<ChartDataDto> GetWeeklyPlanningDataAsync();

        /// <summary>
        /// Recupera dati distribuzione certificazioni personale
        /// </summary>
        /// <returns>Dataset con competenze certificate</returns>
        Task<ChartDataDto> GetCertificationDistributionDataAsync();

        /// <summary>
        /// Recupera trend conflitti/interferenze nel tempo
        /// </summary>
        /// <returns>Dataset con serie temporale conflitti</returns>
        Task<ChartDataDto> GetConflictsTrendDataAsync();
    }
}
