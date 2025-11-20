//////PermitType.cs
using System.ComponentModel.DataAnnotations;


namespace SaipemE_PTW.Shared.Models.PWT
{

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class UrlAttribute : Attribute
    {
        public string Url { get; }

        public UrlAttribute(string url)
        {
            Url = url;
        }
    }


    public class TipoPDLDto : Attribute
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
        
        public string? Url { get; set; }


        

    }

    public enum TipoPDL
    {
        [Display(Name = "Permesso di Lavoro Generico (Caldo)", ShortName = "", Description = "")]
        [Url("/workflow/general-hot/")]
        PermessoLavoroGenericoCaldo = 1,

        [Display(Name = "Permesso di Lavoro a Freddo", ShortName = "", Description = "")]
        [Url("/workflow/cold/")]
        PermessoLavoroFreddo = 2,

        [Display(Name = "Permesso di Lavoro per Attività Radiografica", ShortName = "", Description = "")]
        [Url("/workflow/radiographic/")]
        PermessoLavoroAttivitaRadiografica = 3,

        [Display(Name = "Permesso di Lavoro a Caldo", ShortName = "", Description = "")]
        [Url("/workflow/hot/")]
        PermessoLavoroCaldo = 4
    }

    public static class TipoPDLMapper
    {
        public static List<TipoPDLDto> GetAll()
        {
            var list = new List<TipoPDLDto>();

            foreach (TipoPDL tipo in Enum.GetValues(typeof(TipoPDL)))
            {
                var member = typeof(TipoPDL).GetMember(tipo.ToString()).First();

                var display = member.GetCustomAttributes(typeof(DisplayAttribute), false)
                                    .Cast<DisplayAttribute>()
                                    .FirstOrDefault();

                var urlAttr = member.GetCustomAttributes(typeof(UrlAttribute), false)
                                    .Cast<UrlAttribute>()
                                    .FirstOrDefault();

                list.Add(new TipoPDLDto
                {
                    Id = (int)tipo,
                    Nome = display?.Name,
                    Descrizione = display?.Description,
                    Url = urlAttr?.Url
                });
            }

            return list;
        }
    }

    //public static class TipoPDLMapper
    //{
    //    public static TipoPDLDto ToDto(this TipoPDL tipo)
    //    {
    //        return new TipoPDLDto
    //        {
    //            Id = (int)tipo,
    //            Codice = tipo.GetShortName(),
    //            Nome = tipo.GetDisplayName(),
    //            Descrizione = tipo.GetDescription()
    //        };
    //    }

    //    public static IEnumerable<TipoPDLDto> GetAll()
    //    {
    //        return Enum.GetValues(typeof(TipoPDL))
    //            .Cast<TipoPDL>()
    //            .Select(e => e.ToDto());
    //    }
    //}
}
