# ??? Saipem e-PTW - Sistema Elettronico Permessi di Lavoro

[![.NET Version](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor WebAssembly](https://img.shields.io/badge/Blazor-WebAssembly-512BD4?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License](https://img.shields.io/badge/license-Proprietary-red)](LICENSE)
[![MudBlazor](https://img.shields.io/badge/UI-MudBlazor-594ae2)](https://mudblazor.com/)

## ?? Descrizione

**Saipem e-PTW** è un sistema web per la gestione digitale dei Permessi di Lavoro (Permit To Work) in ambienti industriali complessi. L'applicazione permette di digitalizzare l'intero workflow di richiesta, approvazione ed esecuzione dei permessi di lavoro, garantendo tracciabilità completa e conformità alle normative di sicurezza.

Il sistema è progettato per gestire diversi tipi di autorità e ruoli operativi (Autorità Richiedente, Esecutrice, Emittente, PTW Coordinator, ecc.) con workflow personalizzati e controlli di sicurezza integrati.

---

## ?? Caratteristiche Principali

- ? **Gestione completa workflow PTW** - Dalla richiesta all'esecuzione e chiusura
- ?? **Autenticazione sicura** - Integrazione Microsoft Entra ID (Azure AD) + Mock Auth per sviluppo
- ?? **Sistema ruoli avanzato** - 12 ruoli predefiniti con autorizzazioni granulari
- ?? **Gestione allegati** - Upload e download documenti con tipizzazione
- ?? **Multi-lingua** - Supporto Italiano/Inglese
- ?? **Dashboard interattive** - Visualizzazione stato permessi e statistiche
- ?? **Sincronizzazione real-time** - Aggiornamenti automatici dello stato
- ??? **Sicurezza JWT** - Token-based authentication con refresh automatico
- ?? **Responsive design** - Interfaccia ottimizzata per desktop e mobile (MudBlazor)

---

## ??? Architettura del Sistema

```
???????????????????????????????????????????????????????
? Saipem e-PTW Architecture           ?
???????????????????????????????????????????????????????
?               ?
?  ???????????????????      ????????????????????    ?
?  ?  Blazor WASM    ????????  API Producer    ?    ?
?  ?   (Client)      ? JWT  ?   (.NET 9)       ?    ?
?  ?  Port: 7010     ?      ?  Port: 5101      ?    ?
?  ???????????????????  ???????????????????? ?
?         ?          ??
?    ?         ?         ?
?  ???????????????????      ????????????????????   ?
?  ?  LocalStorage   ?      ?  SQL Server      ?   ?
?  ?  (JWT Token)    ?  ?  (AppDbContext)  ?   ?
?  ???????????????????      ????????????????????   ?
?                  ?
?  ???????????????????????????????????????????????  ?
?  ?     Microsoft Entra ID (Azure AD)           ?  ?
?  ?     (Production Authentication)      ?  ?
?  ???????????????????????????????????????????????  ?
???????????????????????????????????????????????????????
```

### Progetti della Solution

| Progetto | Descrizione | Tecnologia |
|----------|-------------|------------|
| **SaipemE-PTW** | Applicazione Blazor WebAssembly (Frontend) | .NET 9, MudBlazor, MSAL |
| **SaipemE-PTW.Producer** | API RESTful (Backend) | .NET 9, Entity Framework Core, JWT |
| **SaipemE-PTW.Services** | Logica di business e servizi condivisi | .NET 9 |
| **SaipemE-PTW.Shared** | Modelli e DTO condivisi tra client e server | .NET 9 |

---

## ?? Sistema di Autenticazione

Il progetto implementa un **sistema di autenticazione duale** per facilitare lo sviluppo e la produzione:

### ?? Modalità Mock (Sviluppo)

Per lo sviluppo rapido senza configurazione Azure AD:

```csharp
// File: Authentication/Mock/MockAuthConfig.cs
public const bool UseMockAuthentication = true;  // ? Attiva Mock
public const bool EnableAutoLogin = false;       // Login automatico opzionale
```

**Caratteristiche Mock Mode:**
- ?? Login simulato con utenti predefiniti
- ?? Generazione JWT locale firmato con chiave simmetrica
- ?? Storage token via `ProtectedLocalStorage` (JS interop)
- ?? 12 utenti di test con ruoli diversi (vedi `MockUserRepository`)
- ?? Workflow completo funzionante senza backend reale

**Endpoint Mock Login:** `https://localhost:7010/mock-login`

### ?? Modalità Produzione (Microsoft Entra ID)

Per deployment in produzione con autenticazione enterprise:

```csharp
// File: Authentication/Mock/MockAuthConfig.cs
public const bool UseMockAuthentication = false;  // ? Disattiva Mock

// File: wwwroot/appsettings.json
"AzureAd": {
  "Authority": "https://login.microsoftonline.com/{TENANT_ID}",
  "ClientId": "{CLIENT_ID}",
  "ValidateAuthority": true
}
```

**Caratteristiche Production Mode:**
- ?? Autenticazione OIDC/OAuth2 via MSAL
- ?? Token JWT emesso da Microsoft Entra ID
- ?? Sincronizzazione ruoli da Azure AD Groups
- ?? Refresh token automatico
- ??? MFA e Conditional Access supportati

### ?? Ruoli Applicativi

| Codice | Ruolo | Descrizione |
|--------|-------|-------------|
| **RA** | Autorità Richiedente | Richiede permessi di lavoro |
| **PA** | Autorità Esecutrice | Esegue i lavori autorizzati |
| **IA** | Autorità Emittente | Emette i permessi di lavoro |
| **PTWC** | PTW Coordinator | Coordina il sistema dei permessi |
| **CSE** | Coordinatore In Esecuzione | Supervisiona l'esecuzione sul campo |
| **AGT** | Persona Autorizzata Test Gas | Esegue test atmosfere pericolose |
| **OA** | Autorità Operativa | Gestisce le operazioni |
| **EQ** | Esperto Qualificato | Consulenza tecnica specializzata |
| **ADMIN** | Amministratore Sistema | Gestisce configurazioni e utenti |
| **SUPER_OWNER** | Super Owner | Accesso completo al sistema |
| **Visitor** | Visitatore | Accesso in sola lettura |

### ?? Flusso di Autenticazione JWT

```
1. User ? Accede a /mock-login (o Azure AD)
2. System ? Genera JWT con claims (ruoli, nome, email, etc.)
3. System ? Salva token in LocalStorage (ProtectedLocalStorage)
4. User ? Naviga a pagina protetta
5. HttpClient ? Aggiunge automaticamente header: Authorization: Bearer {JWT}
6. API ? Valida JWT (JwtBearerHandler + AuthConstants)
7. API ? Autorizza richiesta se token valido e ruolo corretto
8. API ? Risponde con dati JSON
```

**Componenti chiave:**
- `MockOidcAuthService` / `MSAL` - Generazione token
- `ProtectedLocalStorageTokenStore` - Storage sicuro token
- `JwtAuthorizationMessageHandler` - Injection automatica Authorization header
- `AuthConstants.cs` - Parametri validazione JWT condivisi client/server
- `AppRoles.cs` - Definizione ruoli e descrizioni

---

## ?? Iniziare

### ?? Prerequisiti

Prima di iniziare, assicurati di avere installato:

- ? [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) (versione 9.0 o superiore)
- ? [Visual Studio 2022](https://visualstudio.microsoft.com/) (v17.8+) **oppure** [VS Code](https://code.visualstudio.com/) + [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
- ? [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (LocalDB, Express, o Developer Edition)
- ? [Node.js 18+](https://nodejs.org/) (opzionale, per tooling frontend)
- ? Browser moderno (Chrome, Edge, Firefox)

### ?? Installazione

#### 1?? Clona il Repository

```bash
git clone https://github.com/expriviapaolo/AppSaipemE-PTW.git
cd AppSaipemE-PTW
```

#### 2?? Configura il Database

```bash
cd SaipemE-PTW.Producer

# Aggiorna la connection string in appsettings.Development.json
# Default: "Server=localhost;Database=SaipemPWT;Trusted_Connection=True;TrustServerCertificate=True;"

# Applica le migrations
dotnet ef database update
```

#### 3?? Configura l'Autenticazione

**Opzione A: Mock Mode (Consigliato per sviluppo)**

```csharp
// File: SaipemE-PTW/Authentication/Mock/MockAuthConfig.cs
public const bool UseMockAuthentication = true;  // ? Lascia true
```

**Opzione B: Microsoft Entra ID (Produzione)**

```json
// File: SaipemE-PTW/wwwroot/appsettings.Development.json
{
  "AzureAd": {
    "Authority": "https://login.microsoftonline.com/{IL_TUO_TENANT_ID}",
    "ClientId": "{IL_TUO_CLIENT_ID}",
    "ValidateAuthority": true
  }
}
```

```csharp
// File: SaipemE-PTW/Authentication/Mock/MockAuthConfig.cs
public const bool UseMockAuthentication = false;  // ? Imposta false
```

#### 4?? Installa le Dipendenze

```bash
# Dalla root della solution
dotnet restore
```

---

## ?? Utilizzo

### ?? Avvio Rapido (Mock Mode)

#### Metodo 1: Visual Studio

1. Apri la solution `SaipemE-PTW.sln` in Visual Studio
2. Imposta **Multiple Startup Projects**:
   - Right-click sulla Solution ? **Properties** ? **Startup Project**
   - Seleziona **Multiple startup projects**
   - Imposta `SaipemE-PTW.Producer` ? **Start**
   - Imposta `SaipemE-PTW` ? **Start**
3. Premi `F5` per avviare in debug

#### Metodo 2: Command Line (2 terminali)

**Terminal 1 - API Backend:**
```bash
cd SaipemE-PTW.Producer
dotnet run
# API disponibile su https://localhost:5101
```

**Terminal 2 - Blazor Frontend:**
```bash
cd SaipemE-PTW
dotnet run
# App disponibile su https://localhost:7010
```

#### Metodo 3: Script PowerShell Automatico

```powershell
# File: start-dev.ps1 (crea questo script nella root)
Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd SaipemE-PTW.Producer; dotnet run"
Start-Sleep -Seconds 3
Start-Process pwsh -ArgumentList "-NoExit", "-Command", "cd SaipemE-PTW; dotnet run"
```

### ?? Primo Accesso (Mock Mode)

1. Apri browser su `https://localhost:7010`
2. Sarai reindirizzato automaticamente a `/mock-login`
3. Seleziona un utente dal menu a tendina (es. "Mario Rossi - Autorità Richiedente")
4. Clicca **"Accedi con Utente Selezionato"**
5. Verrai reindirizzato alla dashboard con autenticazione completa

**Utenti Mock Disponibili:**
- `mario.rossi@saipem.com` (RA - Autorità Richiedente)
- `luigi.bianchi@saipem.com` (PA - Autorità Esecutrice)
- `admin@saipem.com` (Tutti i ruoli - SUPER_OWNER)

### ?? Comandi Utili

```bash
# Build dell'intera solution
dotnet build

# Esegui i test (se presenti)
dotnet test

# Pulisci i file di build
dotnet clean

# Crea nuova migration (dal progetto Producer)
dotnet ef migrations add NomeMigration

# Applica migrations
dotnet ef database update

# Genera client HTTP (opzionale, per NSwag)
dotnet run --project SaipemE-PTW.Producer -- swagger

# Publish per produzione
dotnet publish -c Release -o ./publish
```

---

## ??? Funzionalità Principali

### 1. Dashboard Interattiva

```razor
@page "/"
@attribute [Authorize]

<MudGrid>
    <MudItem xs="12" sm="6" md="3">
        <MudCard>
         <MudCardContent>
    <MudText Typo="Typo.h6">Permessi Attivi</MudText>
     <MudText Typo="Typo.h3">@activePermitsCount</MudText>
          </MudCardContent>
        </MudCard>
  </MudItem>
</MudGrid>
```

### 2. Gestione Permessi di Lavoro

**Endpoint API:**
```csharp
GET    /api/permessi-lavoro       // Lista permessi
GET    /api/permessi-lavoro/{id}         // Dettaglio permesso
POST   /api/permessi-lavoro         // Crea nuovo permesso
PUT    /api/permessi-lavoro/{id}         // Aggiorna permesso
DELETE /api/permessi-lavoro/{id}         // Elimina permesso
```

**Utilizzo nel componente Blazor:**
```csharp
@inject IPermessoLavoroService PermessoService

private async Task LoadPermessiAsync()
{
  var permessi = await PermessoService.GetAllAsync();
    // ... logica UI
}
```

### 3. Sistema di Autorizzazione

**Protezione pagine per ruolo:**
```razor
@page "/admin/dashboard"
@attribute [Authorize(Roles = "ADMIN,SUPER_OWNER")]
```

**Controllo condizionale UI:**
```razor
<AuthorizeView Roles="RA,PA">
    <Authorized>
<MudButton Color="Color.Primary">Richiedi Permesso</MudButton>
    </Authorized>
    <NotAuthorized>
     <MudText>Non hai i permessi per questa azione</MudText>
    </NotAuthorized>
</AuthorizeView>
```

### 4. Upload e Gestione Allegati

```csharp
POST /api/attachment-types    // Crea tipo allegato
GET  /api/attachment-types  // Lista tipi (con cache)
GET  /api/attachment-types?lang=it   // Lista localizzata
```

**Caching automatico (Output Cache):**
```csharp
// TTL: 300 secondi (5 minuti)
// Cache key varia per lingua (query param 'lang')
```

---

## ?? Documentazione Aggiuntiva

### ?? File di Documentazione

- `ENTRA_ID_SETUP.md` - Configurazione Microsoft Entra ID
- `Authentication/Mock/README.md` - Guida sistema Mock Auth
- `Authentication/Mock/GUIDA_RAPIDA.md` - Quick start Mock Auth
- `FIX_HTTPCLIENT_QUICK.md` - Troubleshooting HttpClient
- `FIX_ITOKENSTORAGESERVICE_QUICK.md` - Configurazione token storage

### ?? Risorse Esterne

- [MudBlazor Documentation](https://mudblazor.com/)
- [Blazor WebAssembly Security](https://learn.microsoft.com/aspnet/core/blazor/security/webassembly)
- [Microsoft Entra ID Documentation](https://learn.microsoft.com/entra/identity/)
- [Entity Framework Core](https://learn.microsoft.com/ef/core/)

### ??? Roadmap

- [ ] Implementazione notifiche real-time (SignalR)
- [ ] Export PDF permessi di lavoro
- [ ] Firma digitale documenti
- [ ] Mobile app nativa (MAUI)
- [ ] Integrazione con sistemi esterni (SAP, ecc.)
- [ ] Dashboard analytics avanzate
- [ ] Supporto offline (PWA)

---

## ?? Contribuire

Questo è un progetto proprietario di Saipem. Le contribuzioni sono gestite internamente.

### Per Sviluppatori Interni

1. Crea un branch feature: `git checkout -b feature/nome-feature`
2. Commit delle modifiche: `git commit -m "Add: descrizione feature"`
3. Push del branch: `git push origin feature/nome-feature`
4. Apri una Pull Request verso `master`

### Linee Guida Codice

- Segui le convenzioni di naming C# (.NET)
- Usa commenti esplicativi con data e scopo (es. `// 2025-01-15 - Mock Auth - Descrizione`)
- Testa localmente con Mock Auth prima di committare
- Esegui `dotnet format` per formattazione automatica
- Scrivi test unitari per nuove funzionalità

### Eseguire i Test

```bash
# Esegui tutti i test
dotnet test

# Esegui con copertura codice
dotnet test /p:CollectCoverage=true
```

---

## ?? Licenza

**Proprietario:** Saipem S.p.A.  
**Tipo:** Software Proprietario - Tutti i diritti riservati

Questo software è proprietà esclusiva di Saipem S.p.A. Non è consentita la distribuzione, modifica o utilizzo al di fuori dell'organizzazione senza autorizzazione scritta esplicita.

---

## ?? Autori e Contatti

### Team di Sviluppo

**Exprivia S.p.A.** - Sviluppo e Maintenance  
**Cliente:** Saipem S.p.A.

### Supporto

Per supporto tecnico o domande:

- **Email:** support@exprivia.it
- **Issue Tracker:** [GitHub Issues](https://github.com/expriviapaolo/AppSaipemE-PTW/issues)
- **Documentazione Interna:** Confluence (link interno)

---

## ?? Ringraziamenti

- **MudBlazor Team** - Per l'eccellente libreria UI
- **Microsoft** - Per .NET, Blazor e Azure AD
- **Saipem** - Per la fiducia nel progetto
- **Entity Framework Core Team** - Per il potente ORM

---

## ?? Statistiche Progetto

- **Linguaggio:** C# 13.0
- **Framework:** .NET 9.0
- **Pattern:** Clean Architecture, Repository Pattern
- **UI Framework:** MudBlazor 8.14.0
- **Database:** SQL Server
- **Autenticazione:** JWT + MSAL
- **Logging:** Serilog

---

## ?? Troubleshooting

### Errore: 401 Unauthorized

**Problema:** API restituisce 401 su tutte le chiamate.

**Soluzione:**
1. Verifica di aver effettuato il login (`/mock-login` in dev mode)
2. Controlla che il token JWT sia salvato in LocalStorage (F12 ? Application ? Local Storage)
3. Verifica che `AuthConstants.cs` usi la stessa chiave di firma su client e server

### Errore: ERR_CONNECTION_REFUSED (localhost:5101)

**Problema:** Il frontend non riesce a connettersi all'API.

**Soluzione:**
1. Verifica che il progetto `SaipemE-PTW.Producer` sia in esecuzione
2. Controlla che `appsettings.Development.json` abbia `ApiBaseUrl: "https://localhost:5101/"`
3. Avvia l'API manualmente: `cd SaipemE-PTW.Producer && dotnet run`

### Errore: Database Migration Failed

**Problema:** Le migrations non si applicano.

**Soluzione:**
```bash
cd SaipemE-PTW.Producer
dotnet ef database drop  # ?? Cancella il database esistente
dotnet ef database update  # Ricrea da zero
```

### Errore: CORS Policy Block

**Problema:** Browser blocca chiamate API per CORS.

**Soluzione:**  
L'API ha già CORS configurato. Se persiste:
```csharp
// In SaipemE-PTW.Producer/Program.cs - verifica sia presente:
app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
```

---

<div align="center">

**Sviluppato con ?? da Exprivia per Saipem**

[![Exprivia](https://img.shields.io/badge/Exprivia-Digital%20Transformation-0066cc)](https://www.exprivia.it/)

</div>
