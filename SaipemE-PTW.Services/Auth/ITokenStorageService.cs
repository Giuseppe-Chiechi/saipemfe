using System.Threading.Tasks;

namespace SaipemE_PTW.Services.Auth
{
    // Data: 2025-10-20 - Interfaccia per storage protetto del token JWT lato WASM
    // Nota sicurezza: in un vero scenario OIDC usare i provider integrati; questo è solo mock.
    public interface ITokenStorageService
    {
        Task SetTokenAsync(string token);
        Task<string?> GetTokenAsync();
        Task ClearTokenAsync();
    }
}
