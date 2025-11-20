//TipoAutorita.cs;

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class AuthorityType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public ICollection<AuthorityTypeLocalization> Localizations { get; set; } = new List<AuthorityTypeLocalization>();
    }

    public class AuthorityTypeLocalization
    {
        public int Id { get; set; }

        public int AuthorityTypeId { get; set; }
        public AuthorityType AuthorityType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }


    public class AuthorityTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public static class AuthorityTypeMapper
    {
        public static AuthorityTypeDto ToDto(this AuthorityType entity, string langCode)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == langCode)
                            ?? entity.Localizations.FirstOrDefault();

            return new AuthorityTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<AuthorityTypeDto> ToDtoList(this IEnumerable<AuthorityType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
