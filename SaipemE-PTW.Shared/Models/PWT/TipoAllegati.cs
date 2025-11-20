//AttachmentType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{



    public class TipoAllegatiDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
        
    }

    public enum TipoAllegati
    {
        [Display(Name = "Layout", ShortName = "", Description = "")]
        Layout = 1,

        [Display(Name = "Scheda POS", ShortName = "", Description = "")]
        SchedaPOS = 2,

        [Display(Name = "MSDS", ShortName = "", Description = "")]
        MSDS = 3,

        [Display(Name = "SWC", ShortName = "", Description = "")]
        SWC = 4,

        [Display(Name = "AIL", ShortName = "", Description = "")]
        AIL = 5,

        [Display(Name = "CHECKLIST SPECIFICHE", ShortName = "", Description = "")]
        CHECKLISTSPECIFICHE = 6,

        [Display(Name = "RELAZIONE ESPERTO QUALIFICATO", ShortName = "", Description = "")]
        RELAZIONEESPERTOQUALIFICATO = 8,

        [Display(Name = "Altro...", ShortName = "", Description = "")]
        Altro = 7

    }

    public static class TipoAllegatiMapper
    {
        public static TipoAllegatiDto ToDto(this TipoAllegati tipo)
        {
            return new TipoAllegatiDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoAllegatiDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoAllegati))
                .Cast<TipoAllegati>()
                .Select(e => e.ToDto());
        }
    }
}
