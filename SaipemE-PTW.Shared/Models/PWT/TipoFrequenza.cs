//FrequencyType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{
   

    public class TipoFrequenzaDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoFrequenza
    {
        [Display(Name = "Solo prima dell'avvio", ShortName = "", Description = "")]
        PrimoAvvio = 1,

        [Display(Name = "Ad ogni rinnovo", ShortName = "", Description = "")]
        OgniRinnovo = 2,

        [Display(Name = "In continuo", ShortName = "", Description = "")]
        InContinuo = 3

    }

    public static class TipoFrequenzaMapper
    {
        public static TipoFrequenzaDto ToDto(this TipoFrequenza tipo)
        {
            return new TipoFrequenzaDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoFrequenzaDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoFrequenza))
                .Cast<TipoFrequenza>()
                .Select(e => e.ToDto());
        }
    }
}
