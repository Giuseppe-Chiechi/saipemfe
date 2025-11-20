//TipoParte.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class WorkPermitSectionType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public ICollection<WorkPermitSectionTypeLocalization> Localizations { get; set; } = new List<WorkPermitSectionTypeLocalization>();
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }

    public class WorkPermitSectionTypeLocalization
    {
        public int Id { get; set; }

        public int WorkPermitSectionTypeId { get; set; }
        public WorkPermitSectionType WorkPermitSectionType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }


    public class WorkPermitSectionTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }


    public static class WorkPermitSectionTypeMapper
    {
        public static WorkPermitSectionTypeDto ToDto(this WorkPermitSectionType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new WorkPermitSectionTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<WorkPermitSectionTypeDto> ToDtoList(this IEnumerable<WorkPermitSectionType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
