//WorkType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{
    public class TipoLavoroDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }         
        public string? Nome { get; set; }           
        public string? Descrizione { get; set; }    
    }

    public enum TipoLavoro
    {
        [Display(Name = "Lavori in quota", ShortName = "", Description = "")]
        LavorQuota = 1,

        [Display(Name = "SollevaMenti", ShortName = "", Description = "")]
        SollevaMenti = 2,

        [Display(Name = "Lavori a caldo/fuoco", ShortName = "", Description = "")]
        LavoriCaldoFuoco = 3,

        [Display(Name = "Altro...", ShortName = "", Description = "")]
        Altro = 4
    }

    public static class TipoLavoroMapper
    {
        public static TipoLavoroDto ToDto(this TipoLavoro tipo)
        {
            return new TipoLavoroDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoLavoroDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoLavoro))
                .Cast<TipoLavoro>()
                .Select(e => e.ToDto());
        }
    }



    
}
