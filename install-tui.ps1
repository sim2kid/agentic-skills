# Bootstrap Script for Agentic Skills TUI Installer

$repoOwner = "sim2kid"
$repoName = "agentic-skills"

if ([string]::IsNullOrWhiteSpace($PSScriptRoot)) {
    $bootstrapUrl = "https://raw.githubusercontent.com/$repoOwner/$repoName/main/install-tui.ps1"
    $tempScript = Join-Path $env:TEMP "agentic-skills-install-tui-$(Get-Random).ps1"
    Invoke-WebRequest -Uri $bootstrapUrl -OutFile $tempScript -ErrorAction Stop
    & powershell -ExecutionPolicy Bypass -File $tempScript
    exit $LASTEXITCODE
}

$repoRoot = $PSScriptRoot
$projectPath = Join-Path $repoRoot "install-scripts\tui\AgenticSkillsInstaller\AgenticSkillsInstaller.csproj"

function Show-Welcome {
    Clear-Host
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "      Agentic Skills TUI Installer      " -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Target Repo: $repoOwner/$repoName" -ForegroundColor Gray
    Write-Host "Repo Root: $repoRoot" -ForegroundColor Gray
    Write-Host ""
}

Show-Welcome

if (-not (Test-Path -LiteralPath $projectPath)) {
    Write-Error "TUI installer project not found at: $projectPath"
    exit 1
}

try {
    Write-Host "Launching TUI installer..." -ForegroundColor Yellow
    dotnet run --project $projectPath
} catch {
    Write-Error "Failed to launch TUI installer: $($_.Exception.Message)"
    exit 1
}
