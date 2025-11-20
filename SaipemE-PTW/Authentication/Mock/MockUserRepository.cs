namespace SaipemE_PTW.Authentication.Mock;

// 2025-01-15 - Mock Auth - Repository di utenti mock per simulare l'autenticazione Entra ID
// In produzione, questi dati verranno sostituiti dai dati reali provenienti da Microsoft Entra ID
public static class MockUserRepository
{
    // 2025-01-15 - Mock Auth - Tenant ID fittizio per simulare l'organizzazione Saipem
    private const string TenantId = "12345678-1234-1234-1234-123456789abc";

    // 2025-01-15 - Mock Auth - Lista di utenti mock predefiniti per test
    public static List<MockUser> Users { get; } = new()
    {
         new MockUser
        {
        Sub = "user-004",
        Name = "Anna Ferrari",
        PreferredUsername = "anna.ferrari@saipem.com",
        Email = "anna.ferrari@saipem.com",
        Oid = "oid-004",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.AutoritaRichiedente },
        Groups = new List<string> { "group-requesters" }
        },

         new MockUser
        {
        Sub = "user-005",
        Name = "Marco Colombo",
        PreferredUsername = "marco.colombo@saipem.com",
        Email = "marco.colombo@saipem.com",
        Oid = "oid-005",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.AutoritaEsecutrice },
        Groups = new List<string> { "group-executors" }
        },
         new MockUser
        {
        Sub = "user-003",
        Name = "Giuseppe Verdi",
        PreferredUsername = "giuseppe.verdi@saipem.com",
        Email = "giuseppe.verdi@saipem.com",
        Oid = "oid-003",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.AutoritaEmittente },
        Groups = new List<string> { "group-safety", "group-operations" }
        },

         new MockUser
        {
        Sub = "user-002",
        Name = "Laura Bianchi",
        PreferredUsername = "laura.bianchi@saipem.com",
        Email = "laura.bianchi@saipem.com",
        Oid = "oid-002",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.PTWCoordinator },
        Groups = new List<string> { "group-coordinators", "group-safety" }
        },

        new MockUser
        {
        Sub = "user-006",
        Name = "Francesca Romano",
        PreferredUsername = "francesca.romano@saipem.com",
        Email = "francesca.romano@saipem.com",
        Oid = "oid-006",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.CoordinatoreInEsecuzione },
        Groups = new List<string> { "group-coordinators", "group-field" }
        },

        new MockUser
        {
        Sub = "user-007",
        Name = "Roberto Esposito",
        PreferredUsername = "roberto.esposito@saipem.com",
        Email = "roberto.esposito@saipem.com",
        Oid = "oid-007",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.PersonaAutorizzataTestGas },
        Groups = new List<string> { "group-safety", "group-specialists" }
        },

        new MockUser
        {
        Sub = "user-008",
        Name = "Giulia Ricci",
        PreferredUsername = "giulia.ricci@saipem.com",
        Email = "giulia.ricci@saipem.com",
        Oid = "oid-008",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.AutoritaOperativa },
        Groups = new List<string> { "group-operations" }
        },

        new MockUser
        {
        Sub = "user-009",
        Name = "Alessandro Russo",
        PreferredUsername = "alessandro.russo@saipem.com",
        Email = "alessandro.russo@saipem.com",
        Oid = "oid-009",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.EspertoQualificato },
        Groups = new List<string> { "group-specialists", "group-technical" }
        },

        new MockUser
        {
        Sub = "user-010",
        Name = "Elena Marino",
        PreferredUsername = "elena.marino@saipem.com",
        Email = "elena.marino@saipem.com",
        Oid = "oid-010",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.AmministratoreSistema },
        Groups = new List<string> { "group-admin", "group-it" }
        },

        new MockUser
        {
        Sub = "user-001",
        Name = "Mario Rossi",
        PreferredUsername = "mario.rossi@saipem.com",
        Email = "mario.rossi@saipem.com",
        Oid = "oid-001",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.SuperOwner },
        Groups = new List<string> { "group-admin", "group-management" }
        },


        new MockUser
        {
        Sub = "user-011",
        Name = "Luca Galli",
        PreferredUsername = "luca.galli@external.com",
        Email = "luca.galli@external.com",
        Oid = "oid-011",
        Tid = TenantId,
        Roles = new List<string> { AppRoles.Visitatore },
        Groups = new List<string> { "group-visitors" }
        },


        new MockUser
        {
        Sub = "user-012",
        Name = "Chiara Conti (Multiruolo per test)",
        PreferredUsername = "chiara.conti@saipem.com",
        Email = "chiara.conti@saipem.com",
        Oid = "oid-012",
        Tid = TenantId,
        Roles = new List<string>
        {
        AppRoles.CoordinatoreInEsecuzione, AppRoles.AutoritaEmittente,AppRoles.AutoritaOperativa
        },
        Groups = new List<string> { "group-requesters", "group-executors", "group-coordinators" }
        }
    };

    // 2025-01-15 - Mock Auth - Metodo helper per trovare un utente per email
    public static MockUser? GetUserByEmail(string email)
    {
        return Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    // 2025-01-15 - Mock Auth - Metodo helper per trovare un utente per ID
    public static MockUser? GetUserById(string userId)
    {
        return Users.FirstOrDefault(u => u.Sub == userId);
    }
}
