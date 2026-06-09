# Implementation Plan: Install Script

## Goal
Create a robust, single-file installation script that allows users to select specific skills/agents from this repository and install them into another repository, with support for versioning via git releases.

## Requirements

### 1. Package Selection
- Read `packages.json` to list available packages.
- Provide an interactive CLI menu for the user to select one or multiple packages.

### 2. Versioning & Git Integration
- Fetch available git tags/releases to allow the user to specify a version.
- Implement a mechanism to rollback to a previous release by checking out specific tags before copying files.

### 3. Installation Logic
- Clone/Fetch the source repository to a temporary directory.
- Identify the target repository path.
- Copy the selected package files (skills, agents, configs) into the target directory's `.opencode/` or relevant configuration paths.
- Handle merges or overwrites of existing configuration files.

### 4. Environment Compatibility
- Ensure the script runs on multiple platforms (Bash for Unix/macOS, potentially providing a PowerShell equivalent or using a cross-platform runtime).

## Proposed Workflow
1. **Initialize**: Validate git environment and target directory.
2. **Fetch**: Retrieve the latest release or a specified tag.
3. **Select**: Parse `packages.json` $\rightarrow$ Present options $\rightarrow$ Capture selection.
4. **Deploy**: Copy files $\rightarrow$ Update target `.opencode` configs.
5. **Verify**: Run a quick check to ensure files are in place.

## Success Criteria
- Successfully installs a selected skill into a target repo.
- Correctly rolls back a package to a previous git tag.
- Fails gracefully if `packages.json` is malformed or target paths are invalid.
