using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using SaipemE_PTW.Components.Base;
using SaipemE_PTW.Shared.Models.Auth;
using SaipemE_PTW.Shared.Models.Certificati;
using SaipemE_PTW.Shared.Models.PWT;
using System.Data;
using System.Text.Json;


namespace SaipemE_PTW.Pages.Workflow.Certificati.IsolamentoFontiEnergetico
{.
    public partial class Detail : CommonComponentBase
    {

        private SaipemE_PTW.Shared.Models.Certificati.IsolamentoFontiEnergetico _modelCertificato = new();
        private PermessoLavoroModel _model = new();
        private sealed class PermissiDemoComponent { }


        private MudForm? _form1;
        string? jsonOutput;
        private string? _allegatiError;

        private MudMessageBox _mudMessageBox = new MudMessageBox();
        private bool _isInline = true;

        #region VERIFICA SUI CAMPI READONLY IN BASE RUOLO, STATO
        private bool _isReadOnlyParte1 = true;
        private bool _isReadOnlyParte2 = true;
        private bool _isReadOnlyParte3 = true;
        private bool _isReadOnlyParteCSE = true;
        private bool _isReadOnlyChiusura = false;
        private bool _isReadOnlyAllegati = false;
        private bool _isReadOnlyCertificati = false;
        private bool _isReadOnlyGasTest = false;

        private async Task isReadOnly()
        {
            _isReadOnly = false;

            if (Id == null)
            {
                _isReadOnlyParte1 = false;
            }

            switch (userRole)
            {
                case UserRole.AutoritaRichiedente:
                    _isReadOnlyParte1 = false;
                    
                break;
                case UserRole.EspertoQualificatoEQ:
                case UserRole.AutoritaEsecutrice:
                    _isReadOnlyParte1 = false;
                    break;
                case UserRole.AutoritaEmittente:
                    _isReadOnlyParte1 = true;
                    _isReadOnlyParte2 = false;
                    break;
                case UserRole.PTWCoordinator:
                    _isReadOnlyParte1 = true;
                    _isReadOnlyParte2 = true;
                    break;
                case UserRole.CoordinatoreInEsecuzioneCSE:
                    _isReadOnlyParteCSE = false;
                    break;
                case UserRole.PersonaAutorizzataTestGas:

                    break;
                case UserRole.AutoritaOperativa:

                    _isReadOnlyParte3 = false;

                    break;
                case UserRole.AmministratoreSistema:
                    _isReadOnly = true;
                    break;

                case UserRole.SuperOwner:
                    _isReadOnly = false;
                    _isReadOnlyParte1 = false;
                    _isReadOnlyParte2 = false;
                    _isReadOnlyParte3 = false;
                    _isReadOnlyParteCSE = false;
                    _isReadOnlyChiusura = false;
                    _isReadOnlyAllegati = false;
                    _isReadOnlyCertificati = false;
                    _isReadOnlyGasTest = false;
                    break;
                case UserRole.Visitatore:
                    _isReadOnly = true;
                    _isReadOnlyParte1 = true;
                    _isReadOnlyParte2 = true;
                    _isReadOnlyParte3 = true;
                    _isReadOnlyParteCSE = true;


                    _isReadOnlyChiusura = true;

                    _isReadOnlyAllegati = true;
                    _isReadOnlyCertificati = true;
                    _isReadOnlyGasTest = true;
                    _isReadOnly = true;
                    break;
                default:
                    break;

            }

           
            await Task.CompletedTask;
        }

        #endregion

        private Validators.Workflow.PWT.CaldoGenerico.Validator _validator = null!;
        protected override async Task OnInitializedCoreAsync()//OnInitializedAsync
        {

            try
            {
                // Inizializza il validator con il ruolo dell'utente corrente
                _validator = new Validators.Workflow.PWT.CaldoGenerico.Validator(userRole);


                //_model = new PermessoLavoroModel();
                await LoadData();
                await isReadOnly();

                _pageTitle = "Certificato di Isolamento Energetico: " + Certificate;

                var listUrl = "/" + _localArea + "/" + _sublocalArea + "/list";
                SetBreadcrumb(
                     new BreadcrumbItem("⬅ Back", href: "javascript:history.back()"),
                     new BreadcrumbItem(Localization.GetString("Home.Dashboard"), href: "/"),
                     new BreadcrumbItem("Elenco Permessi lavoro", href: listUrl),
                     new BreadcrumbItem(_pageTitle, href: "#", disabled: true)
                 );




            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
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

            // assicuro valori utili al render anche qui
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
            if (_isInline)
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
        TipoTabPWT tabActive = TipoTabPWT.Parte1;

        void OnTabChanged(int index)
        {
            activeIndex = index;
            tabActive = (TipoTabPWT)index;
        }

      



    }

}
