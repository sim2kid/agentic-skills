# Install Script for Agentic Skills
# This script allows users to install specific skills and agents into their repository.

$repoOwner = "anomalyco"
$repoName = "agentic-skills"
$stateFile = ".agents/agent-packages-installed.json"

function Get-State {
    if (Test-Path -LiteralPath $stateFile) {
        return Get-Content -Raw -LiteralPath $stateFile | ConvertFrom-Json
    }
    return $null
}

function Save-State ($state) {
    $stateDir = Split-Path $stateFile -Parent
    if (-not (Test-Path -LiteralPath $stateDir)) {
        New-Item -ItemType Directory -Path $stateDir -Force | Out-Null
    }
    $state | ConvertTo-Json | Set-Content -LiteralPath $stateFile
}

function Show-Welcome {
    Clear-Host
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host "   Agentic Skills Installation Script   " -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host ""
}

function Select-AgentType {
    $types = @("OpenCode")
    Write-Host "Select Agent Type:" -ForegroundColor Yellow
    for ($i = 0; $i -lt $types.Count; $i++) {
        Write-Host "[$i] $($types[$i])"
    }
    $choice = Read-Host "Enter choice (default 0)"
    if ([string]::IsNullOrWhiteSpace($choice)) { $choice = 0 }
    return $types[[int]$choice]
}

function Select-Version {
    $apiUrl = "https://api.github.com/repos/$repoOwner/$repoName/tags"
    try {
        $tags = Invoke-RestMethod -Uri $apiUrl
        $versions = @("latest")
        if ($tags -and $tags.Count -gt 0) {
            foreach ($tag in $tags) {
                $versions += $tag.name
            }
        }

        Write-Host "`nSelect Version:" -ForegroundColor Yellow
        for ($i = 0; $i -lt $versions.Count; $i++) {
            Write-Host "[$i] $($versions[$i])"
        }
        $choice = Read-Host "Enter choice (default 0)"
        if ([string]::IsNullOrWhiteSpace($choice)) { $choice = 0 }
        return $versions[[int]$choice]
    } catch {
        Write-Warning "Could not fetch tags from GitHub API, defaulting to 'latest'. Error: $($_.Exception.Message)"
        return "latest"
    }
}

function Get-PackageMetadata ($version) {
    if ($env:LOCAL_TEST_MODE -eq "true") {
        return Get-Content -Raw -LiteralPath "packages.json" | ConvertFrom-Json
    }

    $branch = if ($version -eq "latest") { "main" } else { $version }
    $url = "https://raw.githubusercontent.com/$repoOwner/$repoName/$branch/packages.json"
    try {
        $response = Invoke-WebRequest -Uri $url -ErrorAction Stop
        return $response.Content | ConvertFrom-Json
    } catch {
        Write-Error "Failed to fetch packages.json: $($_.Exception.Message)"
        exit 1
    }
}

function Select-Packages ($metadata) {
    $state = Get-State
    $allPackages = $metadata.packages
    $selected = @()

    if ($state -and $state.packages) {
        Write-Host "`nExisting installation found. Pre-selecting previously installed packages." -ForegroundColor Gray
        $selected = $state.packages
    } else {
        Write-Host "`nNo previous installation found. Selecting all available packages." -ForegroundColor Gray
        $selected = $allPackages | ForEach-Object { $_.id }
    }

    Write-Host "`nAvailable Packages:" -ForegroundColor Yellow
    $finalSelection = @()
    for ($i = 0; $i -lt $allPackages.Count; $i++) {
        $pkg = $allPackages[$i]
        $isSelected = $selected -contains $pkg.id
        $mark = if ($isSelected) { "[X]" } else { "[ ]" }
        Write-Host "$mark $($pkg.id) - $($pkg.description)"
    }

    Write-Host "`nPress Enter to confirm selection, or type package IDs to modify (comma separated)." -ForegroundColor Gray
    $input = Read-Host "Selection"
    
    if (-not [string]::IsNullOrWhiteSpace($input)) {
        return $input.Split(',').Trim()
    }
    return $selected
}

function Cleanup-Existing {
    $state = Get-State
    if (-not $state) { return }

    Write-Host "`nCleaning up existing installation..." -ForegroundColor Yellow
    # For OpenCode, we clean .opencode/skills and .opencode/agents
    if (Test-Path -LiteralPath ".opencode") {
        # This is a simplified cleanup. In a real scenario, we'd use the state file 
        # to precisely remove only what was installed by this script.
        # For now, we ensure the directories exist.
    }
}

