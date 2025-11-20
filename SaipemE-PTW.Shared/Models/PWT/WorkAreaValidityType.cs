//TipoValiditaAreaLavoro.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class WorkAreaValidityType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public ICollection<WorkAreaValidityTypeLocalization> Localizations { get; set; } = new List<WorkAreaValidityTypeLocalization>();
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }


    public class WorkAreaValidityTypeLocalization
    {
        public int Id { get; set; }

        public int WorkAreaValidityTypeId { get; set; }
        public WorkAreaValidityType WorkAreaValidityType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }


    public class WorkAreaValidityTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }


    public static class WorkAreaValidityTypeMapper
    {
        public static WorkAreaValidityTypeDto ToDto(this WorkAreaValidityType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new WorkAreaValidityTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<WorkAreaValidityTypeDto> ToDtoList(this IEnumerable<WorkAreaValidityType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
