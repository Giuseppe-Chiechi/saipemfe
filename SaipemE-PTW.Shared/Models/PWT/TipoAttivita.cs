//ActivityType.cs 
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{
    
    public class TipoAttivitaDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoAttivita
    {
        [Display(Name = "Meccanica", ShortName = "", Description = "")]
        Meccanica = 1,

        [Display(Name = "Civile", ShortName = "", Description = "")]
        Civile = 2,

        [Display(Name = "Elettrostrumentale", ShortName = "", Description = "")]
        Elettrostrumentale = 3,

        [Display(Name = "Lavori in quota", ShortName = "", Description = "")]
        LavorQuota = 4,

        [Display(Name = "SollevaMenti", ShortName = "", Description = "")]
        SollevaMenti = 5,

        [Display(Name = "Lavori a caldo/fuoco", ShortName = "", Description = "")]
        LavoriCaldoFuoco = 6,

        [Display(Name = "Altro", ShortName = "", Description = "")]
        Altro = 7

       

    }

    public static class TipoAttivitaMapper
    {
        public static TipoAttivitaDto ToDto(this TipoAttivita tipo)
        {
            return new TipoAttivitaDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoAttivitaDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoAttivita))
                .Cast<TipoAttivita>()
                .Select(e => e.ToDto());
        }
    }
}
