// Data: 2025-01-19 - Implementazione servizio localizzazione con file .resx
// Data: 2025-01-19 - FIX: Compatibilità Blazor WASM con caricamento esplicito risorse
// Sicurezza: validazione input, gestione errori, fallback sicuro
using System.Globalization;
using System.Resources;
using System.Reflection;
using SaipemE_PTW.Services.Common;

namespace SaipemE_PTW.Services;

/// <summary>
/// Servizio singleton per gestione localizzazione stringhe tramite .resx
/// Integrato con LanguageService per aggiornamento automatico
/// Ottimizzato per Blazor WebAssembly
/// </summary>
public class LocalizationService : ILocalizationService
{
    private readonly ILanguageService _languageService;
    private readonly ILoggingService _logger;
    private readonly ResourceManager _resourceManager;

    // Data: 2025-01-19 - Evento per notificare cambio localizzazione
    public event EventHandler? LocalizationChanged;

    public LocalizationService(ILanguageService languageService, ILoggingService logger)
    {
        _languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Data: 2025-01-19 - Inizializza ResourceManager per file AppResources.resx
        _resourceManager = new ResourceManager(
            "SaipemE_PTW.Resources.AppResources",
            Assembly.GetExecutingAssembly()
        );

        // Data: 2025-01-19 - IMPORTANTE: Ignora cache ResourceManager per Blazor WASM
        _resourceManager.IgnoreCase = false;

        // Data: 2025-01-19 - Sottoscrivi evento cambio lingua per aggiornare localizzazione
        _languageService.LanguageChanged += OnLanguageChanged;

        _logger.Info("LocalizationService initialized for Blazor WASM");
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
            // Data: 2025-01-19 - Formatta stringa con parametri (es: "2 ore fa")
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
        // Data: 2025-01-19 - Validazione input per sicurezza
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
            // Data: 2025-01-19 - FIX BLAZOR WASM: Forza uso cultura specifica
            // ResourceManager.GetString con CultureInfo evita fallback automatico
            var localizedString = _resourceManager.GetString(key, culture);

            if (string.IsNullOrEmpty(localizedString))
            {
                // Data: 2025-01-19 - Fallback: ritorna chiave se risorsa non trovata
                _logger.Warning($"Missing localization for key: {key} (Culture: {culture.Name})");
                return $"[{key}]";
            }

            return localizedString;
        }
        catch (MissingManifestResourceException ex)
        {
            // Data: 2025-01-19 - Errore specifico Blazor WASM: satellite assembly non trovato
            _logger.Error(ex, $"Satellite assembly not found for culture: {culture.Name}. Key: {key}");
            return $"[{key}]";
        }
        catch (Exception ex)
        {
            // Data: 2025-01-19 - Gestione errore sicura
            _logger.Error(ex, $"Error getting localized string for key: {key}");
            return $"[{key}]";
        }
    }

    /// <inheritdoc/>
    public void RefreshLocalization()
    {
        // Data: 2025-01-19 - Notifica componenti del cambio localizzazione
        LocalizationChanged?.Invoke(this, EventArgs.Empty);
        _logger.Info("Localization refreshed");
    }

    // Data: 2025-01-19 - Handler evento cambio lingua dal LanguageService
    private void OnLanguageChanged(object? sender, EventArgs e)
    {
        _logger.Info($"Language changed to: {_languageService.GetCurrentLanguageCode()}, refreshing localization");
        RefreshLocalization();
    }
}
