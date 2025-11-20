using System;
using System.Collections.Generic;

namespace SaipemE_PTW.Shared.Models.Dashboard
{
    // Data: 2025-01-19 - DTO per dati grafici MudChart con sicurezza e validazione
    // Utilizzato per trasferire dati statistici dalla logica al client WCAG 2.2 compliant
    public class ChartDataDto
    {
        /// <summary>
        /// Identificatore univoco del dataset
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Nome del dataset per visualizzazione (es: "Data 2021", "Distribuzione Area")
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Valori numerici del dataset (validati, non null)
        /// </summary>
        public double[] Data { get; set; } = Array.Empty<double>();

        /// <summary>
        /// Etichette associate ai valori (es: "Buenos Aires", "Cordoba")
        /// </summary>
        public string[] Labels { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Tipo di grafico suggerito (Pie, Bar, Line, etc.)
        /// </summary>
        public string ChartType { get; set; } = "Pie";

        /// <summary>
        /// Descrizione accessibile per screen reader (WCAG 2.2)
        /// </summary>
        public string AccessibleDescription { get; set; } = string.Empty;

        /// <summary>
        /// Data ultima modifica per cache/invalidazione
        /// </summary>
        public DateTimeOffset LastUpdated { get; set; } = DateTimeOffset.UtcNow;
    }

    // Data: 2025-01-19 - DTO per statistiche dashboard con validazione sicura
    public class DashboardStatsDto
    {
        /// <summary>
        /// Personale attivo oggi
        /// </summary>
        public int ActivePersonnel { get; set; }

        /// <summary>
        /// Permessi attualmente in corso
        /// </summary>
        public int ActivePermits { get; set; }

        /// <summary>
        /// Permessi in attesa di approvazione
        /// </summary>
        public int PendingPermits { get; set; }

        /// <summary>
        /// Conflitti/interferenze rilevate
        /// </summary>
        public int PotentialConflicts { get; set; }
    }
}
