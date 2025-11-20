//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Authorization;
//using SaipemE_PTW.Shared.Models;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;

//namespace SaipemE_PTW.Pages.Leave
//{
//    // Data: 2025-01-21 - Partial class per logica switch ruoli utente
//    public partial class Leave
//    {
//        [Inject]
//        private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

//        // Data: 2025-01-21 - Variabili per gestione ruolo utente
//        private UserRole? _currentUserRole;
//        private string _currentUserRoleName = string.Empty;

//        /// <summary>
//        /// Data: 2025-01-21 - Carica il ruolo dell'utente autenticato e configura la pagina di conseguenza
//        /// </summary>
//        private async Task LoadUserRoleAndConfigureAsync()
//        {
//            try
//            {
//                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
//                var user = authState.User;

//                if (user?.Identity?.IsAuthenticated != true)
//                {
//                    Logger.LogWarning("[Leave] Utente non autenticato");
//                    _errorMessage = "È necessario autenticarsi per accedere a questa pagina.";
//                    return;
//                }

//                // Data: 2025-01-21 - Estrazione claim ruolo
//                var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

//                if (string.IsNullOrWhiteSpace(roleClaim))
//                {
//                    Logger.LogWarning("[Leave] Claim ruolo non trovato per l'utente autenticato");
//                    _errorMessage = "Ruolo utente non trovato. Contattare l'amministratore.";
//                    return;
//                }

//                if (!UserRoleHelper.TryParse(roleClaim, out var parsedRole))
//                {
//                    Logger.LogWarning("[Leave] Ruolo non valido: {RoleClaim}", roleClaim);
//                    _errorMessage = "Ruolo utente non valido. Contattare l'amministratore.";
//                    return;
//                }

//                _currentUserRole = parsedRole;
//                _currentUserRoleName = UserRoleHelper.ToDisplayName(parsedRole);

//                Logger.LogInformation("[Leave] Utente autenticato con ruolo: {Role} ({DisplayName})",
//                    UserRoleHelper.ToCode(parsedRole), _currentUserRoleName);

//                // Data: 2025-01-21 - Switch comportamento in base al ruolo
//                await ConfigureForRoleAsync(_currentUserRole.Value);
//            }
//            catch (Exception ex)
//            {
//                Logger.LogError(ex, "[Leave] Errore durante il caricamento del ruolo utente");
//                _errorMessage = "Errore durante il caricamento del ruolo utente.";
//            }
//        }

//        /// <summary>
//        /// Data: 2025-01-21 - Configura la pagina in base al ruolo dell'utente
//        /// </summary>
//        private async Task ConfigureForRoleAsync(UserRole role)
//        {
//            switch (role)
//            {
//                case UserRole.AutoritaRichiedente:
//                    Logger.LogInformation("[Leave] Configurazione per Autorità Richiedente (RA)");
//                    await ConfigureForAutoritaRichiedenteAsync();
//                    break;

//                case UserRole.AutoritaEsecutrice:
//                    Logger.LogInformation("[Leave] Configurazione per Autorità Esecutrice (PA)");
//                    await ConfigureForAutoritaEsecutriceAsync();
//                    break;

//                case UserRole.PTWCoordinator:
//                    Logger.LogInformation("[Leave] Configurazione per PTW Coordinator");
//                    await ConfigureForPTWCoordinatorAsync();
//                    break;

//                case UserRole.AutoritaEmittente:
//                    Logger.LogInformation("[Leave] Configurazione per Autorità Emittente");
//                    await ConfigureForAutoritaEmittenteAsync();
//                    break;

//                case UserRole.CoordinatoreInEsecuzione:
//                    Logger.LogInformation("[Leave] Configurazione per Coordinatore in Esecuzione (CSE)");
//                    await ConfigureForCoordinatoreInEsecuzioneAsync();
//                    break;

