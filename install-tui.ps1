# Bootstrap Script for Agentic Skills TUI Installer

$repoOwner = "sim2kid"
$repoName = "agentic-skills"
$projectPath = Join-Path $PSScriptRoot "install-scripts\tui\AgenticSkillsInstaller\AgenticSkillsInstaller.csproj"

function Show-Welcome {
    Clear-Host
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "      Agentic Skills TUI Installer      " -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Target Repo: $repoOwner/$repoName" -ForegroundColor Gray
    Write-Host "Project: $projectPath" -ForegroundColor Gray
    Write-Host ""
}

Show-Welcome

if (-not (Test-Path -LiteralPath $projectPath)) {
    Write-Error "TUI installer project not found at $projectPath"
    exit 1
}

try {
    Write-Host "Launching TUI installer with dotnet..." -ForegroundColor Yellow
    dotnet run --project $projectPath
} catch {
    Write-Error "Failed to launch TUI installer: $($_.Exception.Message)"
    exit 1
}
