# Bootstrap Script for Agentic Skills TUI Installer

$repoOwner = "sim2kid"
$repoName = "agentic-skills"
$branch = "main"
$tempRoot = Join-Path $env:TEMP ("agentic-skills-installer-" + [guid]::NewGuid().ToString("N"))
$zipPath = Join-Path $tempRoot "source.zip"
$extractRoot = Join-Path $tempRoot "source"

function Show-Welcome {
    Clear-Host
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "      Agentic Skills TUI Installer      " -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "Target Repo: $repoOwner/$repoName" -ForegroundColor Gray
    Write-Host "Branch: $branch" -ForegroundColor Gray
    Write-Host ""
}

function Get-SourceRoot {
    $directories = Get-ChildItem -LiteralPath $extractRoot -Directory
    if (-not $directories -or $directories.Count -eq 0) {
        throw "Failed to locate extracted repository contents."
    }

    return $directories[0].FullName
}

Show-Welcome

try {
    New-Item -ItemType Directory -Path $tempRoot -Force | Out-Null
    New-Item -ItemType Directory -Path $extractRoot -Force | Out-Null

    $zipUrl = "https://github.com/$repoOwner/$repoName/archive/refs/heads/$branch.zip"
    Write-Host "Downloading repository archive..." -ForegroundColor Yellow
    Invoke-WebRequest -Uri $zipUrl -OutFile $zipPath -ErrorAction Stop

    Write-Host "Extracting repository archive..." -ForegroundColor Yellow
    Expand-Archive -LiteralPath $zipPath -DestinationPath $extractRoot -Force

    $sourceRoot = Get-SourceRoot
    $projectPath = Join-Path $sourceRoot "install-scripts\tui\AgenticSkillsInstaller\AgenticSkillsInstaller.csproj"

    Write-Host "Source Root: $sourceRoot" -ForegroundColor DarkGray
    Write-Host "Project Path: $projectPath" -ForegroundColor DarkGray

    if (-not (Test-Path -LiteralPath $projectPath)) {
        throw "TUI installer project not found at: $projectPath"
    }

    Write-Host "Launching TUI installer..." -ForegroundColor Yellow
    dotnet run --project $projectPath
}
catch {
    Write-Error $_
    exit 1
}
finally {
    if (Test-Path -LiteralPath $tempRoot) {
        Remove-Item -LiteralPath $tempRoot -Recurse -Force -ErrorAction SilentlyContinue
    }
}
