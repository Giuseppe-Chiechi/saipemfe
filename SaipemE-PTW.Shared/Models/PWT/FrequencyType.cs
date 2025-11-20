//TipoFrequenza.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class FrequencyType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public ICollection<FrequencyTypeLocalization> Localizations { get; set; } = new List<FrequencyTypeLocalization>();
    }

    public class FrequencyTypeLocalization
    {
        public int Id { get; set; }

        public int FrequencyTypeId { get; set; }
        public FrequencyType FrequencyType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "it", "en"
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class FrequencyTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public static class FrequencyTypeMapper
    {
        public static FrequencyTypeDto ToDto(this FrequencyType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new FrequencyTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<FrequencyTypeDto> ToDtoList(this IEnumerable<FrequencyType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
