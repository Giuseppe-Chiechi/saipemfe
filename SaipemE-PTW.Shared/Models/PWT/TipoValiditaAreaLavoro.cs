//WorkAreaValidityType.cs
using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
  
    public class TipoValiditaAreaLavoroDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoValiditaAreaLavoro
    {
        [Display(Name = "Validità Area Lavoro 1", ShortName = "", Description = "")]
        ValiditaAreaLavoro1 = 1,

        [Display(Name = "Validità Area Lavoro 2", ShortName = "", Description = "")]
        ValiditaAreaLavoro2 = 2,

        [Display(Name = "Validità Area Lavoro 3", ShortName = "", Description = "")]
        ValiditaAreaLavoro3 = 3

    }

    public static class TipoValiditaAreaLavoroMapper
    {
        public static TipoValiditaAreaLavoroDto ToDto(this TipoValiditaAreaLavoro tipo)
        {
            return new TipoValiditaAreaLavoroDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoValiditaAreaLavoroDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoValiditaAreaLavoro))
                .Cast<TipoValiditaAreaLavoro>()
                .Select(e => e.ToDto());
        }
    }
}
