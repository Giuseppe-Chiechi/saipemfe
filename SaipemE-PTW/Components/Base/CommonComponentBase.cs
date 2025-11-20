using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using SaipemE_PTW.Services;
using SaipemE_PTW.Services.Common;
using SaipemE_PTW.Shared.Models.Auth;
using System.Security.Claims;

namespace SaipemE_PTW.Components.Base
{
    public abstract class CommonComponentBase : ComponentBase, IDisposable
    {
        #region Servizi Iniettati
        [Inject] public ILocalizationService Localization { get; set; } = default!;
        [Inject] public ILanguageService LanguageService { get; set; } = default!;
        [Inject] protected IDialogService DialogService { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;
        [Inject] protected ILoggingService Logger { get; set; } = default!; // logging locale/sincrono
        [Inject] protected ILoggerService AsyncLogger { get; set; } = default!; // logging remoto/asincrono
        [Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [CascadingParameter] protected Task<AuthenticationState> AuthStateTask { get; set; } = default!;
        #endregion

        #region Proprietà Comuni
        protected string _pageTitle { get; set; } = string.Empty;
        protected string? _localArea = string.Empty;
        protected string? _sublocalArea = string.Empty;
        protected string _lang = "en";
        protected bool _loading = true;
        protected bool _isReadOnly = true;

        /// <summary>Ruolo utente autenticato</summary>
        //protected UserRole userRole { get; set; }

        /// <summary>Messaggio errore da visualizzare</summary>
        protected string ErrorMessage { get; set; } = string.Empty;

        /// <summary>Stato caricamento componente</summary>
        protected bool IsLoading { get; set; }

        /// <summary>Token cancellazione chiamate async</summary>
        protected CancellationTokenSource? _Cts { get; set; }

        // Alert comuni
        protected string? AlertTipo { get; set; }
        protected string? AlertMessage { get; set; }
        protected bool AlertShow { get; set; }

        // Breadcrumb
        protected List<BreadcrumbItem> _BreadcrumbItems { get; set; } = new();

        protected int si = 1;
        protected int no = 0;
        #endregion

        //protected string? userRole;
        protected UserRole userRole { get; set; }

        protected ClaimsPrincipal? currentUser;

        protected void LocalArea()
        {
            var uri = Navigation.Uri; // URL completo, es: https://localhost:5001/monitoring/general-hot/list
            var path = new Uri(uri).AbsolutePath; // solo path: /monitoring/general-hot/list
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length > 0)
            {
                _localArea = segments[0];

                if (segments.Length > 1)
                    _sublocalArea = segments[1];
            }
        }

        #region Lifecycle
        protected override async Task OnInitializedAsync()
        {


            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            currentUser = authState.User;
            userRole = UserRole.Anonymous; // Default

            if (currentUser?.Identity?.IsAuthenticated == false)
            {

            }
            else
            {


                var roleClaim = currentUser?.FindFirst(ClaimTypes.Role)?.Value ?? currentUser?.FindFirst("roles")?.Value;

                if (!string.IsNullOrWhiteSpace(roleClaim)
                    && UserRoleHelper.TryParse(roleClaim, out var parsedRole))
                {
                    userRole = parsedRole;
                }
                //userRole = currentUser?.FindFirst(ClaimTypes.Role)?.Value
                //  ?? currentUser?.FindFirst("roles")?.Value;

                _Cts = new CancellationTokenSource();
            }
                IsLoading = true;
                ErrorMessage = string.Empty;

                try
                {
                    LocalArea();

                    // Log inizializzazione componente (remoto + locale)
                    //var initProps = new Dictionary<string, object?>
                    //{
                    //    ["ComponentType"] = GetType().FullName,
                    //    ["LocalArea"] = _localArea,
                    //    ["SubLocalArea"] = _sublocalArea
                    //};
                    //await LogInfoAsync($"Initializing component: {GetType().Name}", initProps);

                    // Sottoscrivi evento cambio lingua
                    LanguageService.LanguageChanged += OnLanguageChanged;
                    _lang = LanguageService.GetCurrentLanguageCodeShort().ToLower();
                    //await LoadUserRoleAsync();
                    await OnInitializedCoreAsync();
               
                //await LogInfoAsync($"Component initialized successfully: {GetType().Name}");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Si è verificato un errore durante il caricamento.";

                var errorProps = new Dictionary<string, object?>
                {
                    ["ComponentType"] = GetType().FullName,
                    ["ErrorMessage"] = ErrorMessage
                };
                var errorId = await LogErrorAsync(ex, $"Error initializing component: {GetType().Name}", errorProps);

                // Mostra correlation ID all'utente per supporto tecnico
                ErrorMessage = $"{ErrorMessage} (ID: {errorId})";
            }
            finally
            {
                IsLoading = false;
            }
        }
        /// <summary>
        /// Override questo metodo nei componenti derivati per logica di inizializzazione specifica.
        /// Viene chiamato automaticamente anche quando cambia la lingua.
        /// </summary>
        protected virtual Task OnInitializedCoreAsync() => Task.CompletedTask;

        /// <summary>
        /// Handler cambio lingua: ricarica contenuto localizzato e aggiorna UI.
        /// Data:2025-11-04 - Gestione automatica refresh localizzazione
        /// Data:2025-01-20 - Aggiunto logging cambio lingua
        /// </summary>
        protected virtual void OnLanguageChanged(object? sender, EventArgs e)
        {
            // Data:2025-01-20 - Log cambio lingua
            Logger.Info($"Language changed in component: {GetType().Name}", new Dictionary<string, object?>
            {
                ["ComponentType"] = GetType().FullName,
                ["NewLanguage"] = LanguageService.GetCurrentLanguageCode()
            });
            _lang = LanguageService.GetCurrentLanguageCodeShort().ToLower();
            // Ricarica contenuto localizzato (titoli, breadcrumb, ecc.)
            InvokeAsync(async () =>
            {
                try
                {
                    // Richiama il metodo di inizializzazione per ricaricare i contenuti localizzati
                    await OnInitializedCoreAsync();

                    // Forza aggiornamento UI
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    // Data:2025-01-20 - Log errore cambio lingua
                    var errorId = Logger.Error(ex, $"Error during language change in component: {GetType().Name}", new Dictionary<string, object?>
                    {
                        ["ComponentType"] = GetType().FullName
                    });

                    ErrorMessage = $"Errore durante il cambio lingua: {ex.Message} (ID: {errorId})";
                }
            });
        }
        #endregion

        #region Metodi Helper
        /// <summary>
        /// Costruisce dizionario proprietà per log arricchito con info utente/component.
        /// </summary>
        protected async Task<Dictionary<string, object?>> BuildUserLogPropertiesAsync(IDictionary<string, object?>? extra = null)
        {
            var props = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
            {
                ["ComponentType"] = GetType().FullName,
                ["Language"] = LanguageService.GetCurrentLanguageCode(),
                ["LocalArea"] = _localArea,
                ["SubLocalArea"] = _sublocalArea
            };
            if (extra != null)
            {
                foreach (var kv in extra) props[kv.Key] = kv.Value;
            }
            try
            {
                var auth = await AuthStateTask;
                var user = auth.User;
                if (user?.Identity?.IsAuthenticated == true)
                {
                    props["UserName"] = user.Identity?.Name;
                    props["UserId"] = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    props["UserRoleClaim"] = user.FindFirst(ClaimTypes.Role)?.Value;
                    props["Authenticated"] = true;
                }
                else
                {
                    props["Authenticated"] = false;
                }
            }
            catch
            {
                // ignore auth retrieval errors in logging props
            }
            return props;
        }

        protected async Task LogInfoAsync(string message, IDictionary<string, object?>? properties = null)
        {
            var props = await BuildUserLogPropertiesAsync(properties);
            await AsyncLogger.LogInfoAsync(message, props);
        }

        protected async Task LogWarningAsync(string message, IDictionary<string, object?>? properties = null)
        {
            var props = await BuildUserLogPropertiesAsync(properties);
            await AsyncLogger.LogWarningAsync(message, props);
        }

        protected async Task<string> LogErrorAsync(Exception ex, string message, IDictionary<string, object?>? properties = null)
        {
            var props = await BuildUserLogPropertiesAsync(properties);
            return await AsyncLogger.LogErrorAsync(ex, message, props);
        }

        /// <summary>
        /// Carica il ruolo dell'utente autenticato
        /// </summary>
        //protected async Task LoadUserRoleAsync()
        //{
        //    try
        //    {
        //        //userRole = UserRole.Anonymous;

        //        var state = await AuthStateTask;
        //        var user = state.User;
        //        var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;

        //        if (!string.IsNullOrWhiteSpace(roleClaim) && UserRoleHelper.TryParse(roleClaim, out var parsedRole))
        //        {
        //            userRole = parsedRole;

        //            // Data:2025-01-20 - Log ruolo utente caricato
        //            Logger.Info($"User role loaded: {userRole}", new Dictionary<string, object?>
        //            {
        //                ["ComponentType"] = GetType().FullName,
        //                ["UserRole"] = userRole.ToString(),
        //                ["RoleClaim"] = roleClaim
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Data:2025-01-20 - Log warning per errore caricamento ruolo
        //        Logger.Warning($"Failed to load user role in {GetType().Name}: {ex.Message}", new Dictionary<string, object?>
        //        {
        //            ["ComponentType"] = GetType().FullName,
        //            ["Exception"] = ex.GetType().Name
        //        });
        //    }
        //}

        /// <summary>
        /// Mostra un alert con tipo e messaggio personalizzati
        /// </summary>
        protected void ShowAlert(string message, string tipo = "Info")
        {
            AlertMessage = message;
            AlertTipo = tipo;
            AlertShow = true;

            // Data:2025-01-20 - Log alert mostrato
            Logger.Info($"Alert displayed: {tipo}", new Dictionary<string, object?>
            {
                ["ComponentType"] = GetType().FullName,
                ["AlertType"] = tipo,
                ["Message"] = message
            });

            StateHasChanged();
        }

        /// <summary>Mostra alert di successo</summary>
        protected void ShowSuccessAlert(string message) => ShowAlert(message, "Success");

        /// <summary>Mostra alert di errore</summary>
        protected void ShowErrorAlert(string message) => ShowAlert(message, "Error");

        /// <summary>
        /// Imposta il breadcrumb del componente
        /// </summary>
        protected void SetBreadcrumb(params BreadcrumbItem[] items)
        {
            _BreadcrumbItems = new List<BreadcrumbItem>(items);
            StateHasChanged();
        }

        /// <summary>
        /// Mostra un dialog di conferma standard
        /// </summary>
        protected async Task<bool?> ShowConfirmDialogAsync(string title, string message)
        {
            // Data:2025-01-20 - Log apertura dialog
            Logger.Info($"Confirm dialog opened: {title}", new Dictionary<string, object?>
            {
                ["ComponentType"] = GetType().FullName,
                ["DialogTitle"] = title
            });

            var result = await DialogService.ShowMessageBox(
                title,
                message,
                yesText: "Conferma",
                cancelText: "Annulla",
                options: new DialogOptions { Position = DialogPosition.TopCenter });

            // Data:2025-01-20 - Log risposta dialog
            Logger.Info($"Confirm dialog result: {result}", new Dictionary<string, object?>
            {
                ["ComponentType"] = GetType().FullName,
                ["DialogTitle"] = title,
                ["Result"] = result?.ToString() ?? "null"
            });

            return result;
        }
        #endregion

        #region Cleanup
        public virtual void Dispose()
        {
            try
            {
                // Data:2025-01-20 - Log dispose componente
                Logger.Info($"Disposing component: {GetType().Name}", new Dictionary<string, object?>
                {
                    ["ComponentType"] = GetType().FullName
                });

                LanguageService.LanguageChanged -= OnLanguageChanged;
                _Cts?.Cancel();
                _Cts?.Dispose();
            }
            catch (Exception ex)
            {
                // Data:2025-01-20 - Log errore durante dispose (non bloccante)
                Logger.Warning($"Error disposing component {GetType().Name}: {ex.Message}", new Dictionary<string, object?>
                {
                    ["ComponentType"] = GetType().FullName,
                    ["Exception"] = ex.GetType().Name
                });
            }
        }
        #endregion

        protected void GoBack()
        {
            // Data:2025-01-20 - Log navigazione indietro
            Logger.Info($"Navigating back from component: {GetType().Name}", new Dictionary<string, object?>
            {
                ["ComponentType"] = GetType().FullName
            });

            Navigation.NavigateTo("javascript:history.back()");
        }

        public Task OnValueChanged(int value, Action<int> propertySetter)
        {
            propertySetter(value);
            return Task.CompletedTask;
        }

        protected void HandleListClick(int? id)
        {
            Navigation.NavigateTo($"/{_localArea}/{_sublocalArea}/detail?id={id}");
        }

        //protected bool IsInRole(UserRole role)
        //{
        //    return currentUser?.IsInRole(role) ?? false;
        //}
    }
}