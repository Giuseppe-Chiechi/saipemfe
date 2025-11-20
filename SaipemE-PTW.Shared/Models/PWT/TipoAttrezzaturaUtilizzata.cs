//UsedEquipmentType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{

    

    public class TipoAttrezzaturaUtilizzataDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoAttrezzaturaUtilizzata
    {
        [Display(Name = "Gru", ShortName = "", Description = "")]
        Gru = 1,

        [Display(Name = "Autogru", ShortName = "", Description = "")]
        Autogru = 2,

        [Display(Name = "Carrelli elevatori (Forklift)", ShortName = "", Description = "")]
        CarrelliElevatori = 3,

        [Display(Name = "Torre Faro", ShortName = "", Description = "")]
        TorreFaro = 4,

        [Display(Name = "Saldatrici", ShortName = "", Description = "")]
        Saldatrici = 5,

        [Display(Name = "Autobetoniere", ShortName = "", Description = "")]
        Autobetoniere = 6,

        [Display(Name = "Autobetonpompa", ShortName = "", Description = "")]
        Autobetonpompa = 6,

        [Display(Name = "PLE – piattaforme aeree (scissor lift, boom lift)", ShortName = "", Description = "")]
        PLEpiattaforme = 6,

        [Display(Name = "PLE – piattaforme aeree", ShortName = "", Description = "")]
        PLEpiattaFormeAeree = 6,

        [Display(Name = "Attrezzatura manuale", ShortName = "", Description = "")]
        AttrezzaturaManuale = 7,

        [Display(Name = "Impianto ossiacetilenico", ShortName = "", Description = "")]
        ImpiantoOssiacetilenico = 8,

        [Display(Name = "Macchine tagliatubi (pipe cutter)", ShortName = "", Description = "")]
        MacchineTagliatubi = 9,

        [Display(Name = "Filettatrici per tubazioni", ShortName = "", Description = "")]
        Filettatrici = 10,

        [Display(Name = "Trapani a colonna/portatili", ShortName = "", Description = "")]
        TrapaniPortatili = 11,

        [Display(Name = "Piegatubi", ShortName = "", Description = "")]
        Piegatubi = 11,

        [Display(Name = "Martelli demolitori elettrici/pneumatici", ShortName = "", Description = "")]
        MartelliDemolitoriElettriciPneumatici = 11,

        [Display(Name = "Levigatrici e smerigliatrici angolari (grinder)", ShortName = "", Description = "")]
        LevigatriciSmerigliatrici = 11,

        [Display(Name = "Paranchi manuali", ShortName = "", Description = "")]
        ParanchiManuali = 12,

        [Display(Name = "Ponteggi metallici", ShortName = "", Description = "")]
        PonteggiMetallici = 13,

        [Display(Name = "Trabattelli", ShortName = "", Description = "")]
        Trabattelli = 14,

        [Display(Name = "Scale a torre", ShortName = "", Description = "")]
        ScaleTorre = 15,

        [Display(Name = "Frese portatili", ShortName = "", Description = "")]
        FresePortatili = 16,

        [Display(Name = "Seghe a nastro", ShortName = "", Description = "")]
        SegheNastro = 17,

        [Display(Name = "Lampade portatili", ShortName = "", Description = "")]
        LampadePortatili = 18,

        [Display(Name = "Linee vita temporanee", ShortName = "", Description = "")]
        LineeVitaTemporanee = 19,

        [Display(Name = "Paranchi elettrici", ShortName = "", Description = "")]
        ParanchiElettrici = 20,

        [Display(Name = "Motopompe", ShortName = "", Description = "")]
        Motopompe = 21,

        [Display(Name = "Generatori corrente", ShortName = "", Description = "")]
        GeneratoriCorrente = 22,

        [Display(Name = "Rulli per tubi", ShortName = "", Description = "")]
        RulliTubi = 23,

        [Display(Name = "Dispositivi anticaduta", ShortName = "", Description = "")]
        DispositiviAnticaduta = 24,

        [Display(Name = "Cavalletti", ShortName = "", Description = "")]
        Cavalletti = 25,

        [Display(Name = "Martinetti idraulici", ShortName = "", Description = "")]
        MartinettiIdraulici = 26,

        [Display(Name = "Paranco a fune", ShortName = "", Description = "")]
        ParancoFune = 27,

        [Display(Name = "Chiave oleodinamica", ShortName = "", Description = "")]
        ChiaveOleodinamica = 28,

        [Display(Name = "Altro", ShortName = "", Description = "")]
        Altro = 29,

    }

    public static class TipoAttrezzaturaUtilizzataMapper
    {
        public static TipoAttrezzaturaUtilizzataDto ToDto(this TipoAttrezzaturaUtilizzata tipo)
        {
            return new TipoAttrezzaturaUtilizzataDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoAttrezzaturaUtilizzataDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoAttrezzaturaUtilizzata))
                .Cast<TipoAttrezzaturaUtilizzata>()
                .Select(e => e.ToDto());
        }
    }
}
