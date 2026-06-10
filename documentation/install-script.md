# Install Script Design Requirements

The Windows installer is a TUI-driven bootstrap flow that downloads the repository archive, launches the installer from the extracted source, and deploys skills and agents from this repository into a target repository. It supports versioning via GitHub tags and stateful package management.

## 1. User Experience
The primary Windows installer is the TUI bootstrap script and is designed for single-command execution:
```powershell
powershell -ExecutionPolicy Bypass -Command "$t = Join-Path $env:TEMP (\"agentic-skills-install-tui-$([guid]::NewGuid().ToString('N')).ps1\"); Invoke-WebRequest \"https://raw.githubusercontent.com/sim2kid/agentic-skills/main/install-tui.ps1\" -OutFile $t; & powershell -ExecutionPolicy Bypass -File $t"
```

The TUI uses a single centered window with a persistent Info section and step-based content.

## 2. State Management (`packages-installed.json`)
The script maintains a state file in the target repository root to support clean updates and removals.
- **Fields**: `version` (installed version/tag), `agentType` (e.g., OpenCode), `packages` (array of installed package IDs), and `timestamp`.
- **Purpose**: Tracks installed artifacts to ensure idempotent operations and clean uninstalls.

## 3. Execution Workflow

### Phase 1: Configuration & Versioning
- **Agent Type Selection**: Starts with the agent type step and currently defaults to `OpenCode`.
- **Version Selection**: Fetches GitHub tags, always includes `latest`, and treats `latest` as the head of the `main` branch.
- **Metadata Retrieval**: Downloads `packages.json` from the selected version to identify available packages.

### Phase 2: Package Selection
- **State Awareness**: If `packages-installed.json` exists, previous packages are used as the starting selection. If not, all available packages are selected by default.
- **Interactive Confirmation**: Users can review and modify the package selection in the TUI, including select all and unselect all actions.

### Phase 2.5: Review
- **Review Screen**: Before installation, the TUI shows a review screen with agent type, version, and selected packages.
- **Confirmation**: Users can confirm and install or go back to change selections.

### Phase 3: Deployment
- **Cleanup**: If a previous version exists, removes the previously installed `.opencode` tree before installing new packages.
- **State Update Order**: Writes the new version and selected packages to the state file before deployment.
- **Download & Extraction**: Downloads the versioned ZIP archive and extracts the repository source, then deploys only the selected packages.
- **File Placement (OpenCode Standards)**:
    - **Skills**: Each skill folder under `packages/<package-name>/skills/` is deployed to `.opencode/skills/<skill-name>/`.
    - **Subagents**: Each `AGENT.md` file under `packages/<package-name>/sub-agents/<agent-name>/` is renamed and deployed to `.opencode/agents/<agent-name>.md`.
- **Finalization**: Updates or creates the `packages-installed.json` state file.

## 4. Error Handling & Validation
- **Schema Validation**: Validates the structure of `packages.json` before execution.
- **Environment Checks**: Ensures target directories exist or are created with appropriate permissions.
- **API Resilience**: Handles GitHub API rate limits and network failures with clear user feedback.
- **Exit Handling**: `Ctrl+C` exits the TUI cleanly.

## 5. TUI Hotkeys
- **Main Menu**: `I` install/update, `U` uninstall, `Ctrl+C` quit.
- **Agent Type**: `N` next, `C` back.
- **Version**: `N` next, `C` back.
- **Packages**: `A` select all, `U` unselect all, `N` next, `B` back.
- **Review**: `N` confirm and install, `B` back.

## 6. Success Criteria
- **Atomic Deployment**: Prevents "ghost" files by ensuring a clean state before installation.
- **Automatic Selection**: Remembers user preferences from previous installations.
- **Path Compliance**: Correctly maps repository packages to the specific folder structures required by agent platforms (e.g., OpenCode's `.opencode/` directory).
