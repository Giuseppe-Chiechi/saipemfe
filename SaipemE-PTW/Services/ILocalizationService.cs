//// Data: 2025-01-19 - Interfaccia per servizio di localizzazione basato su file .resx
//// Sicurezza: validazione chiavi, gestione fallback, thread-safe
//using System.Globalization;

//namespace SaipemE_PTW.Services;

///// <summary>
///// Servizio per gestione localizzazione stringhe tramite file .resx
///// Supporta cambio lingua dinamico e notifiche
///// </summary>
//public interface ILocalizationService
//{
//    /// <summary>
//    /// Ottiene una stringa localizzata per la chiave specificata
//    /// </summary>
//    /// <param name="key">Chiave risorsa (es: "Home.Dashboard")</param>
//    /// <returns>Stringa localizzata o chiave se non trovata</returns>
//    string GetString(string key);

//    /// <summary>
//    /// Ottiene una stringa localizzata con parametri formattati
//    /// </summary>
//    /// <param name="key">Chiave risorsa</param>
//    /// <param name="args">Argomenti per formattazione</param>
//    /// <returns>Stringa localizzata formattata</returns>
//    string GetString(string key, params object[] args);

//    /// <summary>
//    /// Ottiene una stringa localizzata per una cultura specifica
//    /// </summary>
//    /// <param name="key">Chiave risorsa</param>
//    /// <param name="culture">Cultura specifica</param>
//    /// <returns>Stringa localizzata</returns>
//    string GetString(string key, CultureInfo culture);

//    /// <summary>
//    /// Evento sollevato quando la lingua corrente viene modificata
//    /// </summary>
//    event EventHandler? LocalizationChanged;

//    /// <summary>
//    /// Forza il refresh delle risorse localizzate
//    /// </summary>
//    void RefreshLocalization();
//}
