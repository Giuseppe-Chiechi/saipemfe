//TipoAllegati.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class AttachmentType
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; } = default!;

        public ICollection<AttachmentTypeLocalization> Localizations { get; set; } = new List<AttachmentTypeLocalization>();

        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }

    }

    public class AttachmentTypeLocalization
    {
        public int Id { get; set; }

        [Required]
        public int AttachmentTypeId { get; set; }

        public AttachmentType AttachmentType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"

        [Required]
        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;
    }

    public class AttachmentTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public static class AttachmentTypeMapper
    {
        public static AttachmentTypeDto ToDto(this AttachmentType type, string lang)
        {
            var localization = type.Localizations.FirstOrDefault(l => l.Language == lang);

            return new AttachmentTypeDto
            {
                Id = type.Id,
                Code = type.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<AttachmentTypeDto> ToDtoList(this IEnumerable<AttachmentType> types, string langCode)
        {
            return types.Select(t => t.ToDto(langCode));
        }
    }
}
