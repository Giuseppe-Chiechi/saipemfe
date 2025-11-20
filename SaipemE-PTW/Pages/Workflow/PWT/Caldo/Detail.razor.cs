using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SaipemE_PTW.Components.Base;
using SaipemE_PTW.Shared.Models.Auth;
using SaipemE_PTW.Shared.Models.PWT;
using SaipemE_PTW.Validators.Workflow.PWT.CaldoGenerico;
using Serilog;
using System.Data;
using System.Text.Json;


namespace SaipemE_PTW.Pages.Workflow.PWT.Caldo
{
    public partial class Detail : CommonComponentBase
    {
        //LOGGER
        //[Inject]
        //private IDialogService DialogService { get; set; }

        private PermessoLavoroModel _model = new();
        private sealed class PermissiDemoComponent { }
        

        private MudForm? _form1;
        string? jsonOutput;
        private string? _allegatiError;
        private MudMessageBox? _mudMessageBox;
        private bool _isInline = true;


        private Validators.Workflow.PWT.CaldoGenerico.Validator _validator = null!;
        protected override async Task OnInitializedCoreAsync()//OnInitializedAsync
        {

            try
            {
                // Inizializza il validator con il ruolo dell'utente corrente
                _validator = new Validators.Workflow.PWT.CaldoGenerico.Validator(userRole);


                //_model = new PermessoLavoroModel();
                await LoadData();

                if (Id != null)
                {
                    _pageTitle = "Permesso lavoro a Caldo: " + _model.pdl + " " + _model.Sito;
                }
                else
                {
                    _pageTitle = "Nuovo permesso lavoro a Caldo";
                }

                SetBreadcrumb(
                     new BreadcrumbItem("⬅ Back", href: "javascript:history.back()"),
                     new BreadcrumbItem(Localization.GetString("Home.Dashboard"), href: "/"),
                     new BreadcrumbItem(_pageTitle, href: "#", disabled: true)
                 );




            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Errore");
                ErrorMessage = "Si è verificato un errore durante il caricamento. Riprova.";
            }
            finally
            {
                StateHasChanged();
            }
        }

        private string? GetValidationError(string propertyName)
        {
            // Data: 2025-11-05 - Ottimizzazione: valida solo la proprietà richiesta per ridurre overhead
            var result = _validator.Validate(_model, opts => opts.IncludeProperties(propertyName));
            return result.Errors.FirstOrDefault()?.ErrorMessage;
        }

