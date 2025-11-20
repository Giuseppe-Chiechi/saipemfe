# ?? ISTRUZIONI IMMEDIATE - Inizia Subito!

## ? Avvio in 3 Passi

### 1?? Avvia l'Applicazione
```bash
dotnet run
```
Oppure premi **F5** in Visual Studio

### 2?? Apri il Browser
L'applicazione si aprirà automaticamente a:
```
http://localhost:7035
```
(o porta indicata nella console)

### 3?? Fai Login
1. Clicca su **"Accedi (Mock)"** nel menu in alto a destra
2. Seleziona **"Mario Rossi (SuperOwner, SystemAdmin)"**
3. Clicca **"Accedi con Utente Selezionato"**

**? FATTO! Ora sei loggato come Super Owner**

---

## ?? Cosa Puoi Fare Ora

### Dopo il Login Vedrai:
- ? Nome utente in alto a destra
- ? Badge giallo "MOCK"
- ? Menu "Info Utente" disponibile
- ? Menu "Dashboard Admin" disponibile (solo per admin)

### Pagine da Esplorare:

#### ?? **Dashboard Admin**
```
http://localhost:7035/admin/dashboard
```
- Visualizza statistiche
- Vedi lista utenti mock
- Solo per SuperOwner e SystemAdmin

#### ?? **Info Utente**
```
http://localhost:7035/user-info
```
- Vedi tutti i tuoi claims
- Verifica i tuoi ruoli
- Testa le autorizzazioni

#### ?? **Home**
```
http://localhost:7035/
```
- Pagina principale
- Mostra stato autenticazione

---

## ?? Test Rapidi

### Test 1: Login con Diversi Ruoli
1. **Logout**: Clicca sul tuo nome ? "Disconnetti"
2. **Login**: Clicca "Accedi (Mock)"
3. **Prova**:
   - Laura Bianchi (PTW Coordinator)
   - Anna Ferrari (Autorità Richiedente)
   - Luca Galli (Visitatore - accesso limitato)

### Test 2: Verifica Autorizzazioni
1. **Login come Luca Galli** (Visitatore)
2. **Prova accedere a**: `/admin/dashboard`
3. **Risultato Atteso**: "Non sei autorizzato"

### Test 3: Visualizza Claims
1. **Login con qualsiasi utente**
2. **Vai a**: `/user-info`
3. **Verifica**: Tutti i claims sono presenti

---

## ?? Menu Utente (dopo login)

Clicca sul tuo nome in alto a destra per vedere:
- ? Nome completo
- ? Email
- ? Lista dei tuoi ruoli
- ? Pulsante "Disconnetti"

---

## ?? Modificare Configurazione

### Per Testare con Altro Utente di Default

**File**: `Authentication/Mock/MockAuthConfig.cs`

```csharp
// Riga 14-15
public const bool EnableAutoLogin = true;  // ? Cambia a true
public const string DefaultMockUserEmail = "laura.bianchi@saipem.com"; // ? Cambia email
```

**Salva** e **riavvia** l'app. Login automatico!

---

## ?? Lista Utenti Rapida

| Nome | Email | Ruolo Principale | Quando Usare |
|------|-------|------------------|--------------|
| Mario Rossi | mario.rossi@saipem.com | Super Owner | Tutto (test completo) |
| Laura Bianchi | laura.bianchi@saipem.com | PTW Coordinator | Coordinamento |
| Anna Ferrari | anna.ferrari@saipem.com | Autorità Richiedente | Richieste |
| Marco Colombo | marco.colombo@saipem.com | Autorità Esecutrice | Esecuzione |
| Luca Galli | luca.galli@external.com | Visitatore | Accesso limitato |
| Chiara Conti | chiara.conti@saipem.com | Multi-ruolo | Test complessi |

**Lista completa**: Vedi `Authentication/Mock/GUIDA_RAPIDA.md`

---

## ?? Problema? Risolvi in 30 Secondi

