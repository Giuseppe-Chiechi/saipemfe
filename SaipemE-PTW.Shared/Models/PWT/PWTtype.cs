////PermitType.cs
//using System.ComponentModel.DataAnnotations;


//namespace SaipemE_PTW.Shared.Models.PWT
//{


//    public class PWTtypeDto
//    {
//        public int Id { get; set; }
//        public string? Codice { get; set; }
//        public string? Nome { get; set; }
//        public string? Descrizione { get; set; }
//    }

//    public enum PWTtype
//    {
//        [Display(Name = "Permesso di Lavoro Generico (Caldo)", ShortName = "PLG", Description = "Lavori con fonti di calore")]
//        PermessoLavoroGenericoCaldo = 1,

//        [Display(Name = "Permesso di Lavoro Freddo", ShortName = "PLF", Description = "Lavori senza generazione calore")]
//        PermessoLavoroFreddo = 2,

//        [Display(Name = "Permesso di Lavoro Radiografie", ShortName = "PLR", Description = "Uso sorgenti radioattive")]
//        PermessoLavoroRadiografie = 3,
//        [Display(Name = "Permesso di Lavoro Caldo", ShortName = "HOT", Description = "Uso sorgenti radioattive")]
//        PermessoLavoroCaldo = 4
//    }

//    public static class PWTtypeMapper
//    {
//        public static PWTtypeDto ToDto(this PWTtype tipo)
//        {
//            return new PWTtypeDto
//            {
//                Id = (int)tipo,
//                Codice = tipo.GetShortName(),
//                Nome = tipo.GetDisplayName(),
//                Descrizione = tipo.GetDescription()
//            };
//        }

//        public static IEnumerable<PWTtypeDto> GetAll()
//        {
//            return Enum.GetValues(typeof(PWTtype))
//                .Cast<PWTtype>()
//                .Select(e => e.ToDto());
//        }
//    }
//}
