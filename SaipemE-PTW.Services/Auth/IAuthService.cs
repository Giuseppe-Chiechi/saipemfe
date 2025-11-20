using System.Security.Claims;
using System.Threading.Tasks;
using SaipemE_PTW.Shared.Models.Auth;

namespace SaipemE_PTW.Services.Auth
{
    // Data: 2025-10-20 - Interfaccia servizio autenticazione mock OIDC/Azure AD
    public interface IAuthService
    {
        Task<bool> SignInAsync(/*string email, string password*/UserRole userRole);
        Task SignOutAsync();
        Task<AuthUserDto?> GetCurrentUserAsync();
        Task<ClaimsPrincipal> GetClaimsPrincipalAsync();
    }
}
