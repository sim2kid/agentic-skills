# Agentic Skills Repository

This repository serves as a centralized hub for defining and distributing OpenCode skills and subagents. It allows for the modular definition of specialized capabilities that can be installed into other projects.

## Architecture

The project utilizes OpenCode's [Agent Skills](https://opencode.ai/docs/skills/) and [Subagents](https://opencode.ai/docs/agents/#subagents) to provide extensible software engineering capabilities.

### Package Management
Available packages are defined in `packages.json` and organized within the corresponding file structure. Each package contains the necessary skill definitions and agent configurations.

## Installation

A dedicated install script is provided to deploy these skills into target repositories.

**Features:**
- Explicit package selection.
- Support for upgrades and rollbacks based on git releases.
- Cross-device compatibility.

Run the installation command (Windows):
```powershell
powershell -ExecutionPolicy Bypass -Command "iwr -useb https://raw.githubusercontent.com/OWNER/REPO/main/install.ps1 | iex"
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