function Deploy-Packages ($version, $selectedPackages, $metadata) {
    $branch = if ($version -eq "latest") { "main" } else { $version }
    $tempDir = Join-Path $env:TEMP "agentic-skills-install-$(Get-Random)"
    New-Item -ItemType Directory -Path $tempDir -Force | Out-Null

    if ($env:LOCAL_TEST_MODE -eq "true") {
        Write-Host "`nUsing local files for installation (TEST MODE)" -ForegroundColor Gray
        $sourceRoot = Get-Location
    } else {
        Write-Host "`nDownloading archive..." -ForegroundColor Yellow
        $zipUrl = "https://github.com/$repoOwner/$repoName/archive/$branch.zip"
        $zipPath = Join-Path $tempDir "source.zip"
        Invoke-WebRequest -Uri $zipUrl -OutFile $zipPath

        Write-Host "Extracting..." -ForegroundColor Yellow
        Expand-Archive -Path $zipPath -DestinationPath $tempDir -Force
        
        $extractDir = Get-ChildItem -Path $tempDir | Where-Object { $_.PSIsContainer } | Select-Object -First 1
        $sourceRoot = $extractDir.FullName
    }

    foreach ($pkgId in $selectedPackages) {
        Write-Host "Installing package $($pkgId)..." -ForegroundColor Cyan
        $pkgDir = Join-Path $sourceRoot "packages/$pkgId"
        if (-not (Test-Path -LiteralPath $pkgDir)) {
            Write-Warning "Package directory $pkgDir not found. Skipping."
            continue
        }

        # Install Skills from the package
        $skillsSrc = Join-Path $pkgDir "skills"
        if (Test-Path -LiteralPath $skillsSrc) {
            $skillsDestBase = ".opencode/skills"
            New-Item -ItemType Directory -Path $skillsDestBase -Force | Out-Null
            $installedSkills = Get-ChildItem -Path $skillsSrc -Directory
            foreach ($skill in $installedSkills) {
                $skillDest = Join-Path $skillsDestBase $skill.Name
                New-Item -ItemType Directory -Path $skillDest -Force | Out-Null
                Copy-Item -Path "$($skill.FullName)\*" -Destination $skillDest -Recurse -Force
                Write-Host "  -> Installed skill: $($skill.Name)" -ForegroundColor Gray
            }
        }

        # Install Sub-Agents from the package
        $agentsSrc = Join-Path $pkgDir "sub-agents"
        if (Test-Path -LiteralPath $agentsSrc) {
            $agentsDestBase = ".opencode/agents"
            New-Item -ItemType Directory -Path $agentsDestBase -Force | Out-Null
            $installedAgents = Get-ChildItem -Path $agentsSrc -Directory
            foreach ($agent in $installedAgents) {
                $agentDest = Join-Path $agentsDestBase "$($agent.Name).md"
                # The requirement says sub-agents folder contains agent name folder and then AGENT.md
                $agentFile = Join-Path $agent.FullName "AGENT.md"
                if (Test-Path -LiteralPath $agentFile) {
                    Copy-Item -Path $agentFile -Destination $agentDest -Force
                    Write-Host "  -> Installed agent: $($agent.Name)" -ForegroundColor Gray
                }
            }
        }
    }

    if ($env:LOCAL_TEST_MODE -ne "true") {
        Remove-Item -Path $tempDir -Recurse -Force
    }
}

# Main Execution
Show-Welcome
$state = Get-State
$currentVersion = if ($state) { $state.version } else { "None" }
Write-Host "Current Installed Version: $currentVersion" -ForegroundColor Gray

Write-Host "`nWhat would you like to do?" -ForegroundColor Yellow
    $actionLabel = if ($currentVersion -eq "None") { "Install" } else { "Install / Update" }
    Write-Host "[0] $actionLabel"
Write-Host "[1] Uninstall"
$action = Read-Host "Enter choice (default 0)"
if ([string]::IsNullOrWhiteSpace($action)) { $action = 0 }

if ($action -eq 1) {
    Write-Host "`nUninstalling all installed packages..." -ForegroundColor Yellow
    if ($state -and $state.packages) {
        if (Test-Path -LiteralPath ".opencode") {
            Remove-Item -Recurse -Force ".opencode"
        }
        Remove-Item -LiteralPath $stateFile -Force -ErrorAction SilentlyContinue
        Write-Host "Successfully uninstalled all packages." -ForegroundColor Green
    } else {
        Write-Host "No packages were installed." -ForegroundColor Gray
    }
    exit
}

$agentType = Select-AgentType
$version = Select-Version
$metadata = Get-PackageMetadata $version
$selected = Select-Packages $metadata

Cleanup-Existing
Deploy-Packages $version $selected $metadata

$newState = @{
    version = $version
    agentType = $agentType
    packages = $selected
    timestamp = (Get-Date).ToString("yyyy-MM-dd HH:mm:ss")
}
Save-State $newState

Write-Host "`nInstallation complete!" -ForegroundColor Green
