using SaipemE_PTW.Shared.Models.Auth;

namespace SaipemE_PTW.Shared.Models.PWT
{
   
    public class AnagraficaUtentiInterniDto
    {
        public int Id { get; set; }
        public string? Matricola { get; set; }
        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public string? Email { get; set; }
        public List<UserRoleDto>? Ruoli { get; set; }
    }

}
