using SaipemE_PTW.Shared.Models.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaipemE_PTW.Services.Dashboard
{
    // Data: 2025-01-19 - Implementazione mock servizio dashboard con dati realistici
    // Fornisce dati di test per grafici MudChart seguendo best practice sicurezza
    /// <summary>
    /// Servizio mock per dati dashboard - Blazor WASM
    /// Restituisce dati fittizi realistici per sviluppo UI
    /// Thread-safe, nessuna dipendenza esterna
    /// </summary>
    public class DashboardService_Mock : IDashboardService
    {
        //private readonly ILoggingService _logger;

        // Data: 2025-01-19 - Dependency injection logging per tracciabilità
        public DashboardService_Mock()
        {
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //_logger.Info("DashboardService_Mock inizializzato - dati mock attivi");
        }

        // Data: 2025-01-19 - Statistiche dashboard principali (valori realistici)
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            try
            {
                var delay = new Random().Next(2000, 6001);
                await Task.Delay(delay);

                var stats = new DashboardStatsDto
                {
                    ActivePersonnel = 12,
                    ActivePermits = 8,
                    PendingPermits = 3,
                    PotentialConflicts = 2
                };

                //_logger.Info("GetDashboardStatsAsync completato con successo");
                return await Task.FromResult(stats);
            }
            catch 
            {
                /*_logger.Error(ex, "Errore recupero statistiche dashboard mock")*/;
                throw;
            }
        }

        // Data: 2025-01-19 - Andamento storico - grafico Pie multi-dataset
        public async Task<ChartDataDto> GetHistoricalTrendDataAsync()
        {
            try
            {
                var delay = new Random().Next(20, 60);
                await Task.Delay(delay);

                var chartData = new ChartDataDto
                {
                    Id = "historical-trend",
                    Name = "Andamento Storico",
                    Data = new double[] { 17.59, 3.89, 3.60, 3.23, 2.00 },
                    Labels = new string[] { "Buenos Aires", "Cordoba", "Santa Fe", "CABA", "Mendoza" },
                    ChartType = "Pie",
                    AccessibleDescription = "Grafico a torta mostrante distribuzione storica tra 5 regioni principali",
                    LastUpdated = DateTimeOffset.UtcNow
                };

                //_logger.Info("GetHistoricalTrendDataAsync completato");
                return await Task.FromResult(chartData);
            }
            catch 
            {
                //_logger.Error(ex, "Errore recupero dati andamento storico");
                throw;
            }
        }

        // Data: 2025-01-19 - Distribuzione per area impianto
        public async Task<ChartDataDto> GetAreaDistributionDataAsync()
        {
            try
            {
                var delay = new Random().Next(100, 201);
                await Task.Delay(delay);

                var chartData = new ChartDataDto
                {
                    Id = "area-distribution",
                    Name = "Distribuzione per Area",
                    Data = new double[] { 25.5, 18.3, 15.7, 22.1, 18.4 },
                    Labels = new string[] { "Area A - Produzione", "Area B - Stoccaggio", "Area C - Logistica", "Area D - Manutenzione", "Area E - Amministrazione" },
                    ChartType = "Pie",
                    AccessibleDescription = "Distribuzione percentuale personale attivo nelle 5 aree principali dell'impianto",
                    LastUpdated = DateTimeOffset.UtcNow
                };

                //_logger.Info("GetAreaDistributionDataAsync completato");
                return await Task.FromResult(chartData);
            }
            catch 
            {
                //_logger.Error(ex, "Errore recupero distribuzione area");
                throw;
            }
        }

        // Data: 2025-01-19 - Pianificazione settimanale permessi lavoro
        public async Task<ChartDataDto> GetWeeklyPlanningDataAsync()
        {
            try
            {
                var delay = new Random().Next(200, 301);
                await Task.Delay(delay);

                var chartData = new ChartDataDto
                {
                    Id = "weekly-planning",
                    Name = "Pianificazione Settimanale",
                    Data = new double[] { 12, 18, 15, 22, 20, 14, 10 },
                    Labels = new string[] { "Lunedì", "Martedì", "Mercoledì", "Giovedì", "Venerdì", "Sabato", "Domenica" },
                    ChartType = "Pie",
                    AccessibleDescription = "Distribuzione permessi di lavoro programmati per i prossimi 7 giorni",
                    LastUpdated = DateTimeOffset.UtcNow
                };

                //_logger.Info("GetWeeklyPlanningDataAsync completato");
                return await Task.FromResult(chartData);
            }
            catch 
            {
                //_logger.Error(ex, "Errore recupero pianificazione settimanale");
                throw;
            }
        }

        // Data: 2025-01-19 - Distribuzione certificazioni personale (StackedBar)
        public async Task<ChartDataDto> GetCertificationDistributionDataAsync()
        {
            try
            {
                var delay = new Random().Next(90, 101);
                await Task.Delay(delay);

                var chartData = new ChartDataDto
                {
                    Id = "certification-distribution",
                    Name = "Distribuzione Certificazioni",
                    Data = new double[] { 45, 32, 28, 15, 10 },
                    Labels = new string[] { "Sicurezza Base", "Lavori in Quota", "Spazi Confinati", "Elettrico", "Saldatura" },
                    ChartType = "StackedBar",
                    AccessibleDescription = "Grafico a barre impilate con distribuzione certificazioni di competenza del personale",
                    LastUpdated = DateTimeOffset.UtcNow
                };

                //_logger.Info("GetCertificationDistributionDataAsync completato");
                return await Task.FromResult(chartData);
            }
            catch 
            {
                //_logger.Error(ex, "Errore recupero distribuzione certificazioni");
                throw;
            }
        }

        // Data: 2025-01-19 - Trend conflitti nel tempo (Timeseries)
        public async Task<ChartDataDto> GetConflictsTrendDataAsync()
        {
            try
            {
                // Ritardo simulato tra 2000 e 6000 ms
                var delay = new Random().Next(20, 81);
                await Task.Delay(delay);

                var chartData = new ChartDataDto
                {
                    Id = "conflicts-trend",
                    Name = "Trend Conflitti",
                    Data = new double[] { 5, 3, 7, 4, 2, 6, 3, 8, 4, 2, 5, 3 },
                    Labels = new string[] { "Gen", "Feb", "Mar", "Apr", "Mag", "Giu", "Lug", "Ago", "Set", "Ott", "Nov", "Dic" },
                    ChartType = "Timeseries",
                    AccessibleDescription = "Serie temporale mostrante l'andamento mensile dei conflitti e interferenze rilevate",
                    LastUpdated = DateTimeOffset.UtcNow
                };

                //_logger.Info("GetConflictsTrendDataAsync completato");
                return await Task.FromResult(chartData);
            }
            catch 
            {
                //_logger.Error(ex, "Errore recupero trend conflitti");
                throw;
            }
        }
    }
}
