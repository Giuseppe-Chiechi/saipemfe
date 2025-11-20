//AnagraficaImpresaEsecutrice.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class ContractorCompany
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public ICollection<ContractorCompanyLocalization> Localizations { get; set; } = new List<ContractorCompanyLocalization>();
    }

    public class ContractorCompanyLocalization
    {
        public int Id { get; set; }

        public int ContractorCompanyId { get; set; }
        public ContractorCompany ContractorCompany { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class ContractorCompanyDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public static class ContractorCompanyMapper
    {
        public static ContractorCompanyDto ToDto(this ContractorCompany entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new ContractorCompanyDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<ContractorCompanyDto> ToDtoList(this IEnumerable<ContractorCompany> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
