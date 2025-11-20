using Microsoft.AspNetCore.Components;
using MudBlazor;
using SaipemE_PTW.Components.Base;
using SaipemE_PTW.Services;
using SaipemE_PTW.Services.Dashboard;
using SaipemE_PTW.Shared.Models.Dashboard;


namespace SaipemE_PTW.Pages
{
    public partial class Home : CommonComponentBase
    {
        private DashboardStatsDto? _dashboardStats;
        private ChartDataDto? _historicalData;
        public Position LegendPosicion { get; set; } = Position.Start;

        [Inject] private IDashboardService DashboardService { get; set; } = default!;


        protected override async Task OnInitializedCoreAsync()
        {
            try
            {
                // Log info con contesto utente al caricamento Home
                //await LogInfoAsync("Home initialization started", new Dictionary<string, object?>
                //{
                //    ["Page"] = "Home",
                //    ["Action"] = "Initialize"
                //});

                SetBreadcrumb(new BreadcrumbItem(Localization.GetString("Home.Dashboard"), href: "/"));


                _dashboardStats = await DashboardService.GetDashboardStatsAsync();
                _historicalData = await DashboardService.GetHistoricalTrendDataAsync();
                //_areaDistributionData = await DashboardService.GetAreaDistributionDataAsync();
                //_weeklyPlanningData = await DashboardService.GetWeeklyPlanningDataAsync();
                // _certificationData = await DashboardService.GetCertificationDistributionDataAsync();
                //_conflictsTrendData = await DashboardService.GetConflictsTrendDataAsync();

                Logger.Info("Dati dashboard caricati con successo");
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, ex.Message, new Dictionary<string, object?>
                {
                    ["Page"] = _localArea,
                    ["Action"] = _sublocalArea
                });

            }
            finally
            {
                IsLoading = false;
            }
        }

        #region Andamento Storico
        private string[] xLabels = new[] { "1/10", "2/10", "3/10", "4/10", "5/10", "6/10", "7/10" };

        private List<ChartSeries> lineSeries = new()
    {
        new ChartSeries
        {
            Name = "Permessi attivi",
            Data = new double[] { 120, 132, 101, 134, 90, 230,100 }
        },
        new ChartSeries
        {
            Name = "Personale attivo",
            Data = new double[] { 220, 182, 191, 234, 290, 330,110 }
        },
         new ChartSeries
        {
            Name = "Conflitti",
            Data = new double[] { 40, 213, 44, 178, 270, 300,120 }
        }
    };

        private int selectedPeriod = 0;

        private void SelectPeriod(int index)
        {
            selectedPeriod = index;
            switch (index)
            {
                case 0:
                    xLabels = new[] { "1/10", "2/10", "3/10", "4/10", "5/10", "6/10", "7/10" };
                    lineSeries = new()
                {
                    new ChartSeries
                    {
                        Name = "Permessi attivi",
                        Data = new double[] { 120, 132, 101, 134, 90, 230,100 }
                    },
                    new ChartSeries
                    {
                        Name = "Personale attivo",
                        Data = new double[] { 220, 182, 191, 234, 290, 330,110 }
                    },
                     new ChartSeries
                    {
                        Name = "Conflitti",
                        Data = new double[] { 40, 213, 44, 178, 270, 300,120 }
                    }
                };
                    break;
                case 1:
                    xLabels = new[] { "1/10", "5/10", "10/10", "15/10", "20/10", "25/10", "30/10" };
                    lineSeries = new()
                {
                    new ChartSeries
                    {
                        Name = "Permessi attivi",
                        Data = new double[] { 120, 343, 56, 134, 134, 230,65 }
                    },
                    new ChartSeries
                    {
                        Name = "Personale attivo",
                        Data = new double[] { 100, 182, 211, 89, 290, 330,321 }
                    },
                     new ChartSeries
                    {
                        Name = "Conflitti",
                        Data = new double[] { 232, 123, 90, 145, 189, 243,90 }
                    }
                };
                    break;
                case 2:
                    xLabels = new[] { "1/10", "15/10", "30/10", "1/11", "15/11", "20/11", "30/11" };
                    lineSeries = new()
                {
                    new ChartSeries
                    {
                        Name = "Permessi attivi",
                        Data = new double[] { 90, 123, 134, 167, 78, 200,230 }
                    },
                    new ChartSeries
                    {
                        Name = "Personale attivo",
                        Data = new double[] { 220, 45, 67, 140, 243, 253,90 }
                    },
                     new ChartSeries
                    {
                        Name = "Conflitti",
                        Data = new double[] { 134, 67, 232, 300, 43, 232,160 }
                    }
                };
                    break;
            }
            InvokeAsync(StateHasChanged);
            // TODO: aggiorna i dati del grafico in base al periodo selezionato
        }

        private string GetButtonClass(int index)
        {
            return selectedPeriod == index ? "bg-orange text-white" : "";
        }
        #endregion
    }
}
