using SaipemE_PTW.Shared.Models.Auth;
using SaipemE_PTW.Shared.Models.PWT;


namespace SaipemE_PTW.Services.Administrator
{
    public class UtentiInterniService : IUtentiInterniService
    {
        private readonly List<AnagraficaUtentiInterniDto> _mockData = new()
    {
        new() { Id = 1, Matricola="0000ex1", Nome = "Mario", Cognome = "Rossi", Email = "mario.rossi@example.com", 
            Ruoli = new List<Shared.Models.Auth.UserRoleDto>{
             new UserRoleDto { Nome = "Autorità Richiedente RA" },
              new UserRoleDto { Nome = "Super Owner" }
            } 
        },
        new() { Id = 2, Matricola="0000ex2", Nome = "Laura", Cognome = "Bianchi", Email = "laura.bianchi@example.com",
        Ruoli = new List<Shared.Models.Auth.UserRoleDto>{
             new UserRoleDto { Nome = "Autorita Esecutrice PA" }
            } },
        new() { Id = 3, Matricola="0000ex3", Nome = "Giulia", Cognome = "Verdi", Email = "giulia.verdi@example.com",
        Ruoli = new List<Shared.Models.Auth.UserRoleDto>{
             new UserRoleDto { Nome = "Autorita Emittente" }
            } }
    };

        public Task<List<AnagraficaUtentiInterniDto>> GetUtentiAsync(string? search = null)
        {
            var result = string.IsNullOrWhiteSpace(search)
                ? _mockData
                : _mockData.Where(x =>
                    (x.Nome?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.Cognome?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.Email?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false))
                    .ToList();

            return Task.FromResult(result);
        }
    }
}
