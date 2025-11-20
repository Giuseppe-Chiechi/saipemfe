namespace SaipemE_PTW.Components.Workflow.List;

/// <summary>
/// Classe per raccogliere tutti i filtri di ricerca della lista permessi lavoro
/// </summary>
public class SearchFilters
{
    public string? SearchText { get; set; }
    public string? ImpresaEsecutrice { get; set; }
    public string? AreaLavoro { get; set; }
    public string? Apparecchiatura { get; set; }
    public string? Stato { get; set; }
    public string? Parte { get; set; }
    public string? Richiedente { get; set; }
    public DateTime? DataInizioFrom { get; set; }
    public DateTime? DataInizioTo { get; set; }
}
