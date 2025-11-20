//WorkPermitStatusType.cs
using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    /// <summary>
    /// Elenco degli stati del Permesso di Lavoro (PdL).
    /// </summary>
    
    public class TipoStatoPermessoLavoroDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoStatoPermessoLavoro
    {
        [Display(Name = "In Lavorazione RA", ShortName = "", Description = "Salvato in Bozza dall’Autorità Richiedente (RA)")]
        InLavorazioneRA = 1,

        [Display(Name = "In Lavorazione PA", ShortName = "", Description = "Salvato in Bozza, Inoltrato dall’Autorità Richiedente (RA) ed è in compilazione in carico all’Autorità Esecutrice (PA)")]
        InLavorazionePA = 2,

        [Display(Name = "In Lavorazione IA", ShortName = "", Description = "Il PdL è stato Salvato in Bozza, Inoltrato dall’Autorità Richiedente (RA), Inoltrato dall’Autorità Esecutrice (PA) ed è in compilazione in carico all’Autorità Emittente (IA).")]
        InLavorazioneIA = 3,

        [Display(Name = "In Lavorazione PTWC", ShortName = "", Description = "Il PdL è stato Salvato in Bozza, Inoltrato dall’Autorità Richiedente (RA), Inoltrato dall’Autorità Esecutrice (PA), Inoltrato dall’Autorità Emettente (IA), ed è in Presa Visione in carico al PTW Coordinator (PTWC).")]
        InLavorazionePTWC = 4,

        [Display(Name = "In Lavorazione CSE", ShortName = "", Description = "xxx")]
        InLavorazioneCSE = 5,

        [Display(Name = "In Lavorazione AGT", ShortName = "", Description = "xxx")]
        InLavorazioneAGT = 6,

        [Display(Name = "In Approvazione PA", ShortName = "", Description = "xxx")]
        InApprovazionePA = 7,

        [Display(Name = "In Approvazione OA", ShortName = "", Description = "xxx")]
        InApprovazioneOA = 8,


        [Display(Name = "Autorizzato", ShortName = "", Description = "xxx")]
        Autorizzato = 9,

        [Display(Name = "Scaduto", ShortName = "", Description = "xxx")]
        Scaduto = 10,

        [Display(Name = "Sospeso", ShortName = "", Description = "xxx")]
        Sospeso = 11,

        [Display(Name = "Rinnovato", ShortName = "", Description = "xxx")]
        Rinnovato = 12,

        [Display(Name = "Riattivato", ShortName = "", Description = "xxx")]
        Riattivato = 13,


        [Display(Name = "Chiuso", ShortName = "", Description = "xxx")]
        Chiuso = 14,

        [Display(Name = "Rigettato PA", ShortName = "", Description = "Il PdL è stato rigettato dall’Autorità Esecutrice (PA). Il PdL torna in compilazione dall’Autorità Richiedente (RA);")]
        RigettatoPA = 15,

        [Display(Name = "Rigettato PTWC", ShortName = "", Description = "xxx")]
        RigettatoPTWC = 16,

        [Display(Name = "Rigettato CSE", ShortName = "", Description = "xxx")]
        RigettatoCSE = 17,

        [Display(Name = "Rigettato OA", ShortName = "", Description = "xxx")]
        RigettatoOA = 18,

       
    }

    public static class TipoStatoPermessoLavoroMapper
    {
        public static TipoStatoPermessoLavoroDto ToDto(this TipoStatoPermessoLavoro tipo)
        {
            return new TipoStatoPermessoLavoroDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoStatoPermessoLavoroDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoStatoPermessoLavoro))
                .Cast<TipoStatoPermessoLavoro>()
                .Select(e => e.ToDto());
        }
    }



}
