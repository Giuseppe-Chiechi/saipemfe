// Data: 2025-01-19 - Interfaccia servizio per gestione multilingua (bilinguismo IT/EN)
// Sicurezza: validazione input CultureInfo, gestione cookie sicura, notifica eventi cambio lingua
using System.Globalization;

namespace SaipemE_PTW.Services;

/// <summary>
/// Servizio per gestione lingua applicazione con supporto cookie e notifiche cambio lingua
/// Rispetta standard WCAG 2.2 per accessibilità
/// </summary>
public interface ILanguageService
{
    /// <summary>
    /// Evento sollevato quando la lingua corrente viene modificata
    /// </summary>
    event EventHandler? LanguageChanged;

    /// <summary>
    /// Ottiene la cultura corrente dell'applicazione
    /// </summary>
    CultureInfo GetCurrentCulture();

    /// <summary>
    /// Ottiene il codice lingua corrente (es: "it-IT", "en-GB")
    /// </summary>
    string GetCurrentLanguageCode();

    /// <summary>
    /// Ottiene il codice lingua breve (es: "IT", "EN")
    /// </summary>
    string GetCurrentLanguageCodeShort();

    /// <summary>
    /// Imposta la lingua corrente, salva nel cookie e notifica i componenti
    /// </summary>
    /// <param name="culture">Cultura da impostare (validata per sicurezza)</param>
    Task SetLanguageAsync(CultureInfo culture);

    /// <summary>
    /// Inizializza la lingua da cookie o da user agent
    /// </summary>
    Task InitializeLanguageAsync();

    /// <summary>
    /// Ottiene tutte le lingue supportate dall'applicazione
    /// </summary>
    IReadOnlyList<CultureInfo> GetSupportedCultures();

    /// <summary>
    /// Verifica se una cultura è supportata
    /// </summary>
    bool IsCultureSupported(string cultureName);
}
