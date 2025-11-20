using SaipemE_PTW.Shared.Models.Auth;

namespace SaipemE_PTW.Services.User
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(string userId);
    }
}
