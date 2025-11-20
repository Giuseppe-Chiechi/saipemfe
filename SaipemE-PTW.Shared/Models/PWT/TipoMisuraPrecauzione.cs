//PrecautionMeasureType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{

    public class TipoMisuraPrecauzioneDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoMisuraPrecauzione
    {
        [Display(Name = "Comunicazione Radio", ShortName = "", Description = "")]
        ComunicazioneRadio = 1,

        [Display(Name = "Imbracatori qualificati", ShortName = "", Description = "")]
        ImbracatoriQualificati = 2,

        [Display(Name = "Movieri dedicati", ShortName = "", Description = "")]
        MovieriDedicati = 3,

        [Display(Name = "PIC (Sollevamenti)", ShortName = "", Description = "")]
        PICSollevamenti = 4,

        [Display(Name = "Uso di materiale anti scintilla", ShortName = "", Description = "")]
        UsoMaterialeAntiScintilla = 5,

        [Display(Name = "Rilevazione gas in continuo (LEL, O2, H2S, etc.)", ShortName = "", Description = "")]
        RilevazioneGasContinuo = 6,

        [Display(Name = "Test atmosfera (Gas Test)", ShortName = "", Description = "")]
        TestAtmosferaGasTest = 7,

        [Display(Name = "Teli/Schermi di protezione ignifughi", ShortName = "", Description = "")]
        TeliSchermiProtezioneIgnifughi = 8,

        [Display(Name = "Apparecchiatura depressurizzata", ShortName = "", Description = "")]
        ApparecchiaturaDepressurizzata = 9,

        [Display(Name = "Apparecchiatura drenata", ShortName = "", Description = "")]
        ApparecchiaturaDrenata = 10,

        [Display(Name = "Barriere rigide", ShortName = "", Description = "")]
        BarriereRigide = 11,

        [Display(Name = "Segnaletica", ShortName = "", Description = "")]
        Segnaletica = 12,

        [Display(Name = "Targetta Ponteggi (Scafftag)", ShortName = "", Description = "")]
        TargettaPonteggiScafftag = 13,

        [Display(Name = "Estintore: Polvere", ShortName = "", Description = "")]
        EstintorePolvere = 14,

        [Display(Name = "Estintore: CO₂", ShortName = "", Description = "")]
        EstintoreCO2 = 15,

        [Display(Name = "Altro", ShortName = "", Description = "")]
        Altro = 16

       


    }

    public static class TipoMisuraPrecauzioneMapper
    {
        public static TipoMisuraPrecauzioneDto ToDto(this TipoMisuraPrecauzione tipo)
        {
            return new TipoMisuraPrecauzioneDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoMisuraPrecauzioneDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoMisuraPrecauzione))
                .Cast<TipoMisuraPrecauzione>()
                .Select(e => e.ToDto());
        }
    }
}
