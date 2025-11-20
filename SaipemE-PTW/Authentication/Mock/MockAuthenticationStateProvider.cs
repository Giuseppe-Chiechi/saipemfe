using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SaipemE_PTW.Authentication.Mock;

// 2025-01-15 - Mock Auth - Provider personalizzato per simulare l'autenticazione con Entra ID
// Questo provider sostituisce temporaneamente il provider MSAL reale
// NOTA: In produzione, rimuovere questo provider e riattivare AddMsalAuthentication in Program.cs
public class MockAuthenticationStateProvider : AuthenticationStateProvider
{
    // 2025-01-15 - Mock Auth - Utente correntemente autenticato (null = non autenticato)
    private MockUser? _currentUser;

    // 2025-01-15 - Mock Auth - Evento che notifica i cambiamenti nello stato di autenticazione
    public event Action? OnAuthenticationStateChanged;

    // 2025-01-15 - Mock Auth - Override del metodo principale che restituisce lo stato di autenticazione
    // Questo metodo viene chiamato automaticamente da Blazor quando necessario
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsPrincipal user;

        if (_currentUser != null)
        {
    // 2025-01-15 - Mock Auth - Utente autenticato: converte MockUser in ClaimsPrincipal
            user = _currentUser.ToClaimsPrincipal();
        }
     else
        {
   // 2025-01-15 - Mock Auth - Nessun utente autenticato: crea un principal anonimo
      user = new ClaimsPrincipal(new ClaimsIdentity());
        }

      return Task.FromResult(new AuthenticationState(user));
    }

    // 2025-01-15 - Mock Auth - Simula il login di un utente
    // In produzione, questo sarà gestito da MSAL e Entra ID
    public async Task LoginAsync(string email)
    {
   // 2025-01-15 - Mock Auth - Cerca l'utente nel repository mock
        var user = MockUserRepository.GetUserByEmail(email);

        if (user == null)
        {
throw new InvalidOperationException($"Utente con email '{email}' non trovato nel repository mock.");
        }

      _currentUser = user;

        // 2025-01-15 - Mock Auth - Salva l'email in localStorage per persistenza tra sessioni
        // In produzione, MSAL gestirà automaticamente la persistenza dei token
        await Task.CompletedTask; // Placeholder per operazioni async future

        // 2025-01-15 - Mock Auth - Notifica il cambio di stato di autenticazione
        NotifyAuthenticationStateChanged();
    }

    // 2025-01-15 - Mock Auth - Simula il logout dell'utente
  // In produzione, questo sarà gestito da MSAL
    public Task LogoutAsync()
    {
      _currentUser = null;

// 2025-01-15 - Mock Auth - Rimuove i dati di sessione da localStorage
  // In produzione, MSAL pulirà automaticamente i token

        // 2025-01-15 - Mock Auth - Notifica il cambio di stato di autenticazione
      NotifyAuthenticationStateChanged();

        return Task.CompletedTask;
    }

    // 2025-01-15 - Mock Auth - Ottiene l'utente correntemente autenticato
    public MockUser? GetCurrentUser()
    {
    return _currentUser;
    }

    // 2025-01-15 - Mock Auth - Verifica se l'utente corrente ha un ruolo specifico
    public bool IsInRole(string role)
    {
        return _currentUser?.Roles.Contains(role) ?? false;
    }

    // 2025-01-15 - Mock Auth - Ottiene tutti i ruoli dell'utente corrente
    public List<string> GetUserRoles()
    {
        return _currentUser?.Roles ?? new List<string>();
  }

    // 2025-01-15 - Mock Auth - Metodo privato per notificare i cambiamenti di stato
    private void NotifyAuthenticationStateChanged()
    {
        var authState = GetAuthenticationStateAsync();
   NotifyAuthenticationStateChanged(authState);
    OnAuthenticationStateChanged?.Invoke();
    }
}
