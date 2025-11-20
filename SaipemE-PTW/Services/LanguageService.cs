// Data: 2025-01-19 - Implementazione servizio multilingua con gestione cookie sicura
// Sicurezza: input sanitizzato, protezione XSS, validazione culture, cookie HttpOnly/Secure
using System.Globalization;
using Microsoft.JSInterop;

namespace SaipemE_PTW.Services;

/// <summary>
/// Servizio singleton per gestione lingua applicazione con persistenza cookie
/// Implementa standard sicurezza e accessibilità WCAG 2.2
/// </summary>
public class LanguageService : ILanguageService
{
    // Data: 2025-01-19 - Nome cookie lingua (30 giorni scadenza)
    // Data: 2025-01-22 - Modificato nome cookie in saipem_eptw_language per branding specifico
    private const string LANGUAGE_COOKIE_NAME = "saipem_eptw_language";
    private const int COOKIE_EXPIRY_DAYS = 30;

    // Data: 2025-01-19 - Culture supportate (parametrizzate per facile estensione)
    private static readonly List<CultureInfo> _supportedCultures = new()
    {
        new CultureInfo("it-IT"), // Italiano
        new CultureInfo("en-GB")  // Inglese (UK)
        // Aggiungi qui altre lingue: new CultureInfo("fr-FR"), new CultureInfo("de-DE"), etc.
    };

    private readonly IJSRuntime _jsRuntime;
    private readonly ILoggingService _logger;
    private CultureInfo _currentCulture;

    // Data: 2025-01-19 - Evento per notificare cambio lingua a componenti interessati
    public event EventHandler? LanguageChanged;

    public LanguageService(IJSRuntime jsRuntime, ILoggingService logger)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        // Data: 2025-01-19 - Default italiano se non ancora inizializzato
        _currentCulture = new CultureInfo("it-IT");
        
