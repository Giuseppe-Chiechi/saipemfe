using SaipemE_PTW.Shared.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaipemE_PTW.Services.User
{
    public class UserService_Moke : IUserService
    {
        public Task<UserDto?> GetUserByIdAsync(string userId)
        {
            // Dati fittizi per testare l’interfaccia utente
            var fakeUser = new UserDto
            {
                Id = userId,
                FullName = "Mario Rossi",
                Email = "mario.rossi@example.com"
            };

            return Task.FromResult<UserDto?>(fakeUser);
        }
    }
}
