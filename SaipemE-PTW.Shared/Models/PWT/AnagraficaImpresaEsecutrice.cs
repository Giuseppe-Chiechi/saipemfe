//ContractorCompany.cs 
using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
  
    public class AnagraficaImpresaEsecutriceDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum AnagraficaImpresaEsecutrice
    {
        [Display(Name = "Impresa Esempio 1", ShortName = "", Description = "")]
        ImpresaEsempio1 = 1,

        [Display(Name = "Impresa Esempio 2", ShortName = "", Description = "")]
        ImpresaEsempio2 = 2,

        [Display(Name = "Impresa Esempio 3", ShortName = "", Description = "")]
        ImpresaEsempio3 = 3,

        [Display(Name = "Impresa Esempio 4", ShortName = "", Description = "")]
        ImpresaEsempio4 = 4,

        [Display(Name = "Impresa Esempio 5", ShortName = "", Description = "")]
        ImpresaEsempio5 = 5,

    }

    public static class AnagraficaImpresaEsecutriceMapper
    {
        public static AnagraficaImpresaEsecutriceDto ToDto(this AnagraficaImpresaEsecutrice tipo)
        {
            return new AnagraficaImpresaEsecutriceDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<AnagraficaImpresaEsecutriceDto> GetAll()
        {
            return Enum.GetValues(typeof(AnagraficaImpresaEsecutrice))
                .Cast<AnagraficaImpresaEsecutrice>()
                .Select(e => e.ToDto());
        }
    }
}
