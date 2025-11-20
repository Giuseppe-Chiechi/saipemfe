namespace SaipemE_PTW.Authentication.Mock;

// 2025-01-15 - Mock Auth - Costanti per i ruoli dell'applicazione e-PTW
// Questi ruoli corrispondono a quelli configurati in Microsoft Entra ID
public static class AppRoles
{
    // 2025-01-15 - Mock Auth - Autorità Richiedente - Può richiedere permessi di lavoro
    public const string AutoritaRichiedente = "RA";

// 2025-01-15 - Mock Auth - Autorità Esecutrice - Esegue i lavori autorizzati
    public const string AutoritaEsecutrice = "PA";

    // 2025-01-15 - Mock Auth - Autorità Emittente - Emette i permessi di lavoro
    public const string AutoritaEmittente = "IA";

    // 2025-01-15 - Mock Auth - PTW Coordinator - Coordina il sistema dei permessi
    public const string PTWCoordinator = "PTWC";

    // 2025-01-15 - Mock Auth - Coordinatore In Esecuzione - Supervisiona l'esecuzione
    public const string CoordinatoreInEsecuzione = "CSE";

    // 2025-01-15 - Mock Auth - Persona Autorizzata Test Gas - Esegue test su atmosfere pericolose
    public const string PersonaAutorizzataTestGas = "AGT";

    // 2025-01-15 - Mock Auth - Autorità Operativa - Gestisce le operazioni
    public const string AutoritaOperativa = "OA";

    // 2025-01-15 - Mock Auth - Esperto Qualificato - Fornisce consulenza tecnica specializzata
    public const string EspertoQualificato = "EQ";

    // 2025-01-15 - Mock Auth - Amministratore Sistema - Gestisce configurazioni e utenti
    public const string AmministratoreSistema = "ADMIN";

    // 2025-01-15 - Mock Auth - Super Owner - Accesso completo al sistema
    public const string SuperOwner = "SUPER_OWNER";

    // 2025-01-15 - Mock Auth - Visitatore - Accesso in sola lettura
    public const string Visitatore = "VISITOR";

    // 2025-01-15 - Mock Auth - Anonymous - Utente non autenticato
    public const string Anonymous = "ANONYMOUS";

 // 2025-01-15 - Mock Auth - Array con tutti i ruoli disponibili per facilitare la gestione
    public static readonly string[] AllRoles = 
    {
        AutoritaRichiedente,
        AutoritaEsecutrice,
        AutoritaEmittente,
        PTWCoordinator,
        CoordinatoreInEsecuzione,
        PersonaAutorizzataTestGas,
        AutoritaOperativa,
        EspertoQualificato,
        AmministratoreSistema,
        SuperOwner,
        Visitatore,
        Anonymous
    };

    // 2025-01-15 - Mock Auth - Dizionario con descrizioni leggibili dei ruoli
    public static readonly Dictionary<string, string> RoleDescriptions = new()
    {
        { AutoritaRichiedente, "Autorità Richiedente (RA)" },
        { AutoritaEsecutrice, "Autorità Esecutrice (PA)" },
        { AutoritaEmittente, "Autorità Emittente (IA)" },
        { PTWCoordinator, "PTW Coordinator (PTWC)" },
        { CoordinatoreInEsecuzione, "Coordinatore In Esecuzione (CSE)" },
        { PersonaAutorizzataTestGas, "Persona Autorizzata Test Gas (AGT)" },
        { AutoritaOperativa, "Autorità Operativa (OA)" },
        { EspertoQualificato, "Esperto Qualificato (EQ)" },
        { AmministratoreSistema, "Amministratore Sistema" },
        { SuperOwner, "Super Owner" },
        { Visitatore, "Visitatore" },
        { Anonymous, "Anonymous" }
    };
}
