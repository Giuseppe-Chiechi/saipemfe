using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Data: 2025-11-05 - Estensioni DI compatibili senza IConfiguration.
    /// Nota: Non viene usata nella registrazione principale; tenuta per retrocompatibilità.
    /// </summary>
    public static class ServiceRegistrationExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            // Data: 2025-11-05 - Handler che inietta JWT
            services.AddTransient<SaipemE_PTW.Services.Http.JwtAuthorizationMessageHandler>();

            // Data: 2025-11-05 - HttpClient di base condiviso
            //services.AddHttpClient<SaipemE_PTW.Services.Http.SafeHttpClient>()
            //    .AddHttpMessageHandler<SaipemE_PTW.Services.Http.JwtAuthorizationMessageHandler>();

            return services;
        }
    }
}
