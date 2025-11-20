using SaipemE_PTW.Shared.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaipemE_PTW.Shared.Models.PWT
{
    

    public class AnagraficaUtentiEsterni
    {
        public int Id { get; set; }
        public string? Matricola { get; set; }
        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public string? Email { get; set; }
        public List<UserRoleDto>? Ruoli { get; set; }
    }
}
