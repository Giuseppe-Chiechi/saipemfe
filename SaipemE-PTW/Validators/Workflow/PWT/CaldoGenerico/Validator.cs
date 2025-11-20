using FluentValidation;
using SaipemE_PTW.Shared.Models;
using SaipemE_PTW.Shared.Models.Auth;
using SaipemE_PTW.Shared.Models.PWT;


namespace SaipemE_PTW.Validators.Workflow.PWT.CaldoGenerico
{
    public class Validator : AbstractValidator<PermessoLavoroModel>
    {
        private readonly UserRole _ruolo;

        public Validator(UserRole ruolo)
        {
            _ruolo = ruolo;

            // Regole comuni a tutti i ruoli
            ApplicaRegoleComuni();

            // Regole specifiche per ruolo
            ApplicaRegolePerRuolo();
        }

        private void ApplicaRegoleComuni()
        {
            RuleFor(x => x.ValiditaDataInizio)
            .NotEmpty()
            .WithMessage("- La data di inizio validità è obbligatoria")
            .Must(d => d >= DateTime.Today)
            .WithMessage("- La data di inizio validità non può essere precedente a oggi");

            RuleFor(x => x.ValiditaOraInizio)
            .NotEmpty()
            .WithMessage("- Ora inizio validità è obbligatoria");

            RuleFor(x => x.ValiditaDataFine)
                .NotEmpty()
                .WithMessage("- La data di fine è obbligatoria")
                .Must(d => d >= DateTime.Today)
                .WithMessage("- La data di fine non può essere precedente a oggi")
                .Must((model, fine) =>
                    model.ValiditaDataInizio.HasValue && fine.HasValue
                    && fine.Value >= model.ValiditaDataInizio.Value.AddDays(14))
                .WithMessage("- La data di fine deve essere almeno 14 giorni dopo la data di inizio");


            RuleFor(x => x.ValiditaOraFine)
            .NotEmpty()
            .WithMessage("- Ora fine validità è obbligatoria");

            RuleFor(x => x.TipoAttivita)
          .NotNull()
          .WithMessage("- Devi selezionare almeno una attivita")
          .Must(x => x != null && x.Any())
          .WithMessage("- Devi selezionare almeno un attivita.");
        }

        private void ApplicaRegolePerRuolo()
        {
            switch (_ruolo)
            {
                case UserRole.AutoritaRichiedente:
                    ApplicaRegoleAutoritaRichiedente();
                break;

                case UserRole.AutoritaEsecutrice:
                    ApplicaRegoleAutoritaEsecutrice();
                break;

                case UserRole.AutoritaEmittente:
                    ApplicaRegoleAutoritaEmittente();
                    break;

                case UserRole.PTWCoordinator:
                    ApplicaRegolePTWCoordinator();
                    break;

                case UserRole.CoordinatoreInEsecuzioneCSE:
                    ApplicaRegoleCSE();
                    break;

                case UserRole.PersonaAutorizzataTestGas:
                    ApplicaRegolePersonaAutorizzataTestGas();
                break;

                case UserRole.AutoritaOperativa:
                    AutoritaOperativa();
                break;

                case UserRole.AmministratoreSistema:
                case UserRole.SuperOwner:
                    ApplicaRegoleAmministratore();
                break;
       

                default:
                    // Regole di default per utenti senza ruolo specifico o ruoli non riconosciuti
                    ApplicaRegoleDefault();
                    break;
            }
        }

        private void ApplicaRegoleAutoritaRichiedente()
        {
            RuleFor(x => x.AutoritaRichiedenteNome)
             .NotEmpty()
              .WithMessage("Il nome dell'autorità richiedente è obbligatorio");

            RuleFor(x => x.AutoritaRichiedenteCognome)
                   .NotEmpty()
                 .WithMessage("Il cognome dell'autorità richiedente è obbligatorio");

            RuleFor(x => x.DescrizioneAttivita)
                   .NotEmpty()
              .WithMessage("La descrizione dell'attività è obbligatoria");
        }

