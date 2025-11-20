using SaipemE_PTW.Shared.Models.Menu;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace SaipemE_PTW.Services.Common
{
    /// <summary>
    /// Servizio per gestione menu applicazione con supporto localizzazione
    /// Data: 2025-11-04
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly ILocalizationService _localization;

        public MenuService(ILocalizationService localization)
        {
            _localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }

        public Task<List<MenuCard>> GetMenuAsync(CultureInfo language)
        {
            // Usa la cultura passata per ottenere le stringhe localizzate
            var cards = new List<MenuCard>
            {
                new MenuCard
                {
                    id = 1,
                    Title = _localization.GetString("Menu.Activities", language),
                    Icon = "NotificationsActive",
                    Sections = new List<MenuSection>
                    {
                        new MenuSection
                        {
                            id = 2,
                            Header = _localization.GetString("Menu.PendingCompilation", language),
                            Items = new List<MenuItem>
                            {
                                new MenuItem
                                {
                                    id = 3,
                                    Text = _localization.GetString("Menu.HotWorkPermits", language),
                                    Counter = "2",
                                    Link = "/activity/general-hot/list",
                                    Icon = "",
                                    Descrizione = _localization.GetString("Menu.HotWorkPermitsDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 3,
                                    //Text = "Permessi Lavoro Caldo",
                                    Text = "<span class='strike'>Permessi Lavoro Caldo</span>",
                                    Counter = "2",
                                    Link = "/activity/hot/list",
                                    Icon = "",
                                    Descrizione = _localization.GetString("Menu.HotWorkPermitsDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 4,
                                    Text = "<span class='strike'>"+_localization.GetString("Menu.ColdWorkPermits", language)+"</span>",
                                    //Text = _localization.GetString("Menu.ColdWorkPermits", language),
                                    Counter = "2",
                                    Link = "/activity/cold/list",
                                    Icon = "",
                                    Descrizione = _localization.GetString("Menu.ColdWorkPermitsDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 5,
                                    Text = "<span class='strike'>"+_localization.GetString("Menu.RadiograhicActivityPermits", language)+"</span>",
                                    //Text = _localization.GetString("Menu.RadiograhicActivityPermits", language),
                                    Counter = "2",
                                    Link = "/activity/radiographic/list",
                                    Icon = "",
                                    Descrizione = _localization.GetString("Menu.RadiograhicActivityPermitsDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 6,
                                    Text = _localization.GetString("Menu.EnergyIsolationCertificates", language),
                                    Counter = "1",
                                    Link = "/activity/certificate-open-energy-insulation/list",//isolamento-energetico-aperti-list
                                    Icon = "",
                                    Descrizione = _localization.GetString("Menu.EnergyIsolationCertificatesDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 7,
                                    Text = _localization.GetString("Menu.ParapetRemovalCertificates", language),
                                    Counter = "1",
                                    Link = "/activity/certificate-removal-parapets-floor-openings/list",
                                    Icon = "",
                                    Descrizione = _localization.GetString("Menu.ParapetRemovalCertificatesDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 8,
                                    Text = _localization.GetString("Menu.ExcavationCertificates", language),
                                    Counter = "1",
                                    Link = "/activity/certificate-open-excavations/list",
                                    Icon = "",
                                    Descrizione = _localization.GetString("Menu.ExcavationCertificatesDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 9,
                                    Text = _localization.GetString("Menu.ConfinedSpaceCertificates", language),
                                    Counter = "1",
                                    Link = "/activity/certificate-confined-spaces-opened/list",
                                    Icon = "",
                                    Descrizione = _localization.GetString("Menu.ConfinedSpaceCertificatesDesc", language)
                                }
                            }
                        }
                    }
                },
                new MenuCard
                {
                    id = 10,
                    Title = _localization.GetString("Menu.SiteAdministration", language),
                    Icon = "SupervisedUserCircle",
                    Sections = new List<MenuSection>
                    {
                        new MenuSection
                        {
                            id = 11,
                            Header = _localization.GetString("Menu.MasterDataManagement", language),
                            Items = new List<MenuItem>
                            {
                                new MenuItem
                                {
                                    id = 12,
                                    Text = _localization.GetString("Menu.ManageInternalUserRoles", language),
                                    Link = "/administrator/gestione-ruoli-interni",
                                    Icon = "AssignmentInd",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 13,
                                    Text = _localization.GetString("Menu.ManageExternalUserRoles", language),
                                    Link = "/administrator/gestione-ruoli-esterni",
                                    Icon = "ResetTv",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 14,
                                    Text = _localization.GetString("Menu.ManageCompanies", language),
                                    Link = "/administrator/gestione-imprese",
                                    Icon = "AddBusiness",
                                    Descrizione = ""
                                }
                            }
                        }
                    }
                },
                new MenuCard
                {
                    id = 15,
                    Title = _localization.GetString("Menu.WorkflowManagement", language),
                    Icon = "DeviceHub",
                    Sections = new List<MenuSection>
                    {
                       
                        new MenuSection
                        {
                            id = 16,
                            Header = _localization.GetString("Menu.WorkPermits", language),
                            Items = new List<MenuItem>
                            {
                                new MenuItem
                                {
                                    id = 17,
                                    Text = _localization.GetString("Menu.OpenHotWorkPermits", language),
                                    Link = "/workflow/general-hot/list",
                                    Icon = "LocalFireDepartment",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 18,
                                    Text = _localization.GetString("Menu.NewHotWorkPermit", language),
                                    //Link = "/workflow/permesso-lavoro-generico-caldo-view",
                                    Link = "/workflow/general-hot/detail",
                                    Icon = "LocalFireDepartment",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 17,
                                    Text = "Permesso di Lavoro a Caldo",
                                    Link = "/workflow/hot/list",
                                    Icon = "LocalFireDepartment",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 18,
                                    Text = "Nuovo Permesso di Lavoro a Caldo",
                                    //Link = "/workflow/permesso-lavoro-generico-caldo-view",
                                    Link = "/workflow/hot/detail",
                                    Icon = "LocalFireDepartment",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 177,
                                    Text = _localization.GetString("Menu.OpenColdWorkPermits", language),
                                    Link = "/workflow/cold/list",
                                    Icon = "SevereCold",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 19,
                                    Text = _localization.GetString("Menu.NewColdWorkPermit", language),
                                    Link = "/workflow/cold/detail",
                                    Icon = "SevereCold",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 178,
                                    Text = _localization.GetString("Menu.OpenRadiographicActivityPermits", language),
                                    Link = "/workflow/radiographic/list",
                                    Icon = "Gradient",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 20,
                                    Text = _localization.GetString("Menu.NewRadiographicActivityPermit", language),
                                    Link = "/workflow/radiographic/detail",
                                    Icon = "Gradient",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 15161,
                                    Text = _localization.GetString("Menu.OpenEnergyIsolationCertificates", language),
                                    Link = "/workflow/certificate-open-energy-insulation/list",//isolamento-energetico-aperti-list
                                    Icon = "UploadFile",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 15162,
                                    Text = _localization.GetString("Menu.OpenParapetRemovalCertificates", language),
                                    Link = "/workflow/certificate-removal-parapets-floor-openings/list",
                                    Icon = "UploadFile",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 15163,
                                    Text = _localization.GetString("Menu.OpenExcavationCertificates", language),
                                    Link = "/workflow/certificate-open-excavations/list",
                                    Icon = "UploadFile",
                                    Descrizione = ""
                                },
                                new MenuItem
                                {
                                    id = 15164,
                                    Text = _localization.GetString("Menu.OpenConfinedSpaceCertificates", language),
                                    Link = "/workflow/certificate-confined-spaces-opened/list",
                                    Icon = "UploadFile",
                                    Descrizione = ""
                                }
                            },
                        }

                    }
                },
                new MenuCard
                {
                    id = 21,
                    Title = _localization.GetString("Menu.Monitoring", language),
                    Icon = "MonitorHeart",
                    Sections = new List<MenuSection>
                    {
                        new MenuSection
                        {
                            id = 22,
                            Header = _localization.GetString("Menu.OngoingActivities", language),
                            Items = new List<MenuItem>
                            {
                                new MenuItem
                                {
                                    id = 23,
                                    Text = _localization.GetString("Menu.HotWorkPermitsList", language),
                                    Link = "/monitoring/general-hot/list",
                                    Icon = "LocalFireDepartment",
                                    Descrizione = _localization.GetString("Menu.HotWorkPermitsListDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 23,
                                    Text ="Lista Permessi Lavoro Caldo",
                                    Link = "/monitoring/hot/list",
                                    Icon = "LocalFireDepartment",
                                    Descrizione = _localization.GetString("Menu.HotWorkPermitsListDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 24,
                                    Text = _localization.GetString("Menu.ColdWorkPermitsList", language),
                                    Link = "/monitoring/cold/list",
                                    Icon = "SevereCold",
                                    Descrizione = _localization.GetString("Menu.ColdWorkPermitsListDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 25,
                                    Text = _localization.GetString("Menu.RadiograhicActivityPermitsList", language),
                                    Link = "/monitoring/radiographic/list",
                                    Icon = "Gradient",
                                    Descrizione = _localization.GetString("Menu.RadiograhicActivityPermitsListDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 26,
                                    Text = _localization.GetString("Menu.EnergyIsolationCertificatesList", language),
                                    Link = "/monitoring/certificate-open-energy-insulation/list",//isolamento-energetico-aperti-list
                                    Icon = "UploadFile",
                                    Descrizione = _localization.GetString("Menu.EnergyIsolationCertificatesListDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 27,
                                    Text = _localization.GetString("Menu.ParapetRemovalCertificatesList", language),
                                     Link = "/monitoring/certificate-removal-parapets-floor-openings/list",
                                    Icon = "UploadFile",
                                    Descrizione = _localization.GetString("Menu.ParapetRemovalCertificatesListDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 28,
                                    Text = _localization.GetString("Menu.ExcavationCertificatesList", language),
                                     Link = "/monitoring/certificate-open-excavations/list",
                                    Icon = "UploadFile",
                                    Descrizione = _localization.GetString("Menu.ExcavationCertificatesListDesc", language)
                                },
                                new MenuItem
                                {
                                    id = 29,
                                    Text = _localization.GetString("Menu.ConfinedSpaceCertificatesList", language),
                                     Link = "/monitoring/certificate-confined-spaces-opened/list",
                                    Icon = "UploadFile",
                                    Descrizione = _localization.GetString("Menu.ConfinedSpaceCertificatesListDesc", language)
                                }
                            }
                        }
                    }
                }
            };

            return Task.FromResult(cards);
        }
    }
}