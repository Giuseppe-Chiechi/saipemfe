# Script per completare la ristrutturazione del repository Git
# Il repository è già a livello di solution, dobbiamo solo aggiungere tutti i progetti

$solutionDir = "C:\Users\pclemente\OneDrive - Exprivia Spa\Saipem\e-PTW\web-app"
$logFile = Join-Path $solutionDir "git-restructure-log.txt"

Write-Host "Inizio ristrutturazione repository..." -ForegroundColor Cyan

Set-Location $solutionDir

# Verifica remote
Write-Host "Verifica remote..." -ForegroundColor Yellow
git remote -v | Out-File -FilePath $logFile -Encoding utf8
Get-Content $logFile

# Verifica stato corrente
Write-Host "`nVerifica stato corrente..." -ForegroundColor Yellow
git status | Out-File -FilePath $logFile -Append -Encoding utf8

# Aggiungi tutti i file
Write-Host "`nAggiungo tutti i file al repository..." -ForegroundColor Yellow
git add . 2>&1 | Out-File -FilePath $logFile -Append -Encoding utf8

# Verifica cosa verrà committato
Write-Host "`nVerifica file aggiunti..." -ForegroundColor Yellow
git status --short | Out-File -FilePath $logFile -Append -Encoding utf8

Write-Host "`nLog salvato in: $logFile" -ForegroundColor Green
Write-Host "`nPer completare:" -ForegroundColor Yellow
Write-Host "1. Verifica il file $logFile" -ForegroundColor White
Write-Host "2. Esegui: git commit -m 'Restructured repository to include entire solution'" -ForegroundColor White  
Write-Host "3. Esegui: git push origin master" -ForegroundColor White
