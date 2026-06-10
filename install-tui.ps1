# Bootstrap Script for Agentic Skills TUI Installer

$repoOwner = "sim2kid"
$repoName = "agentic-skills"

$scriptRoot = if ([string]::IsNullOrWhiteSpace($PSScriptRoot)) { Get-Location } else { $PSScriptRoot }
$repoRoot = if (Test-Path -LiteralPath (Join-Path $scriptRoot "packages.json")) { $scriptRoot } else {
    $cwd = Get-Location
    $check = Join-Path $cwd "packages.json"
    if (Test-Path -LiteralPath $check) { $cwd } else {
        while ($cwd) {
            $check = Join-Path $cwd "packages.json"
            if (Test-Path -LiteralPath $check) { break }
            $cwd = Split-Path $cwd -Parent
        }
        if (-not $cwd) { throw "Could not find repository root (packages.json not found)" }
        $cwd
    }
}
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
