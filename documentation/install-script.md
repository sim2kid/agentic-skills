# Install Script Design Requirements

The installation script is a robust, single-file utility designed to deploy skills and agents from this repository into a target repository. It supports versioning via GitHub releases and stateful package management.

## 1. User Experience
The script is designed for single-command execution on Windows:
```powershell
powershell -ExecutionPolicy Bypass -Command "iwr -useb https://raw.githubusercontent.com/OWNER/REPO/main/install.ps1 | iex"
```

## 2. State Management (`packages-installed.json`)
The script maintains a state file in the target repository root to support clean updates and removals.
- **Fields**: `version` (installed version/tag), `agentType` (e.g., OpenCode), `packages` (array of installed package IDs), and `timestamp`.
- **Purpose**: Tracks installed artifacts to ensure idempotent operations and clean uninstalls.

## 3. Execution Workflow

### Phase 1: Configuration & Versioning
- **Agent Type Selection**: Default support for `OpenCode`, with future-proofing for other agent ecosystems.
- **Version Selection**: Fetches release tags from the GitHub API, allowing users to choose `latest` or a specific version.
- **Metadata Retrieval**: Downloads `packages.json` from the selected version to identify available packages.

### Phase 2: Package Selection
- **State Awareness**: If `packages-installed.json` exists, previous packages are auto-selected. If not, all available packages are selected by default.
- **Interactive Confirmation**: Users can review and modify the package selection via a CLI menu.

### Phase 3: Deployment
- **Cleanup**: Removes all files/directories listed in the previous state file before installing new ones.
- **Download & Extraction**: Downloads the versioned ZIP archive and extracts only the selected packages.
- **File Placement (OpenCode Standards)**:
    - **Skills**: Each skill folder under `packages/<package-name>/skills/` is deployed to `.opencode/skills/<skill-name>/`.
    - **Subagents**: Each `AGENT.md` file under `packages/<package-name>/sub-agents/<agent-name>/` is renamed and deployed to `.opencode/agents/<agent-name>.md`.
- **Finalization**: Updates or creates the `packages-installed.json` state file.

## 4. Error Handling & Validation
- **Schema Validation**: Validates the structure of `packages.json` before execution.
- **Environment Checks**: Ensures target directories exist or are created with appropriate permissions.
- **API Resilience**: Handles GitHub API rate limits and network failures with clear user feedback.

## 5. Success Criteria
- **Atomic Deployment**: Prevents "ghost" files by ensuring a clean state before installation.
- **Automatic Selection**: Remembers user preferences from previous installations.
- **Path Compliance**: Correctly maps repository packages to the specific folder structures required by agent platforms (e.g., OpenCode's `.opencode/` directory).
