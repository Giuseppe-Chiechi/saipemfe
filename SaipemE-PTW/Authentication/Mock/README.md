# Sistema di Autenticazione Mock per e-PTW Saipem

## Panoramica

Questo sistema di autenticazione mock simula Microsoft Entra ID (Azure AD) per permettere lo sviluppo e il testing dell'applicazione e-PTW senza richiedere una connessione al tenant Azure reale.

## ?? Caratteristiche

- ? Simulazione completa dei claims di Entra ID
- ? 12 utenti predefiniti con diversi ruoli
- ? Supporto per tutti i ruoli dell'applicazione e-PTW
- ? Facile commutazione tra mock e autenticazione reale
- ? Interface utente per selezione utente
- ? Persistenza dello stato di autenticazione
- ? Completamente compatibile con `AuthorizeView` e `[Authorize]`

## ?? Ruoli Disponibili

L'applicazione supporta i seguenti ruoli:

| Codice | Descrizione | Responsabilità |
|--------|-------------|----------------|
| **RA** | Autorità Richiedente | Richiede permessi di lavoro |
| **PA** | Autorità Esecutrice | Esegue i lavori autorizzati |
| **IA** | Autorità Emittente | Emette i permessi di lavoro |
| **PTWC** | PTW Coordinator | Coordina il sistema dei permessi |
| **CSE** | Coordinatore In Esecuzione | Supervisiona l'esecuzione |
| **AGT** | Persona Autorizzata Test Gas | Esegue test atmosfere pericolose |
| **OA** | Autorità Operativa | Gestisce le operazioni |
| **EQ** | Esperto Qualificato | Consulenza tecnica specializzata |
| **SystemAdmin** | Amministratore Sistema | Configurazioni e gestione utenti |
| **SuperOwner** | Super Owner | Accesso completo al sistema |
| **Visitor** | Visitatore | Accesso in sola lettura |
| **Anonymous** | Anonymous | Utente non autenticato |

## ?? Utenti Mock Predefiniti

### 1. Mario Rossi (Super Owner)
- **Email**: mario.rossi@saipem.com
- **Ruoli**: SuperOwner, SystemAdmin
- **Uso**: Testing con privilegi completi

### 2. Laura Bianchi (PTW Coordinator)
- **Email**: laura.bianchi@saipem.com
- **Ruoli**: PTWC, IA
- **Uso**: Testing coordinamento e emissione permessi

### 3. Giuseppe Verdi (Autorità Emittente)
- **Email**: giuseppe.verdi@saipem.com
- **Ruoli**: IA
- **Uso**: Testing emissione permessi

### 4. Anna Ferrari (Autorità Richiedente)
- **Email**: anna.ferrari@saipem.com
- **Ruoli**: RA
- **Uso**: Testing richiesta permessi

### 5. Marco Colombo (Autorità Esecutrice)
- **Email**: marco.colombo@saipem.com
- **Ruoli**: PA
- **Uso**: Testing esecuzione lavori

### 6. Francesca Romano (Coordinatore In Esecuzione)
- **Email**: francesca.romano@saipem.com
- **Ruoli**: CSE
- **Uso**: Testing supervisione esecuzione

### 7. Roberto Esposito (Persona Autorizzata Test Gas)
- **Email**: roberto.esposito@saipem.com
- **Ruoli**: AGT
- **Uso**: Testing gestione atmosfere pericolose

### 8. Giulia Ricci (Autorità Operativa)
- **Email**: giulia.ricci@saipem.com
- **Ruoli**: OA
- **Uso**: Testing operazioni

### 9. Alessandro Russo (Esperto Qualificato)
- **Email**: alessandro.russo@saipem.com
- **Ruoli**: EQ
- **Uso**: Testing consulenza tecnica

### 10. Elena Marino (Amministratore Sistema)
- **Email**: elena.marino@saipem.com
- **Ruoli**: SystemAdmin
- **Uso**: Testing amministrazione

### 11. Luca Galli (Visitatore)
- **Email**: luca.galli@external.com
- **Ruoli**: Visitor
- **Uso**: Testing accesso limitato

### 12. Chiara Conti (Multi-Ruolo)
- **Email**: chiara.conti@saipem.com
- **Ruoli**: RA, PA, CSE
- **Uso**: Testing scenari con più ruoli

## ?? Configurazione

### Abilitare/Disabilitare Mock Authentication

Modifica il file `Authentication/Mock/MockAuthConfig.cs`:

```csharp
public const bool UseMockAuthentication = true;  // true = Mock, false = Entra ID
```

### Auto-Login (Opzionale)

Per sviluppo rapido, abilita il login automatico:

```csharp
public const bool EnableAutoLogin = true;
public const string DefaultMockUserEmail = "mario.rossi@saipem.com";
```

## ?? Utilizzo

### 1. Accesso Manuale

1. Avvia l'applicazione
2. Naviga a `/mock-login`
3. Seleziona un utente dalla lista
4. Clicca su "Accedi con Utente Selezionato"

### 2. Verifica Autenticazione

Dopo il login:
- Il nome utente e badge "MOCK" appaiono nel menu
- Naviga a `/user-info` per vedere tutti i dettagli
- Testa le autorizzazioni basate sui ruoli

### 3. Uso nei Componenti

```razor
@* Verifica autenticazione *@
<AuthorizeView>
    <Authorized>
        <p>Benvenuto @context.User.Identity.Name!</p>
    </Authorized>
    <NotAuthorized>
        <p>Devi effettuare il login</p>
    </NotAuthorized>
</AuthorizeView>

@* Verifica ruolo specifico *@
<AuthorizeView Roles="SuperOwner,SystemAdmin">
    <Authorized>
        <button>Funzione Amministrativa</button>
    </Authorized>
</AuthorizeView>
```

