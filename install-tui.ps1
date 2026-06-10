# Bootstrap Script for Agentic Skills TUI Installer

$repoOwner = "sim2kid"
$repoName = "agentic-skills"

function Get-RepoRoot {
    $searchRoot = if ([string]::IsNullOrWhiteSpace($PSScriptRoot)) {
        (Get-Location).Path
    } else {
        $PSScriptRoot
    }

    $checkPath = Join-Path $searchRoot "packages.json"
    if (Test-Path -LiteralPath $checkPath) {
        return $searchRoot
    }

    $parent = Split-Path $searchRoot -Parent
    while (-not [string]::IsNullOrWhiteSpace($parent)) {
        $checkPath = Join-Path $parent "packages.json"
        if (Test-Path -LiteralPath $checkPath) {
            return $parent
        }
        $parent = Split-Path $parent -Parent
    }

    return $null
}

$repoRoot = Get-RepoRoot

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

if ([string]::IsNullOrWhiteSpace($repoRoot)) {
    Write-Error "Could not find repository root (packages.json not found in any parent directory)."
    exit 1
}

$projectPath = Join-Path $repoRoot "install-scripts\tui\AgenticSkillsInstaller\AgenticSkillsInstaller.csproj"

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