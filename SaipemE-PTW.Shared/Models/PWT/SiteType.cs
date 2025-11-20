using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class SiteType
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public ICollection<SiteTypeLocalization> Localizations { get; set; } = new List<SiteTypeLocalization>();
    }


    public class SiteTypeLocalization
    {
        public int Id { get; set; }

        public int SiteTypeId { get; set; }
        public SiteType SiteType { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "it", "en"
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
    }


    public class SiteTypeDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }


    public static class SiteTypeMapper
    {
        public static SiteTypeDto ToDto(this SiteType entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                            ?? entity.Localizations.FirstOrDefault();

            return new SiteTypeDto
            {
                Id = entity.Id,
                Code = entity.Code,
                Name = localization?.Name,
                Description = localization?.Description
            };
        }

        public static IEnumerable<SiteTypeDto> ToDtoList(this IEnumerable<SiteType> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }



}
