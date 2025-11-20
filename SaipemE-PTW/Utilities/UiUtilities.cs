using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SaipemE_PTW.Utilities
{
    /// <summary>
    /// UI Utilities for Blazor WebAssembly.
    /// 
    /// Sicurezza e linee guida (uso in UI):
    /// - Evita ambiguità tra orari Local/UTC utilizzando metodi che normalizzano DateTime in UTC (Kind=Utc).
    /// - Non genera eccezioni per input non validi: i metodi di parsing ritornano null quando l'input è invalido.
    /// - Utilizza CultureInfo.InvariantCulture per prevenire problemi di localization e possibili injection via formattazione.
    /// - Usa TimeZoneInfo passato esplicitamente, oppure TimeZoneInfo.Local come fallback, per conversioni deterministiche.
    /// - Formattazione ISO-8601 conforme (yyyy-MM-ddTHH:mm:ss.fffZ) per serializzazione sicura in UI/API.
    /// 
    /// Note Blazor WebAssembly:
    /// - In ambienti browser il fuso orario locale è quello del client. TimeZoneInfo.Local riflette il browser, ma può variare.
    /// - Per display o conversioni coerenti fra client multipli, preferire UTC lato UI e convertire solo al rendering finale.
    /// 
    /// Esempi d'uso:
    /// - EnsureUtc: normalizza qualsiasi DateTime in UTC (gestione Kind Local/Unspecified/UTC) prima di salvare/mostrare.
    /// - FormatIso8601Utc: converte/formatta in stringa ISO-8601 con suffisso Z per standardizzare il trasporto dei dati.
    /// - ParseIso8601ToUtc: parse sicuro da string a DateTime (UTC) senza eccezioni.
    /// </summary>
    public static class UiUtilities
    {
        // Data: 2025-10-08 - Converte qualsiasi DateTime in UTC in modo sicuro
        /// <summary>
        /// Converte un <see cref="DateTime"/> in UTC garantendo Kind=Utc.
        /// - Se il valore è già in UTC, viene ritornato invariato.
        /// - Se il valore è Local, usa <see cref="DateTime.ToUniversalTime"/>.
        /// - Se il valore è Unspecified, usa il <paramref name="sourceTimeZone"/> (o <see cref="TimeZoneInfo.Local"/>) per la conversione.
        /// Gestisce in sicurezza orari ambigui (es. cambio DST) e valori ai limiti, evitando eccezioni.
        /// </summary>
        /// <param name="value">Il valore da convertire.</param>
        /// <param name="sourceTimeZone">Fuso orario di origine quando <paramref name="value"/> ha Kind=Unspecified; se null usa TimeZoneInfo.Local.</param>
        /// <returns>Un <see cref="DateTime"/> in UTC con Kind=Utc.</returns>
        public static DateTime EnsureUtc(DateTime value, TimeZoneInfo? sourceTimeZone = null)
        {
            if (value.Kind == DateTimeKind.Utc)
                return value;

            if (value.Kind == DateTimeKind.Local)
                return value.ToUniversalTime();

            // Unspecified ? interpreta come ora nel fuso indicato (o locale) e converte in UTC
            var tz = sourceTimeZone ?? TimeZoneInfo.Local;
            var unspecified = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);
            try
            {
                return TimeZoneInfo.ConvertTimeToUtc(unspecified, tz);
            }
            catch (ArgumentException)
            {
                // Ambiguous/invalid local time: fallback conservativo ? tratta come Local e usa ToUniversalTime
                return DateTime.SpecifyKind(value, DateTimeKind.Local).ToUniversalTime();
            }
            catch (Exception)
            {
                // Fallback finale: preserva il valore e marca come UTC (evita crash UI)
                return DateTime.SpecifyKind(value, DateTimeKind.Utc);
            }
        }

        // Data: 2025-10-08 - Formatta una data in ISO-8601 UTC (Z)
        /// <summary>
        /// Converte e formatta un <see cref="DateTime"/> in stringa ISO-8601 UTC con suffisso 'Z'.
        /// Esempio output: 2025-01-31T14:05:12.123Z
        /// </summary>
        /// <param name="value">Data/ora da convertire e formattare.</param>
        /// <param name="sourceTimeZone">Fuso orario di origine se <paramref name="value"/> è Unspecified.</param>
        /// <returns>Stringa ISO-8601 in UTC sicura per UI/API.</returns>
        public static string FormatIso8601Utc(DateTime value, TimeZoneInfo? sourceTimeZone = null)
        {
            var utc = EnsureUtc(value, sourceTimeZone);
            return utc.ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture);
        }

        // Data: 2025-10-08 - Parsing sicuro ISO-8601 ? UTC
        /// <summary>
        /// Effettua il parsing sicuro di una stringa ISO-8601 in un <see cref="DateTime"/> UTC.
        /// Non solleva eccezioni: ritorna null in caso di input nullo/vuoto/invalid.
        /// </summary>
        /// <param name="input">Stringa data in formato ISO-8601 (es. "2025-01-31T14:05:12Z").</param>
        /// <returns><see cref="DateTime"/> in UTC o null se parsing fallisce.</returns>
        public static DateTime? ParseIso8601ToUtc(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            // Usa DateTimeOffset per gestire offset e normalizzare a UTC senza eccezioni
            if (DateTimeOffset.TryParse(input.Trim(), CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out var dto))
            {
                return dto.UtcDateTime;
            }

            return null;
        }
    }

    public class DateCoverter : JsonConverter<DateTime>
    {
        private string _date = "dd/MM/yyyy";
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString(), _date, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_date));
        }
    }
}
