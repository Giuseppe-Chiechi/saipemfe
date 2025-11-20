//using System;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;

//namespace SaipemE_PTW.Services.Auth
//{
//    // Data: 2025-10-20 - AuthenticationStateProvider personalizzato che legge JWT da storage
//    // e costruisce ClaimsPrincipal per le pagine Blazor
//    public sealed class CustomAuthStateProvider(ITokenStorageService storage, IAuthService auth, ILogger<CustomAuthStateProvider> logger) : AuthenticationStateProvider
//    {
//        private readonly ITokenStorageService _storage = storage;
//        private readonly IAuthService _auth = auth;
//        private readonly ILogger<CustomAuthStateProvider> _logger = logger;

//        //public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//        //{
//        //    try
//        //    {
//        //        //var principal = await _auth.GetClaimsPrincipalAsync();
//        //        //return new AuthenticationState(principal);
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        //_logger.LogError(ex, "Errore in GetAuthenticationStateAsync");
//        //        //return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//        //    }
//        //}

//        // Data: 2025-10-20 - Notifica cambi stato dopo login/logout
//        public void NotifyAuthenticationStateChangedSafe()
//        {
//            try
//            {
//                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Errore durante NotifyAuthenticationStateChangedSafe");
//            }
//        }
//    }
//}
