//TipoStatoPermessoLavoro.cs;

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class WorkPermitStatusType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public ICollection<WorkPermitStatusTypeLocalization> Localizations { get; set; } = new List<WorkPermitStatusTypeLocalization>();
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }


    public class WorkPermitStatusTypeLocalization
    {
        public int Id { get; set; }

        public int WorkPermitStatusTypeId { get; set; }
        public WorkPermitStatusType WorkPermitStatusType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }


    public class WorkPermitStatusTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }


    public static class WorkPermitStatusTypeMapper
    {
        public static WorkPermitStatusTypeDto ToDto(this WorkPermitStatusType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new WorkPermitStatusTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<WorkPermitStatusTypeDto> ToDtoList(this IEnumerable<WorkPermitStatusType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
