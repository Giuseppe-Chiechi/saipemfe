//SiteType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{
  
    public class TipoSitoDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoSito
    {
        [Display(Name = "Ravenna", ShortName = "", Description = "")]
        Sito1 = 1,

        [Display(Name = "Napoli", ShortName = "", Description = "")]
        Sito2 = 2,

        [Display(Name = "Taranto", ShortName = "", Description = "")]
        Sito3 = 3,

        [Display(Name = "Palermo", ShortName = "", Description = "")]
        Sito4 = 4,

        [Display(Name = "Catania", ShortName = "", Description = "")]
        Sito5 = 5,

    }

    public static class TipoSitoMapper
    {
        public static TipoSitoDto ToDto(this TipoSito tipo)
        {
            return new TipoSitoDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoSitoDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoSito))
                .Cast<TipoSito>()
                .Select(e => e.ToDto());
        }
    }
}
