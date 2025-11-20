using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SaipemE_PTW.Services.Auth
{
    // Data: 2025-10-20 - Costanti e parametri di validazione JWT per mock OIDC
    public static class AuthConstants
    {
        // Data: 2025-10-20 - Issuer/Audience fittizi coerenti con Azure AD
        public const string Issuer = "https://login.microsoftonline.com/{tenant-id}/v2.0";
        public const string Audience = "api://client-id";

        // Data: 2025-10-20 - Chiave simmetrica mock (sviluppo)
        private static readonly byte[] _key = Encoding.UTF8.GetBytes("dev-only-super-secret-signing-key-32bytes!");
        public static SymmetricSecurityKey SigningKey { get; } = new SymmetricSecurityKey(_key);

        // Data: 2025-10-20 - Parametri di validazione token condivisi
        public static TokenValidationParameters GetValidationParameters() => new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SigningKey,
            ClockSkew = System.TimeSpan.FromSeconds(5)
        };
    }
}
