using SaipemE_PTW.Shared.Models.Auth;


namespace SaipemE_PTW.Shared.Models.PWT
{
    

    public class CronologiaPermessoLavoro
    {
        public int Id { get; set; }
        public string? Azione { get; set; }
        public DateTime? Data { get; set; }
        public TimeSpan? Ora { get; set; }
        public string? Nome { get; set; }
        public string? Cognome { get; set; }
        public string? Matricola { get; set; }
        public string? Email { get; set; }
        public UserRoleDto? Ruolo { get; set; }

    }
}
