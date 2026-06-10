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

- **`sub-agents/`**: Contains specialized agents with their own `AGENT.md` definition.
- **`skills/`**: Contains modular skills, each in its own directory with a `SKILL.md` file.
- **`package-info.json`**: Stores metadata (id, name, description, dependencies) for the package.

All definitions follow the [OpenCode](https://opencode.ai/docs/) standards for skills and agents.

## Installation

A dedicated install script is provided to deploy these skills into target repositories.

**Features:**
- Explicit package selection.
- Support for upgrades and rollbacks based on git releases.
- Cross-device compatibility.

Run the installation command (Windows):
```powershell
powershell -ExecutionPolicy Bypass -Command "iwr -useb https://raw.githubusercontent.com/sim2kid/agentic-skills/main/install-tui.ps1 | iex"
```

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
