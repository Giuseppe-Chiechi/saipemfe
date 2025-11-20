using SaipemE_PTW.Shared.Models.PWT;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SaipemE_PTW.Shared.Models.Auth
{
    public class UserRoleDto
    {
        public int Id { get; set; }
        public string? Codice { get; set; }
        public string? Nome { get; set; }
        public string? Descrizione { get; set; }
        public string? Email { get; set; }
    }


    // Data: 2025-10-20 - Enum ruoli applicativi PTW (con mapping a codici role claim)
    public enum UserRole
    {
        // Data: 2025-10-20 - Autorità Richiedente (RA)
        [Display(Name = "Autorità Richiedente (RA)", ShortName = "RA", Description = "")]
        AutoritaRichiedente = 1,
        // Data: 2025-10-20 - Autorità Esecutrice (PA)
        [Display(Name = "Autorita Esecutrice (PA)", ShortName = "PA", Description = "")]
        AutoritaEsecutrice = 2,
        // Data: 2025-10-20 - Autorità Emittente
        [Display(Name = "Autorita Emittente (IA)", ShortName = "IA", Description = "")]
        AutoritaEmittente = 3,
        // Data: 2025-10-20 - PTW Coordinator
        [Display(Name = "PTW Coordinator (PTWC)", ShortName = "PTWC", Description = "")]
        PTWCoordinator = 4,
        
        // Data: 2025-10-20 - Coordinatore in Esecuzione (CSE)
        [Display(Name = "Coordinatore In Esecuzione (CSE)", ShortName = "CSE", Description = "")]
        CoordinatoreInEsecuzioneCSE = 5,
        // Data: 2025-10-20 - Persona Autorizzata ai Test dei Gas (AGT)
        [Display(Name = "Persona Autorizzata Test Gas (AGT)", ShortName = "AGT", Description = "")]
        PersonaAutorizzataTestGas = 6,
        // Data: 2025-10-20 - Autorità Operativa
        [Display(Name = "Autorita Operativa (OA)", ShortName = "OA", Description = "")]
        AutoritaOperativa = 7,
        [Display(Name = "Esperto Qualificato (EQ)", ShortName = "EQ", Description = "")]
        EspertoQualificatoEQ = 8,
        [Display(Name = "Amministratore Sistema", ShortName = "", Description = "")]
        AmministratoreSistema = 9,
        [Display(Name = "Super Owner", ShortName = "", Description = "")]
        SuperOwner = 10,
        [Display(Name = "Visitatore", ShortName = "", Description = "Possono solo vedere")]
        Visitatore = 11,
        [Display(Name = "Anonymous", ShortName = "", Description = "")]
        Anonymous = 12

    }

   

    public static class UserRoleMapper
    {
        public static UserRoleDto ToDto(this UserRole tipo)
        {
            return new UserRoleDto
            {
                Id = (int)tipo,
                Codice = tipo.GetShortName(),
                Nome = tipo.GetDisplayName(),
                Descrizione = tipo.GetDescription()
            };
        }

        public static IEnumerable<UserRoleDto> GetAll()
        {
            return Enum.GetValues(typeof(UserRole))
                .Cast<UserRole>()
                .Select(e => e.ToDto());
        }
    }

    // Data: 2025-10-20 - Helper ruoli: mapping codici, display e controlli su ClaimsPrincipal
    public static class UserRoleHelper
    {
        // Data: 2025-10-20 - Codici normalizzati (usati come claim Role)
        private static readonly IReadOnlyDictionary<UserRole, string> Codes = new Dictionary<UserRole, string>
        {
             [UserRole.AutoritaRichiedente] = "RA",
        [UserRole.AutoritaEsecutrice] = "PA",
          [UserRole.AutoritaEmittente] = "IA",
      [UserRole.PTWCoordinator] = "PTWC",
   [UserRole.CoordinatoreInEsecuzioneCSE] = "CSE",
            [UserRole.PersonaAutorizzataTestGas] = "AGT",
     [UserRole.AutoritaOperativa] = "OA",
        [UserRole.EspertoQualificatoEQ] = "EQ",
            [UserRole.AmministratoreSistema] = "ADMIN",
       [UserRole.SuperOwner] = "SUPER_OWNER",
      [UserRole.Visitatore] = "VISITOR",
     [UserRole.Anonymous] = "ANONYMOUS",
   };

        // Data: 2025-10-20 - Etichette per UI (IT)
        private static readonly IReadOnlyDictionary<UserRole, string> Display = new Dictionary<UserRole, string>
        {
            [UserRole.AutoritaRichiedente] = "Autorità Richiedente (RA)",
            [UserRole.AutoritaEsecutrice] = "Autorità Esecutrice (PA)",
            [UserRole.PTWCoordinator] = "PTW Coordinator (PTWC)",
            [UserRole.AutoritaEmittente] = "Autorità Emittente (IA)",
            [UserRole.CoordinatoreInEsecuzioneCSE] = "Coordinatore in Esecuzione (CSE)",
            [UserRole.PersonaAutorizzataTestGas] = "Persona Autorizzata ai Test dei Gas (AGT)",
            [UserRole.AutoritaOperativa] = "Autorità Operativa (OA)",
            [UserRole.EspertoQualificatoEQ] = "Esperto Qualificato (EQ)",
            [UserRole.AmministratoreSistema] = "Amministratore di sistema",
            [UserRole.SuperOwner] = "Super Owner",
            [UserRole.Visitatore] = "Visitatore",
            [UserRole.Anonymous] = "Anonymous"
        };

        // Data: 2025-10-20 - Ritorna il codice da salvare nel claim Role
        public static string ToCode(UserRole role) => Codes[role];

        // Data: 2025-10-20 - Ritorna l’etichetta utente
        public static string ToDisplayName(UserRole role) => Display[role];

        // Data: 2025-10-20 - Parsing sicuro da codice/label (accetta sia codice che etichetta)
        public static bool TryParse(string? value, out UserRole role)
        {
            role = default;
            if (string.IsNullOrWhiteSpace(value)) return false;

            var v = value.Trim();

            // match per codice
            foreach (var kv in Codes)
            {
                if (string.Equals(kv.Value, v, StringComparison.OrdinalIgnoreCase))
                {
                    role = kv.Key; return true;
                }
            }

            // match per display name
            foreach (var kv in Display)
            {
                if (string.Equals(kv.Value, v, StringComparison.OrdinalIgnoreCase))
                {
                    role = kv.Key; return true;
                }
            }

            return false;
        }

        // Data: 2025-10-20 - Verifica ruolo su ClaimsPrincipal (usa ClaimTypes.Role)
        public static bool HasRole(this ClaimsPrincipal user, UserRole role)
        {
            if (user?.Identity?.IsAuthenticated != true) return false;
            var code = ToCode(role);
            foreach (var claim in user.FindAll(ClaimTypes.Role))
            {
                if (string.Equals(claim.Value, code, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
