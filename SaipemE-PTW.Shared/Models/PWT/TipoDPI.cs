
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{


    public class TipoDPIDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }


    // "Maschera antipolvere FFP",
    //"Visiera",
    //"Protezione Udito",
    //"Guanti",
    //"Visiera",
    //"Stivali di gomma anti-infortunistici",
    //"Maschera con filtri",
    //"Imbracatura anti-caduta",
    //"Autorespiratore","Altro..."


    public enum TipoDPI
    {
        [Display(Name = "Maschera antipolvere FFP", ShortName = "", Description = "")]
        MascheraAntipolvereFFP = 1,

        [Display(Name = "Visiera", ShortName = "", Description = "")]
        Visiera = 2,

        [Display(Name = "Protezione Udito", ShortName = "", Description = "")]
        ProtezioneUdito = 3,

        [Display(Name = "Guanti", ShortName = "", Description = "")]
        Guanti = 4,

            [Display(Name = "Stivali di gomma anti-infortunistici", ShortName = "", Description = "")]
        StivaliGommaAntiInfortunistici = 5,

            [Display(Name = "Maschera con filtri", ShortName = "", Description = "")]
        MascheraFiltri = 6,

        [Display(Name = "Imbracatura anti-caduta", ShortName = "", Description = "")]
        ImbracaturaAntiCaduta = 7,

        [Display(Name = "Autorespiratore", ShortName = "", Description = "")]
        Autorespiratore = 8,

        [Display(Name = "Altro", ShortName = "", Description = "")]
        Altro = 9
    }

    public static class TipoDPIMapper
    {
        public static TipoDPIDto ToDto(this TipoDPI tipo)
        {
            return new TipoDPIDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoDPIDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoDPI))
                .Cast<TipoDPI>()
                .Select(e => e.ToDto());
        }
    }
}
