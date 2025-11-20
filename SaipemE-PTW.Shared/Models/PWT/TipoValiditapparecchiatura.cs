
using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{


    public class TipoValiditapparecchiaturaDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoValiditapparecchiatura
    {
        [Display(Name = "Validità apparecchiatura  1", ShortName = "", Description = "")]
        ValiditaApparecchiatura1 = 1,

        [Display(Name = "Validità apparecchiatura 2", ShortName = "", Description = "")]
        ValiditaApparecchiatura2 = 2,

        [Display(Name = "Validità apparecchiatura 3", ShortName = "", Description = "")]
        ValiditaApparecchiatura3 = 3

    }

    public static class TipoValiditapparecchiaturaMapper
    {
        public static TipoValiditapparecchiaturaDto ToDto(this TipoValiditapparecchiatura tipo)
        {
            return new TipoValiditapparecchiaturaDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoValiditapparecchiaturaDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoValiditapparecchiatura))
                .Cast<TipoValiditapparecchiatura>()
                .Select(e => e.ToDto());
        }
    }
}
