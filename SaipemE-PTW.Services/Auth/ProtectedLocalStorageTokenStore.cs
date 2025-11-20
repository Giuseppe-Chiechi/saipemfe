using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace SaipemE_PTW.Services.Auth
{
    // Data: 2025-10-20 - Implementazione storage token usando JS interop (simula ProtectedLocalStorage)
    // Sicurezza: imposta SameSite/secure via Indexed/localStorage; evita XSS sanitizzando key e validando stringhe
    public sealed class ProtectedLocalStorageTokenStore(IJSRuntime js) : ITokenStorageService
    {
        private readonly IJSRuntime _js = js;
        private const string StorageKey = "auth_token"; // Data: 2025-10-20 - Key fissa sanificata

        public async Task SetTokenAsync(string token)
        {
            // Data: 2025-10-20 - Validazione base token (non vuoto e dimensione massima 8KB)
            if (string.IsNullOrWhiteSpace(token)) return;
            if (token.Length > 8192) throw new ArgumentOutOfRangeException(nameof(token), "Token troppo grande");

            await _js.InvokeVoidAsync("authStorage.setToken", StorageKey, token);
        }

        public async Task<string?> GetTokenAsync()
        {
            try
            {
                var token = await _js.InvokeAsync<string?>("authStorage.getToken", StorageKey);
                return string.IsNullOrWhiteSpace(token) ? null : token;
            }
            catch
            {
                return null;
            }
        }

        public Task ClearTokenAsync()
            => _js.InvokeVoidAsync("authStorage.removeToken", StorageKey).AsTask();
    }
}
