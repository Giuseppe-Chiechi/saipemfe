//CertificateType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{
   

    public class TipoCertificatoDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoCertificato
    {
        [Display(Name = "Scavo", ShortName = "", Description = "")]
        Scavo = 1,

        [Display(Name = "Spazio confinato", ShortName = "", Description = "")]
        SpazioConfinato = 2,

        [Display(Name = "Isolamento energetico", ShortName = "", Description = "")]
        IsolamentoEnergetico = 3,

        [Display(Name = "Rimozione parapetti/aperture piano calpestio", ShortName = "", Description = "")]
        RimozioneParapettiAperture = 4,

        [Display(Name = "N.A.", ShortName = "", Description = "")]
        NA = 5,

    }

    public static class TipoCertificatoMapper
    {
        public static TipoCertificatoDto ToDto(this TipoCertificato tipo)
        {
            return new TipoCertificatoDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoCertificatoDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoCertificato))
                .Cast<TipoCertificato>()
                .Select(e => e.ToDto());
        }
    }
}
