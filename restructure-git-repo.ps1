# Script per ristrutturare il repository Git a livello di solution
# Questo script sposta la cartella .git dalla sottocartella del progetto alla directory della solution

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Ristrutturazione Repository Git" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$projectDir = "C:\Users\pclemente\OneDrive - Exprivia Spa\Saipem\e-PTW\web-app\SaipemE-PTW"
$solutionDir = "C:\Users\pclemente\OneDrive - Exprivia Spa\Saipem\e-PTW\web-app"
$gitSourcePath = Join-Path $projectDir ".git"
$gitDestPath = Join-Path $solutionDir ".git"

# Step 1: Verifica che la cartella .git esista nel progetto
Write-Host "Step 1: Verifica esistenza repository Git..." -ForegroundColor Yellow
if (Test-Path $gitSourcePath) {
    Write-Host "  [OK] Repository Git trovato in: $projectDir" -ForegroundColor Green
} else {
  Write-Host "  [ERRORE] Repository Git non trovato in: $projectDir" -ForegroundColor Red
    Write-Host "  Verifica manualmente la posizione del repository." -ForegroundColor Red
    exit 1
}

# Step 2: Verifica che non esista già un repository nella solution
Write-Host ""
Write-Host "Step 2: Verifica che la directory solution non abbia già un repository..." -ForegroundColor Yellow
if (Test-Path $gitDestPath) {
    Write-Host "  [ATTENZIONE] Esiste già una cartella .git nella directory solution!" -ForegroundColor Red
    Write-Host "  Per sicurezza, lo script si ferma qui." -ForegroundColor Red
    Write-Host "  Rimuovi manualmente la cartella .git dalla solution se necessario." -ForegroundColor Red
    exit 1
} else {
    Write-Host "  [OK] Nessun repository esistente nella solution." -ForegroundColor Green
}

# Step 3: Sposta la cartella .git
Write-Host ""
Write-Host "Step 3: Spostamento cartella .git..." -ForegroundColor Yellow
try {
    Move-Item -Path $gitSourcePath -Destination $gitDestPath -Force
    Write-Host "  [OK] Cartella .git spostata con successo!" -ForegroundColor Green
} catch {
    Write-Host "  [ERRORE] Impossibile spostare la cartella .git: $_" -ForegroundColor Red
    exit 1
}

# Step 4: Verifica il risultato
Write-Host ""
Write-Host "Step 4: Verifica finale..." -ForegroundColor Yellow
if (Test-Path $gitDestPath) {
    Write-Host "  [OK] Repository Git ora si trova in: $solutionDir" -ForegroundColor Green
} else {
    Write-Host "  [ERRORE] Qualcosa è andato storto durante lo spostamento." -ForegroundColor Red
    exit 1
}

if (Test-Path $gitSourcePath) {
    Write-Host "  [ATTENZIONE] La cartella .git esiste ancora nel progetto!" -ForegroundColor Red
} else {
    Write-Host "  [OK] Cartella .git rimossa dal progetto." -ForegroundColor Green
}

# Step 5: Cambia directory e verifica Git
Write-Host ""
Write-Host "Step 5: Verifica configurazione Git..." -ForegroundColor Yellow
Set-Location $solutionDir

Write-Host ""
Write-Host "Verifico il remote configurato:" -ForegroundColor Cyan
git remote -v

Write-Host ""
Write-Host "Verifico lo stato del repository:" -ForegroundColor Cyan
git status

Write-Host ""
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Ristrutturazione completata!" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "PROSSIMI PASSI:" -ForegroundColor Yellow
Write-Host "1. Esegui: git add ." -ForegroundColor White
Write-Host "2. Esegui: git commit -m 'Restructured repository to include entire solution'" -ForegroundColor White
Write-Host "3. Esegui: git push -f origin master" -ForegroundColor White
Write-Host ""
Write-Host "ATTENZIONE: Il push forzato sovrascriverà la cronologia su GitHub!" -ForegroundColor Red
Write-Host ""
