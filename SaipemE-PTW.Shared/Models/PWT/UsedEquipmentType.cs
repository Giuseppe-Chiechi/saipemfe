//TipoAttrezzaturaUtilizzata.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class UsedEquipmentType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;

        public ICollection<UsedEquipmentTypeLocalization> Localizations { get; set; } = new List<UsedEquipmentTypeLocalization>();
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }

    public class UsedEquipmentTypeLocalization
    {
        public int Id { get; set; }

        public int UsedEquipmentTypeId { get; set; }
        public UsedEquipmentType UsedEquipmentType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }

    public class UsedEquipmentTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public static class UsedEquipmentTypeMapper
    {
        public static UsedEquipmentTypeDto ToDto(this UsedEquipmentType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new UsedEquipmentTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<UsedEquipmentTypeDto> ToDtoList(this IEnumerable<UsedEquipmentType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }

}
