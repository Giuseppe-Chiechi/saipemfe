using System.Security.Claims;

namespace SaipemE_PTW.Authentication.Mock;

// 2025-01-15 - Mock Auth - Modello per rappresentare un utente mock del sistema e-PTW
// Contiene tutti i dati necessari per simulare un utente autenticato con Entra ID
public class MockUser
{
    // 2025-01-15 - Mock Auth - Identificatore univoco dell'utente (simula il claim 'sub')
    public string Sub { get; set; } = string.Empty;

    // 2025-01-15 - Mock Auth - Nome completo dell'utente
    public string Name { get; set; } = string.Empty;

    // 2025-01-15 - Mock Auth - Nome utente preferito (email)
    public string PreferredUsername { get; set; } = string.Empty;

    // 2025-01-15 - Mock Auth - Indirizzo email dell'utente
    public string Email { get; set; } = string.Empty;

  // 2025-01-15 - Mock Auth - Object ID dell'utente in Entra ID
    public string Oid { get; set; } = string.Empty;

    // 2025-01-15 - Mock Auth - Tenant ID dell'organizzazione
    public string Tid { get; set; } = string.Empty;

    // 2025-01-15 - Mock Auth - Ruoli assegnati all'utente
    public List<string> Roles { get; set; } = new();

    // 2025-01-15 - Mock Auth - ID dei gruppi di cui l'utente fa parte
    public List<string> Groups { get; set; } = new();

    // 2025-01-15 - Mock Auth - Metodo per convertire il MockUser in ClaimsPrincipal
    // Questo permette di usare il mock con il sistema di autenticazione standard di Blazor
    public ClaimsPrincipal ToClaimsPrincipal()
    {
        var claims = new List<Claim>
        {
   new(ClaimTypes.NameIdentifier, Sub),
  new(ClaimTypes.Name, Name),
   new("preferred_username", PreferredUsername),
   new(ClaimTypes.Email, Email),
            new("oid", Oid),
       new("tid", Tid)
        };

        // 2025-01-15 - Mock Auth - Aggiunge tutti i ruoli come claims separati
        foreach (var role in Roles)
        {
     claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // 2025-01-15 - Mock Auth - Aggiunge tutti i gruppi come claims separati
  foreach (var group in Groups)
        {
   claims.Add(new Claim("groups", group));
        }

 var identity = new ClaimsIdentity(claims, "MockAuthentication");
   return new ClaimsPrincipal(identity);
    }
}
