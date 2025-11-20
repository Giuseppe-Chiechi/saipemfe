//TipoAttivita.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class ActivityType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public ICollection<ActivityTypeLocalization> Localizations { get; set; } = new List<ActivityTypeLocalization>();
    }

    public class ActivityTypeLocalization
    {
        public int Id { get; set; }

        public int ActivityTypeId { get; set; }
        public ActivityType ActivityType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "it", "en"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

    public class ActivityTypeDto
    {
        public int Id { get; set; }

        // Code è un campo tecnico/codice breve (es. "MECH", "CIVIL")
        public string? Code { get; set; }

        // Name e Description sono localizzate in base alla lingua
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public static class ActivityTypeMapper
    {
        public static ActivityTypeDto ToDto(this ActivityType entity, string langCode)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == langCode)
                             ?? entity.Localizations.FirstOrDefault(); // fallback

            return new ActivityTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<ActivityTypeDto> ToDtoList(this IEnumerable<ActivityType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
