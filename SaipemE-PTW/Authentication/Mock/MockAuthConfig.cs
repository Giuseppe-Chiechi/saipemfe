namespace SaipemE_PTW.Authentication.Mock;

// 2025-01-15 - Mock Auth - Configurazione centralizzata per il sistema di autenticazione mock
// Modificare questi valori per passare facilmente da mock a produzione
public static class MockAuthConfig
{
    // 2025-01-15 - Mock Auth - Flag per abilitare/disabilitare l'autenticazione mock
    // TRUE = Usa mock authentication (sviluppo/test)
    // FALSE = Usa Microsoft Entra ID (produzione)
    // IMPORTANTE: Impostare a FALSE prima del deployment in produzione
    public const bool UseMockAuthentication = true;

    // 2025-01-15 - Mock Auth - Flag per mostrare il badge "MOCK" nell'interfaccia
    public const bool ShowMockBadge = true;

    // 2025-01-15 - Mock Auth - Flag per abilitare il login automatico (utile per sviluppo rapido)
    // Se true, l'utente specificato in DefaultMockUser verrà autenticato automaticamente
    public const bool EnableAutoLogin = false;

    // 2025-01-15 - Mock Auth - Email dell'utente per il login automatico
    // Usato solo se EnableAutoLogin è true
    public const string DefaultMockUserEmail = "mario.rossi@saipem.com";

    // 2025-01-15 - Mock Auth - Metodo helper per verificare se siamo in modalità mock
    public static bool IsMockMode => UseMockAuthentication;

    // 2025-01-15 - Mock Auth - Metodo helper per verificare se siamo in produzione
    public static bool IsProductionMode => !UseMockAuthentication;
}
