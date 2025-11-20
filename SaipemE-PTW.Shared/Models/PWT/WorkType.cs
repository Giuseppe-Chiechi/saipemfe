//TipoLavoro.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class WorkType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public ICollection<WorkTypeLocalization> Localizations { get; set; } = new List<WorkTypeLocalization>();
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }


    public class WorkTypeLocalization
    {
        public int Id { get; set; }

        public int WorkTypeId { get; set; }
        public WorkType WorkType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

    public class WorkTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public static class WorkTypeMapper
    {
        public static WorkTypeDto ToDto(this WorkType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new WorkTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<WorkTypeDto> ToDtoList(this IEnumerable<WorkType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
