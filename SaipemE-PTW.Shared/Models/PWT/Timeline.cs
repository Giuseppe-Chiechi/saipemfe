//TimelineItem.cs

using System.ComponentModel.DataAnnotations;

namespace SaipemE_PTW.Shared.Models.PWT
{
    public class Timeline
    {
        public int Id { get; set; }

        public DateTime? Date { get; set; }

        public bool IsActive { get; set; }

        public ICollection<TimelineLocalization> Localizations { get; set; } = new List<TimelineLocalization>();
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }

    public class TimelineLocalization
    {
        public int Id { get; set; }

        public int TimelineId { get; set; }
        public Timeline Timeline { get; set; } = default!;

        [MaxLength(5)]
        public string Language { get; set; } = default!; // "en", "it"
        public string Title { get; set; } = default!;
        public string? Note { get; set; }
    }

    public class TimelineDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }
        public string? Note { get; set; }
        public DateTime? Date { get; set; }
        public bool IsActive { get; set; }
    }

    public static class TimelineMapper
    {
        public static TimelineDto ToDto(this Timeline entity, string lang)
        {
            var localization = entity.Localizations.FirstOrDefault(l => l.Language == lang)
                               ?? entity.Localizations.FirstOrDefault();

            return new TimelineDto
            {
                Id = entity.Id,
                Title = localization?.Title,
                Note = localization?.Note,
                Date = entity.Date,
                IsActive = entity.IsActive
            };
        }

        public static IEnumerable<TimelineDto> ToDtoList(this IEnumerable<Timeline> entities, string langCode)
        {
            return entities.Select(e => e.ToDto(langCode));
        }
    }



}