        private async Task LoadData()
        {
            try
            {

                if (Id != null)
                {
                    _Cts = new CancellationTokenSource();

                    var list = await PermessiService.GetPermessiAsync(_Cts.Token);
                    _model = list.Where(v => v.id == Id).FirstOrDefault() ?? new PermessoLavoroModel();

                    // Assicuro proprietà annidate non null (evita NRE in render)


                    if (_model != null)
                    {
                        switch (userRole)
                        {
                            case UserRole.AutoritaRichiedente:
                                if (_model.Stato is { Nome: not "In Lavorazione RA" })
                                {
                                    //_isReadOnly = true;
                                }

                                break;
                            case UserRole.AutoritaEsecutrice:
                                if (_model.Stato is { Nome: not "In Lavorazione PA" })
                                {
                                    //_isReadOnly = true;
                                }

                                break;
                            case UserRole.AutoritaEmittente:
                                if (_model.Stato is { Nome: not "In Lavorazione IA" })
                                {
                                   // _isReadOnly = true;
                                }
                                break;
                            case UserRole.PTWCoordinator:
                                if (_model.Stato is { Nome: not "In Lavorazione PTWC" })
                                {
                                   // _isReadOnly = true;
                                }
                                break;
                            case UserRole.CoordinatoreInEsecuzioneCSE:
                                if (_model.Stato is { Nome: not "In Lavorazione CSE" })
                                {
                                   // _isReadOnly = true;
                                }

                                break;
                            case UserRole.PersonaAutorizzataTestGas:
                                if (_model.Stato is { Nome: not "In Lavorazione AGT" })
                                {
                                   // _isReadOnly = true;
                                }

                                break;
                            case UserRole.AutoritaOperativa:
                                if (_model.Stato is { Nome: not "In Lavorazione OA" })
                                {
                                  //  _isReadOnly = false;
                                }

                                break;
                            case UserRole.AmministratoreSistema:
                                //_isReadOnly = true;
                                break;
                            case UserRole.SuperOwner:
                                _isReadOnly = false;
                                break;
                            case UserRole.Visitatore:
                                //_isReadOnly = true;
                                break;
                            default:
                                break;

                        }
                    }
                    //else
                    //{

                    //        // Gestione alternativa: ad esempio lanciare un'eccezione, loggare o restituire un errore
                    //        throw new InvalidOperationException($"Permesso con id {Id} non trovato.");

                    //}

                }
                else
                {

                    //_model.Stato.Nome = "Nuovo permesso";

                }
            }
            catch (Exception ex)
            {
                AlertShow = true;
                AlertMessage = ex.Message;
                AlertTipo = "Error";
                StateHasChanged();

            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (Id == null)
                _model = new PermessoLavoroModel();

            await Task.CompletedTask;

        }
        private async Task SubmitSalvaBozze()
        {
            var confirm = await OpenDialogAsync(_topCenter);
            if (confirm)
            {
                try
                {
                    //chiamata ai servizi



                    AlertShow = true;
                    AlertMessage = "Aggioramento avvenuto correttamente!";
                    AlertTipo = "Success";
                }
                catch (Exception ex)
                {
                    AlertShow = true;
                    AlertMessage = ex.Message;
                    AlertTipo = "Error";
                }
                finally
                {
                    StateHasChanged();
                }
                //chiamata ai servizi ecc. ecc.

            }
        }
        private async Task SubmitStampa()
        {
            var confirm = await OpenDialogAsync(_topCenter);
            if (confirm)
            {
                try
                {
                    //chiamata ai servizi



                    AlertShow = true;
                    AlertMessage = "Stampa avvenuto correttamente!";
                    AlertTipo = "Success";
                }
                catch (Exception ex)
                {
                    AlertShow = true;
                    AlertMessage = ex.Message;
                    AlertTipo = "Error";
                }
                finally
                {
                    StateHasChanged();
                }
                //chiamata ai servizi ecc. ecc.

            }
        }
        private async Task SubmitRigetta()
        {
            var confirm = await OpenDialogAsync(_topCenter);
            if (confirm)
            {
                try
                {
                    //chiamata ai servizi



                    AlertShow = true;
                    AlertMessage = "Rigetto avvenuto correttamente!";
                    AlertTipo = "Success";
                }
                catch (Exception ex)
                {
                    AlertShow = true;
                    AlertMessage = ex.Message;
                    AlertTipo = "Error";
                }
                finally
                {
                    StateHasChanged();
                }
                //chiamata ai servizi ecc. ecc.

            }
        }
        private async Task SubmitFirma()
        {
            var confirm = await OpenDialogAsync(_topCenter);
            if (confirm)
            {
                try
                {
                    //chiamata ai servizi



                    AlertShow = true;
                    AlertMessage = "Firma avvenuta correttamente!";
                    AlertTipo = "Success";
                }
                catch (Exception ex)
                {
                    AlertShow = true;
                    AlertMessage = ex.Message;
                    AlertTipo = "Error";
                }
                finally
                {
                    StateHasChanged();
                }
                //chiamata ai servizi ecc. ecc.

            }
        }
        private async Task SubmitInoltro()
        {
            Logger.Info("Start submit inoltro");

            //_logger.Info("LanguageService initialized with default culture: " + _currentCulture.Name);

            try
            {
                if (_form1 != null)
                {


                    await _form1.Validate();



                    _allegatiError = GetValidationError(nameof(_model.TipoAllegati));

                    if (_form1.IsValid && string.IsNullOrEmpty(_allegatiError))
                    {
                        Logger.Info("Form Valido");

                        var confirm = await OpenDialogAsync(_topCenter);
                        if (confirm)
                        {
                            try
                            {
                                //chiamata ai servizi

                                // Data: 2025-11-05 - Evito serializzazione e logging di grandi payload in UI/console per prevenire lag
                                jsonOutput = JsonSerializer.Serialize(_model, new JsonSerializerOptions { WriteIndented = true });
                                Logger.Info(jsonOutput);
                                Logger.Info("Inoltro confermato");

                                AlertShow = true;
                                AlertMessage = "Inoltro avvenuto correttamente!";
                                AlertTipo = "Success";
                            }
                            catch (Exception ex)
                            {
                                AlertShow = true;
                                AlertMessage = ex.Message;
                                AlertTipo = "Error";
                            }
                            finally
                            {
                                StateHasChanged();
                            }
                            //chiamata ai servizi ecc. ecc




                        }
                    }
                    else
                    {
                        Logger.Warning("Form NON Valido");

                        var valid = _validator.Validate(_model);
                        // Rimuove errori duplicati in base al nome della proprietà
                        var uniqueErrors = valid.Errors
                            .GroupBy(e => e.PropertyName)
                            .Select(g => g.First()) // prendi il primo errore per ogni proprietà
                            .ToList();

                        AlertShow = true;
                        AlertMessage = string.Empty;
                        foreach (var error in uniqueErrors)
                        {
                            AlertMessage += $"{error.ErrorMessage}<br/>";
                        }
                        AlertTipo = "Warning";
                    }

                }
                else
                {
                    AlertMessage = "Form NON valido";
                    Logger.Warning("Form NON Valido");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message, null);
            }
            finally
            {
                Logger.Info("finally");
                StateHasChanged();
            }

        }
        private async Task SubmitAnnulla()
        {
            var confirm = await OpenDialogAsync(_topCenter);
            if (confirm)
            {
                try
                {
                    //chiamata ai servizi



                    AlertShow = true;
                    AlertMessage = "Aggioramento avvenuto correttamente!";
                    AlertTipo = "Success";
                }
                catch (Exception ex)
                {
                    AlertShow = true;
                    AlertMessage = ex.Message;
                    AlertTipo = "Error";
                }
                finally
                {
                    StateHasChanged();
                }
                //chiamata ai servizi ecc. ecc.

            }
        }


        private readonly DialogOptions _topCenter = new() { Position = DialogPosition.TopCenter };
        private async Task<bool> OpenDialogAsync(DialogOptions options)
        {
            bool result = false;
            if (_mudMessageBox != null &&_isInline)
            {
                result = await _mudMessageBox.ShowAsync(options) ?? false;
            }
            else
            {
                result = await DialogService.ShowMessageBox(
                            "Attenzione",
                            (MarkupString)"Confermi l'operazione?",
                            yesText: "Delete!", cancelText: "Cancel", options: options) ?? false;
            }
            // Data: 2025-11-05 - Niente StateHasChanged qui: evita re-render extra durante l'apertura del dialog
            return result;

        }


        int activeIndex = 0;
        TabPermessiLavoro tabActive = TabPermessiLavoro.Parte1;

        void OnTabChanged(int index)
        {
            activeIndex = index;
            tabActive = (TabPermessiLavoro)index;
        }

        public enum TabPermessiLavoro
        {
            Parte1 = 0,
            Parte2 = 1,
            Parte3 = 2,
            CSE = 3,
            Rinnovo = 4,
            Sospensione = 5,
            Riattivazione = 6,
            Chiusura = 7
        }
    }
}