### 4. Uso nelle Pagine

```csharp
@page "/admin"
@attribute [Authorize(Roles = "SuperOwner,SystemAdmin")]

// Solo gli utenti con questi ruoli possono accedere
```

### 5. Uso nel Codice C#

```csharp
@inject MockAuthenticationStateProvider AuthStateProvider

// Verifica ruolo
if (AuthStateProvider.IsInRole(AppRoles.SuperOwner))
{
  // Logica per Super Owner
}

// Ottieni ruoli
var roles = AuthStateProvider.GetUserRoles();

// Ottieni utente corrente
var user = AuthStateProvider.GetCurrentUser();
```

## ?? Claims Implementati

Il sistema mock fornisce tutti i claims standard di Entra ID:

| Claim Type | Descrizione | Esempio |
|------------|-------------|---------|
| `sub` | Subject ID (identificatore univoco) | user-001 |
| `name` | Nome completo | Mario Rossi |
| `preferred_username` | Username preferito (email) | mario.rossi@saipem.com |
| `email` | Indirizzo email | mario.rossi@saipem.com |
| `oid` | Object ID in Entra ID | oid-001 |
| `tid` | Tenant ID | 12345678-... |
| `http://schemas.microsoft.com/ws/2008/06/identity/claims/role` | Ruoli utente | SuperOwner |
| `groups` | Gruppi di appartenenza | group-admin |

## ?? Passaggio a Produzione

Quando sei pronto per usare Microsoft Entra ID reale:

### Step 1: Disabilita Mock

```csharp
// Authentication/Mock/MockAuthConfig.cs
public const bool UseMockAuthentication = false;
```

### Step 2: Configura Entra ID

```json
// wwwroot/appsettings.json
{
  "AzureAd": {
    "Authority": "https://login.microsoftonline.com/{VERO_TENANT_ID}",
    "ClientId": "{VERO_CLIENT_ID}",
    "ValidateAuthority": true
  }
}
```

### Step 3: Verifica Script

Assicurati che `wwwroot/index.html` contenga:

```html
<script src="_content/Microsoft.Authentication.WebAssembly.Msal/AuthenticationService.js"></script>
<script src="_framework/blazor.webassembly.js"></script>
```

### Step 4: Test

- Riavvia l'applicazione
- Il sistema userà automaticamente Entra ID
- Il login reindirizzerà a Microsoft login page

## ??? Sicurezza

### ?? IMPORTANTE

- ? **NON** usare mock authentication in produzione
- ? **NON** committare credenziali reali di Entra ID
- ? Verificare sempre `MockAuthConfig.UseMockAuthentication = false` prima del deploy
- ? Rimuovere o nascondere `/mock-login` in produzione

### Best Practices

1. Usa variabili d'ambiente per la configurazione produzione
2. Implementa logging delle attività di autenticazione
3. Monitora gli accessi non autorizzati
4. Usa HTTPS sempre (anche in sviluppo)

## ?? Testing

### Test Autorizzazioni

Visita `/user-info` per:
- Vedere tutti i claims dell'utente
- Verificare i ruoli assegnati
- Testare le autorizzazioni per ogni ruolo

### Test Scenari

1. **Accesso Negato**: Prova ad accedere a pagine senza il ruolo richiesto
2. **Multi-Ruolo**: Usa Chiara Conti per testare utenti con più ruoli
3. **Visitatore**: Usa Luca Galli per testare accesso limitato
4. **Admin**: Usa Mario Rossi per testare funzionalità amministrative

## ?? Personalizzazione

### Aggiungere Nuovi Utenti

Modifica `Authentication/Mock/MockUserRepository.cs`:

```csharp
new MockUser
{
    Sub = "user-013",
    Name = "Nuovo Utente",
 PreferredUsername = "nuovo.utente@saipem.com",
    Email = "nuovo.utente@saipem.com",
    Oid = "oid-013",
    Tid = TenantId,
  Roles = new List<string> { AppRoles.RA },
    Groups = new List<string> { "group-custom" }
}
```

### Aggiungere Nuovi Ruoli

1. Aggiungi il ruolo in `Authentication/Mock/AppRoles.cs`
2. Aggiungi la descrizione in `RoleDescriptions`
3. Aggiorna `AllRoles` array
4. Assegna il ruolo agli utenti desiderati

## ?? Troubleshooting

### Il login non funziona
- Verifica che `MockAuthConfig.UseMockAuthentication = true`
- Controlla che l'utente esista in `MockUserRepository`
- Verifica la console browser per errori

### Claims mancanti
- Verifica che tutti i claims siano definiti in `MockUser.ToClaimsPrincipal()`
- Controlla `/user-info` per vedere i claims effettivi

### Autorizzazioni non funzionanti
- Usa nomi ruoli esatti da `AppRoles`
- Verifica che l'utente abbia il ruolo assegnato
- Controlla che `AuthorizeView` o `[Authorize]` usino i nomi corretti

## ?? Riferimenti

- [Microsoft Entra ID Documentation](https://learn.microsoft.com/en-us/entra/identity/)
- [Blazor Authentication](https://learn.microsoft.com/en-us/aspnet/core/blazor/security/)
- [MSAL.NET](https://learn.microsoft.com/en-us/entra/msal/dotnet/)

## ?? Data Implementazione

**15 Gennaio 2025** - Sistema Mock Authentication implementato per sviluppo e-PTW Saipem

---

**Nota**: Questa è una soluzione temporanea per lo sviluppo. In produzione, utilizzare sempre Microsoft Entra ID per autenticazione e autorizzazione reali.
