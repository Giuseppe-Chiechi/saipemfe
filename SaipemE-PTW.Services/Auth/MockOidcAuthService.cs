using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using SaipemE_PTW.Shared.Models.Auth; // Data: 2025-10-20 - Per gestione liste ruoli

namespace SaipemE_PTW.Services.Auth
{
    // Data: 2025-10-20 - Servizio mock che genera un JWT fittizio con claim OIDC
    // Solo per sviluppo UI in Blazor WASM. Non usare in produzione.
    public sealed class MockOidcAuthService(ITokenStorageService storage, ILogger<MockOidcAuthService> logger) : IAuthService
    {
        private readonly ITokenStorageService _storage = storage;
        private readonly ILogger<MockOidcAuthService> _logger = logger;

        public async Task<bool> SignInAsync(/*string email, string password, */UserRole userRole)
        {
            try
            {
                // Data: 2025-10-20 - Validazione semplice mock
                //if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                //{
                //    _logger.LogWarning("SignInAsync: credenziali vuote");
                //    return false;
                //}

                // Data: 2025-10-20 - Deduzione ruoli mock dall'email (supporta più ruoli)
                var roles = ResolveRolesFromEmail(userRole);
                //IReadOnlyList<UserRole> roles = new List<UserRole> { userRole };
                //var primaryRole = roles;

                var user = new AuthUserDto
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Email = userRole+"@saipem.com",
                    Name = "Mario",
                    Cognome = "Rossi",
                    // Data: 2025-10-20 - Codice ruolo primario per compatibilità UI (es. RA, PA, CSE)
                    Ruolo = userRole.ToString()//UserRoleHelper.ToCode(userRole)
                };

                var token = GenerateJwt(user, roles);
                await _storage.SetTokenAsync(token);

                _logger.LogInformation("SignInAsync: utente mock autenticato con ruoli {Roles}", string.Join(",", roles.Select(UserRoleHelper.ToCode)));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante SignInAsync");
                return false;
            }
        }

        public async Task SignOutAsync()
        {
            await _storage.ClearTokenAsync();

            _logger.LogInformation("SignOut eseguito, token rimosso");
        }

        public async Task<AuthUserDto?> GetCurrentUserAsync()
        {
            try
            {
                var principal = await GetClaimsPrincipalAsync();
                if (principal.Identity?.IsAuthenticated != true) return null;

                // Data: 2025-10-20 - Preleva il primo ruolo per compatibilità UI
                var firstRole = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).FirstOrDefault() ?? string.Empty;

                return new AuthUserDto
                {
                    Id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty,
                    Email = principal.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty,
                    Name = principal.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty,
                    Cognome = principal.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty,
                    Ruolo = firstRole
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore in GetCurrentUserAsync");
                return null;
            }
        }

        public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync()
        {
            try
            {
                var token = await _storage.GetTokenAsync();
                if (string.IsNullOrWhiteSpace(token)) return new ClaimsPrincipal(new ClaimsIdentity());

                var handler = new JwtSecurityTokenHandler();
                var parameters = AuthConstants.GetValidationParameters();

                var principal = handler.ValidateToken(token, parameters, out _);
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token non valido o scaduto");
                return new ClaimsPrincipal(new ClaimsIdentity());
            }
        }

        // Data: 2025-10-20 - Genera JWT con claims OIDC/Azure tipici e ruoli multipli
        private static string GenerateJwt(AuthUserDto user, IReadOnlyList<UserRole> roles)
        {
            var now = DateTimeOffset.UtcNow;

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id), // sub
                new(ClaimTypes.Email, user.Email),       // email
                new(ClaimTypes.GivenName, user.Name),    // given_name
                new(ClaimTypes.Surname, user.Cognome),   // family_name
                new(ClaimTypes.Name, $"{user.Name} {user.Cognome}"),
                new("tid", "mock-tenant"),
                new("nonce", Guid.NewGuid().ToString("N"))
            };

            // Data: 2025-10-20 - Aggiungi tutti i ruoli come claim con codici normalizzati
            foreach (var r in roles.Distinct())
            {
                claims.Add(new Claim(ClaimTypes.Role, UserRoleHelper.ToCode(r)));
            }

            var signingCredentials = new SigningCredentials(AuthConstants.SigningKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: AuthConstants.Issuer,
                audience: AuthConstants.Audience,
                claims: claims,
                notBefore: now.UtcDateTime,
                expires: now.AddHours(8).UtcDateTime,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        // Data: 2025-10-20 - Strategia mock: deduce ruoli dall'email (solo per sviluppo UI)
        private static IReadOnlyList<UserRole> ResolveRolesFromEmail(UserRole userRole)
        {
            //var e = (email ?? string.Empty).Trim().ToLowerInvariant();
            var roles = new List<UserRole>();

            roles.Add(userRole);

            //// Data: 2025-10-20 - Utente 'admin' ha tutti i ruoli (mock)
            //if (e.Contains("admin"))
            //{
            //    roles.AddRange(Enum.GetValues<UserRole>());
            //    return roles;
            //}

            //if (e.Contains(".ra") || e.Contains("@ra") || e.Contains("richiedent"))
            //    roles.Add(UserRole.AutoritaRichiedente);

            //if (e.Contains(".pa") || e.Contains("@pa") || e.Contains("esecutric"))
            //    roles.Add(UserRole.AutoritaEsecutrice);

            //if (e.Contains("ptw"))
            //    roles.Add(UserRole.PTWCoordinator);

            //if (e.Contains("emitt") || e.Contains("issuer"))
            //    roles.Add(UserRole.AutoritaEmittente);

            //if (e.Contains("cse"))
            //    roles.Add(UserRole.CoordinatoreInEsecuzione);

            //if (e.Contains("agt") || e.Contains("gas"))
            //    roles.Add(UserRole.PersonaAutorizzataTestGas);

            //if (e.Contains("op") || e.Contains("operativ"))
            //    roles.Add(UserRole.AutoritaOperativa);

            // Data: 2025-10-20 - Default sicuro: ruolo operativo se nessuno dedotto
            if (roles.Count == 0)
                roles.Add(UserRole.Anonymous);

            return roles;
        }
    }
}
