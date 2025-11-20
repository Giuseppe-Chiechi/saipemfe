

namespace SaipemE_PTW.Shared.Models.Menu
{
    public class MenuItem
    {
        public int? id { get; set; }
        public string? Text { get; set; }
        public string? Link { get; set; }
        public string? Counter { get; set; } // Optional
        public string? Icon { get; set; }
        public string? Descrizione { get; set; }
    }

    public class MenuSection
    {
        public int? id { get; set; }
        public string? Header { get; set; }
        public List<MenuItem>? Items { get; set; } = new();
    }

    public class MenuCard
    {
        public int? id { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public List<MenuSection>? Sections { get; set; } = new();
    }




}
