using SaipemE_PTW.Shared.Models.Auth;
using System.Net.Http.Json;

namespace SaipemE_PTW.Services.User
{
    public class UserService : IUserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            return await _http.GetFromJsonAsync<UserDto>($"api/users/{userId}");
        }
    }
}
