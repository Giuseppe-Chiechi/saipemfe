//TipoUnitaAutorizzanti.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class AuthorizingUnitType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public ICollection<AuthorizingUnitTypeLocalization> Localizations { get; set; } = new List<AuthorizingUnitTypeLocalization>();
    }

    public class AuthorizingUnitTypeLocalization
    {
        public int Id { get; set; }

        public int AuthorizingUnitTypeId { get; set; }
        public AuthorizingUnitType AuthorizingUnitType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

    public class AuthorizingUnitTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public static class AuthorizingUnitTypeMapper
    {
        public static AuthorizingUnitTypeDto ToDto(this AuthorizingUnitType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new AuthorizingUnitTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<AuthorizingUnitTypeDto> ToDtoList(this IEnumerable<AuthorizingUnitType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
