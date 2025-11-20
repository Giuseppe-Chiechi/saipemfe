//TipoMisuraPrecauzione.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
   
    public class PrecautionMeasureType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public ICollection<PrecautionMeasureTypeLocalization> Localizations { get; set; } = new List<PrecautionMeasureTypeLocalization>();
    }

    public class PrecautionMeasureTypeLocalization
    {
        public int Id { get; set; }

        public int PrecautionMeasureTypeId { get; set; }
        public PrecautionMeasureType PrecautionMeasureType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

    public class PrecautionMeasureTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public static class PrecautionMeasureTypeMapper
    {
        public static PrecautionMeasureTypeDto ToDto(this PrecautionMeasureType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new PrecautionMeasureTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<PrecautionMeasureTypeDto> ToDtoList(this IEnumerable<PrecautionMeasureType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }


}
