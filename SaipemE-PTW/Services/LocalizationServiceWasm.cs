// Data: 2025-01-19 - Implementazione localizzazione per Blazor WebAssembly
// Usa dizionari statici invece di ResourceManager per compatibilità WASM
// Sicurezza: validazione input, gestione errori, fallback sicuro
using SaipemE_PTW.Services.Common;
using System.Globalization;

namespace SaipemE_PTW.Services;

/// <summary>
/// Servizio singleton per gestione localizzazione stringhe
/// Ottimizzato per Blazor WebAssembly con dizionari in-memory
/// Integrato con LanguageService per aggiornamento automatico
/// </summary>
public class LocalizationServiceWasm : ILocalizationService
{
    private readonly ILanguageService _languageService;
    private readonly ILoggingService _logger;
    
    // Data: 2025-01-19 - Dizionari per risorse IT/EN (in-memory, no ResourceManager)
    private readonly Dictionary<string, Dictionary<string, string>> _resources;

    // Data: 2025-01-19 - Evento per notificare cambio localizzazione
    public event EventHandler? LocalizationChanged;

    public LocalizationServiceWasm(ILanguageService languageService, ILoggingService logger)
    {
        _languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Data: 2025-01-19 - Inizializza dizionari risorse
        _resources = InitializeResources();

        // Data: 2025-01-19 - Sottoscrivi evento cambio lingua
        _languageService.LanguageChanged += OnLanguageChanged;

        _logger.Info("LocalizationServiceWasm initialized with in-memory dictionaries");
    }

    /// <inheritdoc/>
    public string GetString(string key)
    {
        return GetString(key, _languageService.GetCurrentCulture());
    }

    /// <inheritdoc/>
    public string GetString(string key, params object[] args)
    {
        var localizedString = GetString(key);
        
        try
        {
            return string.Format(localizedString, args);
        }
        catch (FormatException ex)
        {
            _logger.Warning($"Format error for key '{key}': {ex.Message}");
            return localizedString;
        }
    }

    /// <inheritdoc/>
    public string GetString(string key, CultureInfo culture)
    {
        // Data: 2025-01-19 - Validazione input
        if (string.IsNullOrWhiteSpace(key))
        {
            _logger.Warning("GetString called with null or empty key");
            return "[MISSING_KEY]";
        }

        if (culture == null)
        {
            culture = _languageService.GetCurrentCulture();
        }

        try
        {
            // Data: 2025-01-19 - Cerca nel dizionario per cultura
            var cultureName = culture.Name; // "it-IT" o "en-GB"

            if (_resources.TryGetValue(cultureName, out var resourceDict))
            {
                if (resourceDict.TryGetValue(key, out var value))
                {
                    return value;
                }
            }

            // Data: 2025-01-19 - Fallback: risorsa non trovata
            _logger.Warning($"Missing localization for key: {key} (Culture: {cultureName})");
            return $"[{key}]";
        }
        catch (Exception ex)
        {
            _logger.Error(ex, $"Error getting localized string for key: {key}");
            return $"[{key}]";
        }
    }

    /// <inheritdoc/>
    public void RefreshLocalization()
    {
        LocalizationChanged?.Invoke(this, EventArgs.Empty);
        _logger.Info("Localization refreshed");
    }