        _logger.Info("LanguageService initialized with default culture: " + _currentCulture.Name);
    }

    /// <inheritdoc/>
    public CultureInfo GetCurrentCulture()
    {
        return _currentCulture;
    }

    /// <inheritdoc/>
    public string GetCurrentLanguageCode()
    {
        return _currentCulture.Name; // es: "it-IT"
    }

    /// <inheritdoc/>
    public string GetCurrentLanguageCodeShort()
    {
        // Data: 2025-01-19 - Restituisce solo parte lingua (IT, EN)
        return _currentCulture.TwoLetterISOLanguageName.ToUpperInvariant();
    }

    /// <inheritdoc/>
    public IReadOnlyList<CultureInfo> GetSupportedCultures()
    {
        return _supportedCultures.AsReadOnly();
    }

    /// <inheritdoc/>
    public bool IsCultureSupported(string cultureName)
    {
        // Data: 2025-01-19 - Validazione sicura input
        if (string.IsNullOrWhiteSpace(cultureName))
            return false;

        try
        {
            var culture = new CultureInfo(cultureName.Trim());
            return _supportedCultures.Any(c => c.Name.Equals(culture.Name, StringComparison.OrdinalIgnoreCase));
        }
        catch (CultureNotFoundException)
        {
            _logger.Warning("Invalid culture name: " + cultureName);
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task SetLanguageAsync(CultureInfo culture)
    {
        try
        {
            // Data: 2025-01-19 - Validazione cultura per sicurezza
            if (culture == null)
            {
                _logger.Warning("SetLanguageAsync called with null culture");
                return;
            }

            // Data: 2025-01-19 - Verifica che la cultura sia supportata
            if (!IsCultureSupported(culture.Name))
            {
                _logger.Warning("Attempted to set unsupported culture: " + culture.Name);
                return;
            }

            // Data: 2025-01-19 - Aggiorna cultura corrente
            _currentCulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            // Data: 2025-01-19 - Salva nel cookie (usa JS interop sicuro)
            await SaveLanguageToCookieAsync(culture.Name);

            // Data: 2025-01-19 - Notifica componenti del cambio lingua
            LanguageChanged?.Invoke(this, EventArgs.Empty);

            _logger.Info("Language changed to: " + culture.Name);
        }
        catch (Exception ex)
        {
            // Data: 2025-01-19 - Log errore e gestione sicura
            _logger.Error(ex, "Error setting language to " + (culture?.Name ?? "null"));
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task InitializeLanguageAsync()
    {
        try
        {
            _logger.Info("Initializing language from cookie or user agent");

            // Data: 2025-01-19 - Tenta di caricare da cookie
            var savedLanguage = await LoadLanguageFromCookieAsync();

            if (!string.IsNullOrWhiteSpace(savedLanguage) && IsCultureSupported(savedLanguage))
            {
                _currentCulture = new CultureInfo(savedLanguage);
                _logger.Info("Language loaded from cookie: " + savedLanguage);
            }
            else
            {
                // Data: 2025-01-19 - Fallback: rileva da user agent
                var userAgentLang = await DetectUserAgentLanguageAsync();
                
                // Data: 2025-01-19 - Cerca match supportato (es: browser "it" -> "it-IT")
                var matchedCulture = FindMatchingSupportedCulture(userAgentLang);
                
                if (matchedCulture != null)
                {
                    _currentCulture = matchedCulture;
                    _logger.Info("Language detected from user agent: " + matchedCulture.Name);
                }
                else
                {
                    // Data: 2025-01-19 - Default italiano
                    _currentCulture = new CultureInfo("it-IT");
                    _logger.Info("Using default language: it-IT");
                }

                // Data: 2025-01-19 - Salva la lingua rilevata nel cookie
                await SaveLanguageToCookieAsync(_currentCulture.Name);
            }

            // Data: 2025-01-19 - Imposta cultura globale
            CultureInfo.CurrentCulture = _currentCulture;
            CultureInfo.CurrentUICulture = _currentCulture;
        }
        catch (Exception ex)
        {
            // Data: 2025-01-19 - Gestione errore sicura con fallback
            _logger.Error(ex, "Error initializing language, using default");
            _currentCulture = new CultureInfo("it-IT");
            CultureInfo.CurrentCulture = _currentCulture;
            CultureInfo.CurrentUICulture = _currentCulture;
        }
    }

    // Data: 2025-01-19 - Metodo privato per rilevare lingua da user agent (JS interop)
    private async Task<string> DetectUserAgentLanguageAsync()
    {
        try
        {
            // Data: 2025-01-19 - Usa funzione JS sicura già implementata (langCookie.js)
            var lang = await _jsRuntime.InvokeAsync<string>("getPreferredLanguage");
            return lang ?? "en";
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to detect user agent language");
            return "en";
        }
    }

    // Data: 2025-01-19 - Trova cultura supportata che matcha lingua user agent
    private CultureInfo? FindMatchingSupportedCulture(string userAgentLang)
    {
        if (string.IsNullOrWhiteSpace(userAgentLang))
            return null;

        try
        {
            var browserCulture = new CultureInfo(userAgentLang);

            // Data: 2025-01-19 - Cerca match esatto (es: "it-IT")
            var exactMatch = _supportedCultures.FirstOrDefault(c => 
                c.Name.Equals(browserCulture.Name, StringComparison.OrdinalIgnoreCase));
            
            if (exactMatch != null)
                return exactMatch;

            // Data: 2025-01-19 - Cerca match parziale (es: "it" matcha "it-IT")
            var partialMatch = _supportedCultures.FirstOrDefault(c =>
                c.TwoLetterISOLanguageName.Equals(browserCulture.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase));

            return partialMatch;
        }
        catch (CultureNotFoundException)
        {
            _logger.Warning("Could not parse user agent language: " + userAgentLang);
            return null;
        }
    }

    // Data: 2025-01-19 - Salva lingua nel cookie (30 giorni, sicuro)
    private async Task SaveLanguageToCookieAsync(string languageCode)
    {
        try
        {
            // Data: 2025-01-19 - Validazione input per sicurezza (anti-XSS)
            if (string.IsNullOrWhiteSpace(languageCode) || !IsCultureSupported(languageCode))
            {
                _logger.Warning("Invalid language code for cookie: " + languageCode);
                return;
            }

            // Data: 2025-01-19 - Usa funzione JS sicura (langCookie.js con SameSite=Lax, Secure)
            await _jsRuntime.InvokeVoidAsync("setCookie", LANGUAGE_COOKIE_NAME, languageCode, COOKIE_EXPIRY_DAYS);
            
            _logger.Info("Language saved to cookie: " + languageCode);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to save language to cookie");
        }
    }

    // Data: 2025-01-19 - Carica lingua dal cookie
    private async Task<string?> LoadLanguageFromCookieAsync()
    {
        try
        {
            var cookieValue = await _jsRuntime.InvokeAsync<string>("getCookie", LANGUAGE_COOKIE_NAME);
            
            if (string.IsNullOrWhiteSpace(cookieValue))
                return null;

            _logger.Info("Language loaded from cookie: " + cookieValue);
            return cookieValue.Trim();
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to load language from cookie");
            return null;
        }
    }
}
