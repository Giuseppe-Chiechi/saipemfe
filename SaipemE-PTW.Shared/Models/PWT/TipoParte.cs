//WorkPermitSectionType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{

    public class TipoParteDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoParte
    {
        [Display(Name = "Parte 1", ShortName = "", Description = "")]
        Parte1 = 1,

        [Display(Name = "Parte 2", ShortName = "", Description = "")]
        Parte2 = 2,

        [Display(Name = "CSE", ShortName = "", Description = "")]
        CSE = 3,

        [Display(Name = "Parte 3", ShortName = "", Description = "")]
        ParteCSE = 4,

        [Display(Name = "Rinnovo", ShortName = "", Description = "")]
        Rinnovo = 5,

        [Display(Name = "Sospensione", ShortName = "", Description = "")]
        Sospensione = 6,

        [Display(Name = "Riattivazione", ShortName = "", Description = "")]
        Riattivazione = 7,

        [Display(Name = "Chiusura", ShortName = "", Description = "")]
        Chiusura = 8




    }

    public static class TipoParteMapper
    {
        public static TipoParteDto ToDto(this TipoParte tipo)
        {
            return new TipoParteDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoParteDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoParte))
                .Cast<TipoParte>()
                .Select(e => e.ToDto());
        }
    }
}
