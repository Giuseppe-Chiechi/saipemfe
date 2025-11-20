namespace SaipemE_PTW.Services.Http
{
    /// <summary>
    /// Data: 2025-11-05 - Opzioni per typed HttpClient (BaseAddress da appsettings/DI).
    /// </summary>
    public sealed class ApiClientOptions
    {
        public string BaseAddress { get; set; } = string.Empty; // es. https://api.backend.local/
    }
}
