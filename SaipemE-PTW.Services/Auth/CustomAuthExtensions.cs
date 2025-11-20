////using Microsoft.AspNetCore.Components.Authorization;
//using Microsoft.Extensions.DependencyInjection;
//using SaipemE_PTW.Shared.Models.Auth;

//namespace SaipemE_PTW.Services.Auth
//{
//    // Data: 2025-10-20 - Estensioni registrazione servizi auth mock in DI
//    public static class CustomAuthExtensions
//    {
//        public static IServiceCollection AddMockOidcAuthentication(this IServiceCollection services)
//        {
//            // Data: 2025-10-20 - AuthorizationCore con policy ruoli
//            services.AddAuthorizationCore(options =>
//            {
//                options.AddPolicy("RAOnly", p => p.RequireRole(UserRoleHelper.ToCode(UserRole.AutoritaRichiedente)));
//                options.AddPolicy("PAOnly", p => p.RequireRole(UserRoleHelper.ToCode(UserRole.AutoritaEsecutrice)));
//                options.AddPolicy("PTWCoordinatorOnly", p => p.RequireRole(UserRoleHelper.ToCode(UserRole.PTWCoordinator)));
//                options.AddPolicy("EmittenteOnly", p => p.RequireRole(UserRoleHelper.ToCode(UserRole.AutoritaEmittente)));
//                options.AddPolicy("CSEOnly", p => p.RequireRole(UserRoleHelper.ToCode(UserRole.CoordinatoreInEsecuzioneCSE)));
//                options.AddPolicy("AGTOnly", p => p.RequireRole(UserRoleHelper.ToCode(UserRole.PersonaAutorizzataTestGas)));
//                options.AddPolicy("OperativaOnly", p => p.RequireRole(UserRoleHelper.ToCode(UserRole.AutoritaOperativa)));
//                options.AddPolicy("SuperOwnerOnly", p => p.RequireRole(UserRoleHelper.ToCode(UserRole.SuperOwner)));
//                options.AddPolicy("AmministratoreSistemaOnly", p => p.RequireRole(UserRoleHelper.ToCode(UserRole.AmministratoreSistema)));
//            });

//            services.AddScoped<ITokenStorageService, ProtectedLocalStorageTokenStore>();
//            services.AddScoped<IAuthService, MockOidcAuthService>();
//            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

           

//            return services;
//        }
//    }
//}
