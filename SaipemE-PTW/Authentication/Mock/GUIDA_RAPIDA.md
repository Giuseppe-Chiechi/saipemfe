# ?? Guida Rapida - Autenticazione Mock e-PTW

## ? Avvio Rapido

### 1. Avvia l'applicazione
```bash
dotnet run
```

### 2. Vai alla pagina di login mock
Naviga a: `http://localhost:7035/mock-login`

### 3. Seleziona un utente
Scegli dalla lista (es. "Mario Rossi" per accesso completo)

### 4. Accedi
Clicca "Accedi con Utente Selezionato"

---

## ?? Utenti Consigliati per Test

### ?? Sviluppo Generale
**Mario Rossi** - `mario.rossi@saipem.com`
- Super Owner con accesso completo
- Ideale per sviluppo iniziale

### ?? Test Permessi di Lavoro
**Laura Bianchi** - `laura.bianchi@saipem.com`
- PTW Coordinator + Autorità Emittente
- Per testare coordinamento ed emissione

**Anna Ferrari** - `anna.ferrari@saipem.com`
- Autorità Richiedente
- Per testare richiesta permessi

**Marco Colombo** - `marco.colombo@saipem.com`
- Autorità Esecutrice
- Per testare esecuzione lavori

### ?? Test Multi-Ruolo
**Chiara Conti** - `chiara.conti@saipem.com`
- RA + PA + CSE
- Per testare scenari con più ruoli simultanei

### ??? Test Accesso Limitato
**Luca Galli** - `luca.galli@external.com`
- Visitatore
- Per testare restrizioni accesso

---

## ?? Commutazione Mock ?? Produzione

### Per DISABILITARE Mock (usare Entra ID reale)

Apri: `Authentication/Mock/MockAuthConfig.cs`

```csharp
public const bool UseMockAuthentication = false; // ? Cambia a false
```

### Per RIABILITARE Mock

```csharp
public const bool UseMockAuthentication = true; // ? Cambia a true
```

**Riavvia l'applicazione dopo ogni modifica.**

---

## ?? Pagine Utili

| URL | Descrizione |
|-----|-------------|
| `/mock-login` | Pagina di login mock |
| `/user-info` | Dettagli utente e claims |
| `/` | Home page |

---

## ?? Colori Applicazione (Mantenuti)

Il mock utilizza i colori già presenti nell'applicazione:
- **Primario**: Blu Bootstrap (`btn-primary`, `bg-primary`)
- **Successo**: Verde (`bg-success`, `text-success`)
- **Avviso**: Giallo (`bg-warning`, `text-warning`)
- **Pericolo**: Rosso (`bg-danger`, `text-danger`)
- **Info**: Azzurro (`bg-info`, `text-info`)
- **Secondario**: Grigio (`bg-secondary`)
- **Scuro**: Nero (`bg-dark`)

---

## ? Verifiche Post-Login

Dopo il login, verifica che:

1. ? Nome utente appare nel menu in alto a destra
2. ? Badge giallo "MOCK" è visibile
3. ? Menu "Info Utente" è accessibile
4. ? Pagina `/user-info` mostra tutti i claims
5. ? I ruoli corretti sono assegnati

---

## ?? Problemi Comuni

### Il login non funziona
?? Verifica che `UseMockAuthentication = true`
?? Riavvia l'applicazione

### Non vedo il menu utente
?? Verifica di aver effettuato il login
?? Controlla la console browser per errori

### Errore "AuthenticationService.init"
?? Verifica che `index.html` contenga:
```html
<script src="_content/Microsoft.Authentication.WebAssembly.Msal/AuthenticationService.js"></script>
```

---

## ?? Sicurezza

### ?? ATTENZIONE

- **NON** usare mock in produzione
- **SEMPRE** impostare `UseMockAuthentication = false` prima del deploy
- Proteggere `/mock-login` in produzione

---

## ?? Supporto

Per problemi o domande:
1. Controlla il README completo in `Authentication/Mock/README.md`
2. Verifica la console browser (F12)
3. Controlla i log dell'applicazione

---

**Ultimo aggiornamento**: 15 Gennaio 2025
