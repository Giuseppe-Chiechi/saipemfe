using SaipemE_PTW.Shared.Models.PWT;


namespace SaipemE_PTW.Services.Administrator
{
    public interface IUtentiInterniService
    {
        Task<List<AnagraficaUtentiInterniDto>> GetUtentiAsync(string? search = null);
    }
}