        private void ApplicaRegoleAutoritaEsecutrice()
        {
            RuleFor(x => x.AutoritaEsecutriceNome)
                .NotEmpty()
              .WithMessage("Il nome è obbligatorio");

            RuleFor(x => x.AutoritaEsecutriceCognome)
             .NotEmpty()
           .WithMessage("Il cognome è obbligatorio");

            RuleFor(x => x.AutoritaEsecutriceImpresa)
                .NotEmpty()
             .WithMessage("L'impresa è obbligatoria");

            RuleFor(x => x.TipoAllegati)
        .NotNull()
     .WithMessage("Devi selezionare almeno un allegato")
      .Must(x => x != null && x.Any())
        .WithMessage("Devi selezionare almeno un allegato");
        }

        private void ApplicaRegoleAutoritaEmittente()
        {
            RuleFor(x => x.AutoritaEmittenteNome)
                 .NotEmpty()
                     .WithMessage("Il nome dell'autorità emittente è obbligatorio");

            RuleFor(x => x.AutoritaEmittenteCognome)
           .NotEmpty()
         .WithMessage("Il cognome dell'autorità emittente è obbligatorio");

            RuleFor(x => x.CertificatoNumero)
                    .NotEmpty()
                           .WithMessage("Il numero del certificato è obbligatorio per l'autorità emittente");
        }

        private void ApplicaRegolePTWCoordinator()
        {
            RuleFor(x => x.AutoritaEmittenteNome)
                 .NotEmpty()
                     .WithMessage("Il nome dell'autorità emittente è obbligatorio");

            RuleFor(x => x.AutoritaEmittenteCognome)
           .NotEmpty()
         .WithMessage("Il cognome dell'autorità emittente è obbligatorio");

            RuleFor(x => x.CertificatoNumero)
                    .NotEmpty()
                           .WithMessage("Il numero del certificato è obbligatorio per l'autorità emittente");
        }

        private void ApplicaRegoleCSE()
        {
            RuleFor(x => x.CSENome)
   .NotEmpty()
                  .WithMessage("Il nome del CSE è obbligatorio");

            RuleFor(x => x.CSECognome)
       .NotEmpty()
           .WithMessage("Il cognome del CSE è obbligatorio");

            RuleFor(x => x.CSEFirma)
           .NotEmpty()
                      .WithMessage("La firma del CSE è obbligatoria");
        }

        private void ApplicaRegolePersonaAutorizzataTestGas()
        {
            RuleFor(x => x.CSENome)
   .NotEmpty()
                  .WithMessage("Il nome del CSE è obbligatorio");

            RuleFor(x => x.CSECognome)
       .NotEmpty()
           .WithMessage("Il cognome del CSE è obbligatorio");

            RuleFor(x => x.CSEFirma)
           .NotEmpty()
                      .WithMessage("La firma del CSE è obbligatoria");
        }

        private void AutoritaOperativa()
        {
            RuleFor(x => x.CSENome)
   .NotEmpty()
                  .WithMessage("Il nome del CSE è obbligatorio");

            RuleFor(x => x.CSECognome)
       .NotEmpty()
           .WithMessage("Il cognome del CSE è obbligatorio");

            RuleFor(x => x.CSEFirma)
           .NotEmpty()
                      .WithMessage("La firma del CSE è obbligatoria");
        }

        private void ApplicaRegoleAmministratore()
        {
            // Gli amministratori hanno meno restrizioni
            // Possono modificare tutti i campi senza vincoli aggiuntivi
        }

       

        

        

        private void ApplicaRegoleDefault()
        {
            // Regole più restrittive per utenti generici
            RuleFor(x => x.AutoritaEsecutriceNome)
            .NotEmpty()
             .WithMessage("Il nome è obbligatorio");

            RuleFor(x => x.TipoAllegati)
                .NotNull()
        .WithMessage("Devi selezionare almeno un allegato")
   .Must(x => x != null && x.Any())
     .WithMessage("Devi selezionare almeno un allegato");
        }
    }
}
