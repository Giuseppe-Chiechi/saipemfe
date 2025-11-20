//AuthorityType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{
   

    public class TipoAutoritaDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoAutorita
    {
        [Display(Name = "SubAppaltatore", ShortName = "", Description = "")]
        SubAppaltatore = 1,

        //[Display(Name = "Fornitore", ShortName = "", Description = "")]
        //Fornitore = 2,
        [Display(Name = "SubSp. II LIV", ShortName = "", Description = "")]
        SubSpIILIV = 3

    }

    public static class TipoAutoritaMapper
    {
        public static TipoAutoritaDto ToDto(this TipoAutorita tipo)
        {
            return new TipoAutoritaDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoAutoritaDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoAutorita))
                .Cast<TipoAutorita>()
                .Select(e => e.ToDto());
        }
    }
}