//                case UserRole.PersonaAutorizzataTestGas:
//                    Logger.LogInformation("[Leave] Configurazione per Persona Autorizzata Test Gas (AGT)");
//                    await ConfigureForPersonaAutorizzataTestGasAsync();
//                    break;

//                case UserRole.AutoritaOperativa:
//                    Logger.LogInformation("[Leave] Configurazione per Autorità Operativa");
//                    await ConfigureForAutoritaOperativaAsync();
//                    break;

//                default:
//                    Logger.LogWarning("[Leave] Ruolo non gestito: {Role}", role);
//                    _errorMessage = $"Ruolo {role} non supportato.";
//                    break;
//            }
//        }

//        #region Metodi di configurazione per ruolo

//        /// <summary>
//        /// Data: 2025-01-21 - Configura la pagina per Autorità Richiedente (RA)
//        /// RA può creare e modificare bozze di permessi di lavoro
//        /// </summary>
//        private async Task ConfigureForAutoritaRichiedenteAsync()
//        {
//            // TODO: Implementare logica specifica per RA
//            // - Abilitare campi per compilazione iniziale
//            // - Filtrare permessi per stato InLavorazioneRA
//            // - Mostrare pulsanti "Salva in Bozze" e "Inoltra"
//            await Task.CompletedTask;
//        }

//        /// <summary>
//        /// Data: 2025-01-21 - Configura la pagina per Autorità Esecutrice (PA)
//        /// PA compila sezioni specifiche dopo inoltro da RA
//        /// </summary>
//        private async Task ConfigureForAutoritaEsecutriceAsync()
//        {
//            // TODO: Implementare logica specifica per PA
//            // - Abilitare campi specifici per PA
//            // - Filtrare permessi per stato InLavorazionePA
//            // - Mostrare sezione "Autorità Esecutrice"
//            await Task.CompletedTask;
//        }

//        /// <summary>
//        /// Data: 2025-01-21 - Configura la pagina per PTW Coordinator
//        /// PTW Coordinator effettua presa visione
//        /// </summary>
//        private async Task ConfigureForPTWCoordinatorAsync()
//        {
//            // TODO: Implementare logica specifica per PTW Coordinator
//            // - Mostrare permessi in stato InLavorazionePTWC
//            // - Modalità visualizzazione/presa visione
//            await Task.CompletedTask;
//        }

//        /// <summary>
//        /// Data: 2025-01-21 - Configura la pagina per Autorità Emittente
//        /// </summary>
//        private async Task ConfigureForAutoritaEmittenteAsync()
//        {
//            // TODO: Implementare logica specifica per Autorità Emittente
//            // - Mostrare permessi in stato InLavorazioneIA
//            await Task.CompletedTask;
//        }

//        /// <summary>
//        /// Data: 2025-01-21 - Configura la pagina per Coordinatore in Esecuzione (CSE)
//        /// </summary>
//        private async Task ConfigureForCoordinatoreInEsecuzioneAsync()
//        {
//            // TODO: Implementare logica specifica per CSE
//            // - Mostrare permessi in stato InLavorazioneCSE
//            await Task.CompletedTask;
//        }

//        /// <summary>
//        /// Data: 2025-01-21 - Configura la pagina per Persona Autorizzata Test Gas (AGT)
//        /// </summary>
//        private async Task ConfigureForPersonaAutorizzataTestGasAsync()
//        {
//            // TODO: Implementare logica specifica per AGT
//            // - Mostrare permessi in stato InLavorazioneAGT
//            // - Abilitare sezioni test gas
//            await Task.CompletedTask;
//        }

//        /// <summary>
//        /// Data: 2025-01-21 - Configura la pagina per Autorità Operativa
//        /// </summary>
//        private async Task ConfigureForAutoritaOperativaAsync()
//        {
//            // TODO: Implementare logica specifica per Autorità Operativa
//            // - Mostrare permessi in stato InApprovazioneOA
//            // - Approvazione finale
//            await Task.CompletedTask;
//        }

//        #endregion
//    }
//}