### Errore "AuthenticationService.init"
**Soluzione**: Il problema è già risolto! Se appare:
1. Verifica `wwwroot/index.html` contenga:
```html
<script src="_content/Microsoft.Authentication.WebAssembly.Msal/AuthenticationService.js"></script>
```
2. Riavvia l'applicazione

### Non Vedo il Menu Login
**Soluzione**: 
1. Verifica `Authentication/Mock/MockAuthConfig.cs`
2. Controlla che `UseMockAuthentication = true`
3. Riavvia l'applicazione

### Il Login Non Funziona
**Soluzione**:
1. Apri Console Browser (F12)
2. Cerca errori in rosso
3. Verifica che l'email dell'utente esista in `MockUserRepository.cs`

---

## ?? Documentazione Completa

### Per Maggiori Dettagli:
- **Guida Completa**: `Authentication/Mock/README.md`
- **Guida Rapida**: `Authentication/Mock/GUIDA_RAPIDA.md`
- **Riepilogo Tecnico**: `Authentication/Mock/RIEPILOGO_IMPLEMENTAZIONE.md`

---

## ?? Screenshot Percorso Tipico

### 1. Home (non loggato)
```
[Header: SaipemE-PTW]  [Pulsante: Accedi (Mock)]
     
[Messaggio: Benvenuto! Per accedere...]
```

### 2. Pagina Login
```
[Dropdown: Seleziona Utente]
? Seleziona "Mario Rossi"
[Card: Mostra dettagli Mario Rossi]
[Pulsante: Accedi con Utente Selezionato]
```

### 3. Home (loggato)
```
[Header: SaipemE-PTW]  [Mario Rossi ?] [Badge: MOCK]
     
Menu:
- Home
- Info Utente
- Dashboard Admin

[Messaggio: Benvenuto Mario Rossi!]
```

---

## ? Checklist Prima di Sviluppare

Verifica di aver fatto tutto:

- [ ] ? Applicazione avviata
- [ ] ? Login effettuato con Mario Rossi
- [ ] ? Nome utente visibile in alto
- [ ] ? Badge "MOCK" presente
- [ ] ? Visitato `/user-info`
- [ ] ? Visitato `/admin/dashboard`
- [ ] ? Provato logout/login con altro utente

**Se hai tutti i ? sei pronto per sviluppare!**

---

## ?? Prossimo Step: Sviluppo Features

Ora che l'autenticazione funziona, puoi:

### Proteggere Nuove Pagine
```csharp
@page "/mia-pagina"
@attribute [Authorize(Roles = "RA,PA")]  // Solo RA e PA
```

### Mostrare Contenuto Condizionale
```razor
<AuthorizeView Roles="SuperOwner">
    <Authorized>
        <button>Solo per Super Owner</button>
    </Authorized>
</AuthorizeView>
```

### Verificare Ruolo nel Codice
```csharp
@inject MockAuthenticationStateProvider AuthStateProvider

if (AuthStateProvider.IsInRole(AppRoles.SuperOwner))
{
    // Logica per Super Owner
}
```

---

## ?? Tips Utili

### Velocizzare il Test
Abilita auto-login in `MockAuthConfig.cs` per login automatico all'avvio

### Debug Claims
Vai sempre su `/user-info` per vedere tutti i claims dell'utente corrente

### Testare Autorizzazioni
Usa Chiara Conti (multi-ruolo) per testare scenari complessi

---

## ?? Hai Bisogno di Aiuto?

1. **Console Browser (F12)**: Cerca errori JavaScript
2. **Output Visual Studio**: Cerca errori C#
3. **Documentazione**: Leggi i file in `Authentication/Mock/`
4. **Test Passo-Passo**: Segui i test in questo file

---

## ?? Congratulazioni!

Hai ora un sistema di autenticazione mock completamente funzionante!

**Ricorda**: Quando vai in produzione, imposta `UseMockAuthentication = false` in `MockAuthConfig.cs`

---

**Buon Sviluppo! ??**

---

*Ultimo aggiornamento: 15 Gennaio 2025*
*Versione Mock Auth: 1.0*
