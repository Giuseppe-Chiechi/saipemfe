//AuthorizingUnitType.cs
using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{

    public class TipoUnitaAutorizzantiDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
    }

    public enum TipoUnitaAutorizzanti
    {
        [Display(Name = "Unità autorizzante 1", ShortName = "", Description = "")]
        UnitaAutorizzante1 = 1,

        [Display(Name = "Unità autorizzante 2", ShortName = "", Description = "")]
        UnitaAutorizzante2 = 2,

        [Display(Name = "Unità autorizzante 3", ShortName = "", Description = "")]
        UnitaAutorizzante3 = 3


    }

    public static class TipoUnitaAutorizzantiMapper
    {
        public static TipoUnitaAutorizzantiDto ToDto(this TipoUnitaAutorizzanti tipo)
        {
            return new TipoUnitaAutorizzantiDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<TipoUnitaAutorizzantiDto> GetAll()
        {
            return Enum.GetValues(typeof(TipoUnitaAutorizzanti))
                .Cast<TipoUnitaAutorizzanti>()
                .Select(e => e.ToDto());
        }
    }
}
