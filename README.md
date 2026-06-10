# Agentic Skills Repository

This repository serves as a centralized hub for defining and distributing OpenCode skills and subagents. It allows for the modular definition of specialized capabilities that can be installed into other projects.

## Architecture

The project utilizes OpenCode's [Agent Skills](https://opencode.ai/docs/skills/) and [Subagents](https://opencode.ai/docs/agents/#subagents) to provide extensible software engineering capabilities.

### Package Management

This repository uses a structured, package-based organization to manage agents and skills.

#### Folder Structure
```text
packages/
└── <package-name>/
    ├── sub-agents/
    │   └── <agent-name>/
    │       └── AGENT.md
    ├── skills/
    │   └── <skill-name>/
    │       └── SKILL.md
    └── package-info.json
```

Current packages include `foundation`, `studio-authoring`, `core`, `execution-operations`, `specialized-support`, and `git-writing`.

- **`sub-agents/`**: Contains specialized agents with their own `AGENT.md` definition.
- **`skills/`**: Contains modular skills, each in its own directory with a `SKILL.md` file.
- **`package-info.json`**: Stores metadata (id, name, description, dependencies) for the package.

All definitions follow the [OpenCode](https://opencode.ai/docs/) standards for skills and agents.

## Installation

A TUI-based install script is provided to deploy these skills into target repositories.

**Features:**
- Explicit package selection.
- Support for tags and latest-from-main selection.
- Cross-device compatibility.
- Single-window TUI flow with review and confirmation.
- Ctrl+C exits cleanly.

Run the installation command (Windows):
```powershell
$t = Join-Path $env:TEMP ("agentic-skills-install-tui-$([guid]::NewGuid().ToString('N')).ps1"); Invoke-WebRequest "https://raw.githubusercontent.com/sim2kid/agentic-skills/main/install-tui.ps1" -OutFile $t; & powershell -ExecutionPolicy Bypass -File $t
```

The installer downloads the repository archive, extracts the source, and launches the TUI from the extracted project so it can resolve tags, metadata, and package contents from the repo itself.

## Testing & Validation

To ensure the integrity of the package definitions, a testing script is provided. It validates:
- `packages.json` schema and completeness.
- Presence of required files for each defined package.
- Validity of OpenCode configuration files.

Run the validation script:
```bash
# Example validation command
./test_packages.sh
```
