//GasType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{
   

    public class TipoGasDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
        public string? Value { get; set; }
    }

    public enum TipoGas
    {
        [Display(Name = "%Oxygen", ShortName = "", Description = "OSSIGENO (O2) 19 - 21%")]
        Oxygen = 1,

        [Display(Name = "%LEL", ShortName = "", Description = "COMBUSTIBILE (LEL): presenza di combustibile rilevato nella misurazione")]
        LEL = 2,

        [Display(Name = "H2S(ppm)", ShortName = "", Description = "ANIDRIDE SOLFROSA (H2S) ppm")]
        H2S = 3,

        [Display(Name = "CO(ppm)", ShortName = "", Description = "MONOSSIDO DI CARBONIO (CO)")]
        CO = 4
    }

    public static class TipoGasMapper
    {
        public static TipoGasDto ToDto(this TipoGas tipo)
        {
            return new TipoGasDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoGasDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoGas))
                .Cast<TipoGas>()
                .Select(e => e.ToDto());
        }
    }
}
