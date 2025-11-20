

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class ListaRinnovoLavoro
    {
        public DateTime? DataInizio { get; set; }
        public DateTime? DataFine { get; set; }
        public string? Autorizzante { get; set; }
        public string? Modulo { get; set; }
        public string? Note { get; set; }
    }

    public class ListaSospensione
    {
        public DateTime? DataInizioSospensione { get; set; }
        public DateTime? DataRiavvio { get; set; }
        public TimeSpan? OraRiavvio { get; set; }
        public string? AutorizzanteSospensione { get; set; }
        public string? Motivo { get; set; }
        public string? Note { get; set; }
        public string? AutorizzanteRiavvio { get; set; }
    }

    public class ListaRiattivazione
    {
        public DateTime? DataInizioSospensione { get; set; }
        public DateTime? DataRiavvio { get; set; }
        public TimeSpan? OraRiavvio { get; set; }
        public string? AutorizzanteSospensione { get; set; }
        public string? Motivo { get; set; }
        public string? Note { get; set; }
        public string? AutorizzanteRiavvio { get; set; }
    }
}
