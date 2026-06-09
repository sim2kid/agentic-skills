# Implementation Plan: Install Script

## Goal
Create a robust, single-file installation script that allows users to select specific skills/agents from this repository and install them into another repository, with support for versioning via GitHub releases and stateful package management.

## User Experience: Single Command Install
The envisioned flow is a single line for Windows users to download and execute the script directly in their target repository:
```powershell
powershell -ExecutionPolicy Bypass -Command "iwr -useb https://raw.githubusercontent.com/OWNER/REPO/main/install.ps1 | iex"
```

## Detailed Design

### 1. State Management (`packages-installed.json`)
To support updates and clean removals, the script will maintain a state file in the target repository root named `packages-installed.json`.
- **Fields**: `version` (installed version), `agentType`, `packages` (array of installed package IDs), `timestamp`.
- **Purpose**: Tracks what was installed so it can be cleanly removed or updated in the next run.

### 2. Execution Workflow

#### Phase 1: Configuration & Versioning
1.  **Agent Type Selection**:
    - Currently supports: `OpenCode`.
    - Future-proofed to allow other types (e.g., `Claude Desktop`, `AutoGPT`).
2.  **Version Selection**:
    - Fetch all release tags from the GitHub API.
    - Provide options for `latest` (main branch) or specific tagged releases.
3.  **Fetch Metadata**:
    - Download `packages.json` from the selected version to identify available packages and their metadata.

#### Phase 2: Package Selection
1.  **Read Local State**: Check for an existing `packages-installed.json`.
2.  **Selection Logic**:
    - If found: Auto-select packages listed in the state file.
    - If NOT found: Auto-select ALL packages from the fetched `packages.json`.
3.  **Interactive Menu**: Allow the user to confirm or modify the selection before proceeding.

#### Phase 3: Deployment
1.  **Cleanup**:
    - If a previous install exists, use the local state file to remove all previously installed files/directories.
2.  **Download & Extract**:
    - Download the selected version (ZIP archive) from GitHub.
    - Extract selected packages to a temporary location.
3.  **File Placement (Agent-Type Specific)**:
    - For **OpenCode**:
        - **Skills**: Each skill folder under `packages/<package-name>/skills/` is deployed to `.opencode/skills/<skill-name>/`.
        - **Subagents**: Each `AGENT.md` file under `packages/<package-name>/sub-agents/<agent-name>/` is renamed and deployed to `.opencode/agents/<agent-name>.md`. (Including any associated config files from the agent directory).
4.  **Finalize State**:
    - Update/Create `packages-installed.json` with the new version and the list of currently installed packages.

## Implementation Details

### Technology Stack
- **Scripting**: PowerShell (Primary for Windows).
- **GitHub Integration**: `Invoke-RestMethod` for API calls (tags, releases) and file downloads.
- **UI**: Interactive CLI with `Write-Host` and user input loops.

### Directory Structure Requirements (OpenCode)
As per OpenCode documentation:
- **Skills**: Must be in `.opencode/skills/<name>/` with a `SKILL.md` file. The folder name must match the skill name.
- **Subagents**: Typically defined as `.md` files in `.opencode/agents/` with specific frontmatter.

### Error Handling & Validation
- Ensure target directories exist or are created.
- Validate `packages.json` structure before processing.
- Provide clear error messages if GitHub API rate limits are hit or network is unavailable.

## Success Criteria
- [ ] Successfully downloads and runs via a single PowerShell command.
- [ ] Correctly identifies and auto-selects previously installed packages.
- [ ] Removes old files before installing new ones to prevent "ghost" files.
- [ ] Successfully deploys skills and subagents into the correct `.opencode/` structure (maintaining individual skill folders and renaming `AGENT.md` files).
- [ ] Correctly updates the `packages-installed.json` file.
