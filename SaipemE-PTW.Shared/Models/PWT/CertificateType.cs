//TipoCertificato.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class CertificateType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public ICollection<CertificateTypeLocalization> Localizations { get; set; } = new List<CertificateTypeLocalization>();
    }


    public class CertificateTypeLocalization
    {
        public int Id { get; set; }

        public int CertificateTypeId { get; set; }
        public CertificateType CertificateType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }


    public class CertificateTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }


    public static class CertificateTypeMapper
    {
        public static CertificateTypeDto ToDto(this CertificateType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new CertificateTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<CertificateTypeDto> ToDtoList(this IEnumerable<CertificateType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