    // Data: 2025-01-19 - Handler evento cambio lingua
    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        _logger.Info($"Language changed to: {_languageService.GetCurrentLanguageCode()}, refreshing localization");
        RefreshLocalization();
    }

    // Data: 2025-01-19 - Inizializza dizionari risorse (copiati dai file .resx)
    private Dictionary<string, Dictionary<string, string>> InitializeResources()
    {
        return new Dictionary<string, Dictionary<string, string>>
        {
            // ITALIANO (it-IT)
            ["it-IT"] = new Dictionary<string, string>
            {
                ["SiteName"] = "Saipem E-PTW",

                ["Home.Dashboard"] = "Cruscotto",
                ["Home.Activities"] = "Attività",
                ["Home.Workflow"] = "Workflow",
                ["Home.Administration"] = "Amministrazione",
                ["Home.WelcomeText"] = "Questa è la tua nuova dashboard Blazor WebAssembly alimentata da Tailwind CSS. L'interfaccia è completamente responsive e include componenti UI moderni.",
                ["Home.GettingStarted"] = "Per Iniziare",
                ["Home.GettingStartedText"] = "Naviga attraverso il menu laterale per esplorare le diverse sezioni. Personalizza il tema e i componenti nel file di configurazione Tailwind.",
                ["Home.AddView"] = "Aggiungi Vista",
                ["Home.ActiveUsers"] = "Utenti Attivi",
                ["Home.TotalProjects"] = "Progetti Totali",
                ["Home.CompletionRate"] = "Tasso di Completamento",
                ["Home.Revenue"] = "Fatturato",
                ["Home.NewComment"] = "Nuovo commento",
                ["Home.OnYourPost"] = "sul tuo post",
                ["Home.DocumentUploaded"] = "Documento caricato",
                ["Home.Successfully"] = "con successo",
                ["Home.UserSignedOut"] = "Utente disconnesso",
                ["Home.FromTheSystem"] = "dal sistema",
                ["Home.HoursAgo"] = "{0} ore fa",
                ["Home.Yesterday"] = "Ieri",

                // Sidebar
                ["Sidebar.Staff"] = "Personale",
                ["Sidebar.WorkPermits"] = "Permessi di lavoro",
                ["Sidebar.Plant"] = "Impianto",

                ["Sidebar.Attivita"] = "Attività",
                ["Sidebar.AmministrazioneSito"] = "Amministrazione di sito",
                ["Sidebar.GestioneWorkflow"] = "Gestione Flusso Lavoro",
                ["Sidebar.Monitoring"] = "Monitoraggio",

                 ["Settings.User"] = "Impostazioni Utente",
                 ["Settings.SignOut"] = "Esci",

                ["Settings.QuickActions"] = "Azioni rapide",
                ["Settings.Role"] = "Ruolo",
                ["Settings.UserId"] = "ID Utente",
                ["Settings.Email"] = "Email",
                ["Settings.LastName"] = "Cognome",
                ["Settings.FirstName"] = "Nome",
                ["Settings.PersonalInfo"] = "Informazioni personali",
                ["Settings.NotAuthenticatedMessage"] = "Utente non autenticato",
                ["Settings.TechnicalDetails"] = "Dettagli tecnici",


                ["Common.Back"] = "Indietro",

                // Menu - Titoli principali
                ["Menu.Activities"] = "Attività",
                ["Menu.SiteAdministration"] = "Amministrazione di sito",
                ["Menu.WorkflowManagement"] = "Gestione Workflow",
                ["Menu.Monitoring"] = "Monitoring",

                // Menu - Sezioni
                ["Menu.PendingCompilation"] = "In Attesa di Compilazione",
                ["Menu.MasterDataManagement"] = "Gestione Anagrafica",
                ["Menu.WorkPermits"] = "Permessi di Lavoro",
                ["Menu.OngoingActivities"] = "Attività in corso",

                // Menu - Voci Attività
                ["Menu.HotWorkPermits"] = "Permessi Lavoro Generici (caldo)",
                ["Menu.ColdWorkPermits"] = "Permessi Lavoro Freddo",
                ["Menu.RadiograhicActivityPermits"] = "Permessi Lavoro Attività Radiografica",
                ["Menu.EnergyIsolationCertificates"] = "Certificati di Isolamento Energetico",
                ["Menu.ParapetRemovalCertificates"] = "Certificati Rimozione Parapetti Aperture piano di calpestio Strutture",
                ["Menu.ExcavationCertificates"] = "Certificati di Scavo",
                ["Menu.ConfinedSpaceCertificates"] = "Certificati di Spazio Confinato",

                // Menu - Descrizioni
                ["Menu.HotWorkPermitsDesc"] = "Visualizza PdL Generici (Caldo) in compilazione con stato diverso da 'Autorizzato'",
                ["Menu.ColdWorkPermitsDesc"] = "Visualizza PdL a Freddo in compilazione con stato diverso da 'Autorizzato'",
                ["Menu.RadiograhicActivityPermitsDesc"] = "Visualizza PdL Attività Radiografica in compilazione",
                ["Menu.EnergyIsolationCertificatesDesc"] = "Visualizza Certificati Isolamento Energetico in compilazione",
                ["Menu.ParapetRemovalCertificatesDesc"] = "Visualizza Certificati Rimozione Parapetti in compilazione",
                ["Menu.ExcavationCertificatesDesc"] = "Visualizza Certificati di Scavo in compilazione",
                ["Menu.ConfinedSpaceCertificatesDesc"] = "Visualizza Certificati Spazio Confinato in compilazione",

                // Menu - Amministrazione
                ["Menu.ManageInternalUserRoles"] = "Gestione Ruoli Utenti Interni",
                ["Menu.ManageExternalUserRoles"] = "Gestione Ruoli Utenti Esterni",
                ["Menu.ManageCompanies"] = "Gestione Imprese",

                // Menu - Workflow
                ["Menu.OpenHotWorkPermits"] = "Permessi di Lavoro Generici (Caldo) Aperti",
                ["Menu.NewHotWorkPermit"] = "Nuovo permesso lavoro generico (Caldo)",
                ["Menu.OpenColdWorkPermits"] = "Permessi di Lavoro a Freddo Aperti",
                ["Menu.NewColdWorkPermit"] = "Nuovo permesso lavoro a freddo",
                ["Menu.OpenRadiographicActivityPermits"] = "Permessi di Lavoro per Attività Radiografica Aperti",
                ["Menu.NewRadiographicActivityPermit"] = "Nuovo permesso lavoro attività radiografica",
                ["Menu.OpenEnergyIsolationCertificates"] = "Certificati di Isolamento Energetico Aperti",
                ["Menu.OpenParapetRemovalCertificates"] = "Certificati di Rimozione Parapetti Aperti",
                ["Menu.OpenExcavationCertificates"] = "Certificati di Scavo Aperti",
                ["Menu.OpenConfinedSpaceCertificates"] = "Certificati di Spazio Confinato Aperti",

                // Menu - Monitoring
                ["Menu.HotWorkPermitsList"] = "Lista Permessi Lavoro Generici (Caldo)",
                ["Menu.ColdWorkPermitsList"] = "Lista Permessi Lavoro Freddo",
                ["Menu.RadiograhicActivityPermitsList"] = "Lista Permessi Lavoro Attività Radiografica",
                ["Menu.EnergyIsolationCertificatesList"] = "Lista Certificati Isolamento Energetico",
                ["Menu.ParapetRemovalCertificatesList"] = "Lista Certificati Rimozione Parapetti",
                ["Menu.ExcavationCertificatesList"] = "Lista Certificati Scavo",
                ["Menu.ConfinedSpaceCertificatesList"] = "Lista Certificati Spazio Confinato",

                ["Menu.HotWorkPermitsListDesc"] = "Visualizza lista PdL Generici (Caldo) autorizzati",
                ["Menu.ColdWorkPermitsListDesc"] = "Visualizza lista PdL a Freddo autorizzati",
                ["Menu.RadiograhicActivityPermitsListDesc"] = "Visualizza lista PdL Attività Radiografica autorizzati",
                ["Menu.EnergyIsolationCertificatesListDesc"] = "Visualizza lista Certificati Isolamento Energetico autorizzati",
                ["Menu.ParapetRemovalCertificatesListDesc"] = "Visualizza lista Certificati Rimozione Parapetti autorizzati",
                ["Menu.ExcavationCertificatesListDesc"] = "Visualizza lista Certificati Scavo autorizzati",
                ["Menu.ConfinedSpaceCertificatesListDesc"] = "Visualizza lista Certificati Spa"


            },

            // INGLESE (en-GB)
            ["en-GB"] = new Dictionary<string, string>
            {
                ["SiteName"] = "Saipem E-PTW",
                ["Home.Dashboard"] = "Dashboard",
                ["Home.Activities"] = "Activities",
                ["Home.Workflow"] = "Workflow",
                ["Home.Administration"] = "Administration",
                ["Home.WelcomeText"] = "This is your new Blazor WebAssembly dashboard powered by Tailwind CSS. The interface is fully responsive and includes modern UI components.",
                ["Home.GettingStarted"] = "Getting Started",
                ["Home.GettingStartedText"] = "Navigate through the sidebar menu to explore different sections. Customize the theme and components in the Tailwind configuration file.",
                ["Home.AddView"] = "Add View",
                ["Home.ActiveUsers"] = "Active Users",
                ["Home.TotalProjects"] = "Total Projects",
                ["Home.CompletionRate"] = "Completion Rate",
                ["Home.Revenue"] = "Revenue",
                ["Home.NewComment"] = "New comment",
                ["Home.OnYourPost"] = "on your post",
                ["Home.DocumentUploaded"] = "Document uploaded",
                ["Home.Successfully"] = "successfully",
                ["Home.UserSignedOut"] = "User signed out",
                ["Home.FromTheSystem"] = "from the system",
                ["Home.HoursAgo"] = "{0} hours ago",
                ["Home.Yesterday"] = "Yesterday",

                // Sidebar
                ["Sidebar.Staff"] = "Staff",
                ["Sidebar.WorkPermits"] = "Work Permits",
                ["Sidebar.Plant"] = "Plant",

                ["Sidebar.Attivita"] = "Activities",
                ["Sidebar.AmministrazioneSito"] = "Site administration",
                ["Sidebar.GestioneWorkflow"] = "Workflow Management",
                ["Sidebar.Monitoring"] = "Monitoring",

                ["Settings.User"] = "Settings User",
                ["Settings.SignOut"] = "Sign Out",

                ["Settings.QuickActions"] = "Settings.QuickActions",
                ["Settings.Role"] = "Settings.Role",
                ["Settings.UserId"] = "Settings.UserId",
                ["Settings.Email"] = "Settings.Email",
                ["Settings.LastName"] = "Settings.LastName",
                ["Settings.FirstName"] = "Settings.FirstName",
                ["Settings.PersonalInfo"] = "Settings.PersonalInfo",
                ["Settings.NotAuthenticatedMessage"] = "Settings.NotAuthenticatedMessage",
                ["Settings.TechnicalDetails"] = "Settings.TechnicalDetails",
                

                ["Common.Back"] = "Back",


                // Menu - Main titles
                ["Menu.Activities"] = "Activities",
                ["Menu.SiteAdministration"] = "Site Administration",
                ["Menu.WorkflowManagement"] = "Workflow Management",
                ["Menu.Monitoring"] = "Monitoring",

                // Menu - Sections
                ["Menu.PendingCompilation"] = "Pending Compilation",
                ["Menu.MasterDataManagement"] = "Master Data Management",
                ["Menu.WorkPermits"] = "Work Permits",
                ["Menu.OngoingActivities"] = "Ongoing Activities",

                // Menu - Activity items
                ["Menu.HotWorkPermits"] = "Hot Work Permits",
                ["Menu.ColdWorkPermits"] = "Cold Work Permits",
                ["Menu.RadiograhicActivityPermits"] = "Radiographic Activity Permits",
                ["Menu.EnergyIsolationCertificates"] = "Energy Isolation Certificates",
                ["Menu.ParapetRemovalCertificates"] = "Parapet Removal Certificates",
                ["Menu.ExcavationCertificates"] = "Excavation Certificates",
                ["Menu.ConfinedSpaceCertificates"] = "Confined Space Certificates",

                // Menu - Descriptions
                ["Menu.HotWorkPermitsDesc"] = "View Hot Work Permits in compilation with status different from 'Authorized'",
                ["Menu.ColdWorkPermitsDesc"] = "View Cold Work Permits in compilation",
                ["Menu.RadiograhicActivityPermitsDesc"] = "View Radiographic Activity Permits in compilation",
                ["Menu.EnergyIsolationCertificatesDesc"] = "View Energy Isolation Certificates in compilation",
                ["Menu.ParapetRemovalCertificatesDesc"] = "View Parapet Removal Certificates in compilation",
                ["Menu.ExcavationCertificatesDesc"] = "View Excavation Certificates in compilation",
                ["Menu.ConfinedSpaceCertificatesDesc"] = "View Confined Space Certificates in compilation",

                // Menu - Administration
                ["Menu.ManageInternalUserRoles"] = "Manage Internal User Roles",
                ["Menu.ManageExternalUserRoles"] = "Manage External User Roles",
                ["Menu.ManageCompanies"] = "Manage Companies",

                // Menu - Workflow
                ["Menu.OpenHotWorkPermits"] = "Open Hot Work Permits",
                ["Menu.NewHotWorkPermit"] = "New Hot Work Permit",
                ["Menu.OpenColdWorkPermits"] = "Open Cold Work Permits",
                ["Menu.NewColdWorkPermit"] = "New Cold Work Permit",
                ["Menu.OpenRadiographicActivityPermits"] = "Open Radiographic Activity Permits",
                ["Menu.NewRadiographicActivityPermit"] = "New Radiographic Activity Permit",
                ["Menu.OpenEnergyIsolationCertificates"] = "Open Energy Isolation Certificates",
                ["Menu.OpenParapetRemovalCertificates"] = "Open Parapet Removal Certificates",
                ["Menu.OpenExcavationCertificates"] = "Open Excavation Certificates",
                ["Menu.OpenConfinedSpaceCertificates"] = "Open Confined Space Certificates",

                // Menu - Monitoring
                ["Menu.HotWorkPermitsList"] = "Hot Work Permits List",
                ["Menu.ColdWorkPermitsList"] = "Cold Work Permits List",
                ["Menu.RadiograhicActivityPermitsList"] = "Radiographic Activity Permits List",
                ["Menu.EnergyIsolationCertificatesList"] = "Energy Isolation Certificates List",
                ["Menu.ParapetRemovalCertificatesList"] = "Parapet Removal Certificates List",
                ["Menu.ExcavationCertificatesList"] = "Excavation Certificates List",
                ["Menu.ConfinedSpaceCertificatesList"] = "Confined Space Certificates List",

                ["Menu.HotWorkPermitsListDesc"] = "View authorized Hot Work Permits list",
                ["Menu.ColdWorkPermitsListDesc"] = "View authorized Cold Work Permits list",
                ["Menu.RadiograhicActivityPermitsListDesc"] = "View authorized Radiographic Activity Permits list",
                ["Menu.EnergyIsolationCertificatesListDesc"] = "View authorized Energy Isolation Certificates list",
                ["Menu.ParapetRemovalCertificatesListDesc"] = "View authorized Parapet Removal Certificates list",
                ["Menu.ExcavationCertificatesListDesc"] = "View authorized Excavation Certificates list",
                ["Menu.ConfinedSpaceCertificatesListDesc"] = "View authorized Confined Space Certificates list",

            }
        };
    }
}
