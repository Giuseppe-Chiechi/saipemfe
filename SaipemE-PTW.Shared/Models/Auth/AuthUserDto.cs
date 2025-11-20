namespace SaipemE_PTW.Shared.Models.Auth
{
    // Data: 2025-10-20 - DTO utente autenticato (mock OIDC/Azure AD)
    // Sicurezza: usare solo per mock lato client; non fidarsi mai del token lato server
    public sealed class AuthUserDto
    {
        // Data: 2025-10-20 - ID univoco utente (sub)
        public string Id { get; set; } = string.Empty;

        // Data: 2025-10-20 - Email aziendale (UPN)
        public string Email { get; set; } = string.Empty;

        // Data: 2025-10-20 - Nome (given_name)
        public string Name { get; set; } = string.Empty;

        // Data: 2025-10-20 - Cognome (family_name)
        public string Cognome { get; set; } = string.Empty;

        // Data: 2025-10-20 - Ruolo applicativo (es. supervisore, operatore)
        public string Ruolo { get; set; } = string.Empty;
    }
}
