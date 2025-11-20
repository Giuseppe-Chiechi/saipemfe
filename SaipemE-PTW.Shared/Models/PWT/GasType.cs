//TipoGas.cs;

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class GasType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string? Value { get; set; }
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public ICollection<GasTypeLocalization> Localizations { get; set; } = new List<GasTypeLocalization>();
    }
    public class GasTypeLocalization
    {
        public int Id { get; set; }

        public int GasTypeId { get; set; }
        public GasType GasType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
    public class GasTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
    }
    public static class GasTypeMapper
    {
        public static GasTypeDto ToDto(this GasType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                               ?? entity.Localizations.FirstOrDefault();

            return new GasTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description,
                Value = entity.Value
            };
        }

        public static IEnumerable<GasTypeDto> ToDtoList(this IEnumerable<GasType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
