# Project Overview: Agentic Skills

This project aims to create a repository of explicit sub-agents that take on specialized roles. These sub-agents possess specific skills and can collaborate by referring to one another to achieve complex goals.

## Core Philosophy

- **Specialization**: Each agent has a focused domain (e.g., Technical Design, UX, Programming).
- **Skill-Based**: Agents are defined by their "skills" which dictate their capabilities and constraints.
- **Collaboration**: Agents have a defined hierarchy and reporting structure.

## Planned Sub-Agents

The project follows a standardized studio roster to leverage industry-standard behaviors:

### The Core Triad (Conflict Resolution)
- **Product Manager**: Defines vision, goals, and acceptance criteria.
- **Lead Engineer**: Establishes technical architecture and quality standards.
- **Lead UX/UI Designer**: Owns the user experience, flow, and visual interface.

### Execution & Operations
- **Systems Architect**: Designs modular components and interface definitions.
- **Senior Software Engineer**: Executes code implementation and localized logic.
- **QA Lead**: Manages bug triaging, test validation, and change requests.
- **Lead Producer**: Orchestrates workflows, routes tasks, and unblocks the team.

### Specialized Support
- **DevOps Specialist**: Manages deployment, environments, and scalability.
- **Security Auditor**: Conducts passive reviews for vulnerabilities and licensing compliance.

## Repository Structure

The repository is organized into a modular package-based system:

- **`packages/`**: The root directory for all agentic packages.
    - **`<package-name>/`**: A specific package (e.g., `core`, `unity-dev`).
        - **`sub-agents/`**: Contains specialized agents.
            - **`<agent-name>/`**: Individual agent folder.
                - **`AGENT.md`**: The agent definition following the OpenCode format.
        - **`skills/`**: Contains modular skills.
            - **`<skill-name>/`**: Individual skill folder.
                - **`SKILL.md`**: The skill definition following the OpenCode format.
        - **`package-info.json`**: ID, name, description, and dependencies for the package.

> **Note**: During the build process, the root-level `packages.json` is automatically populated using data from each package's `package-info.json`. This ensures metadata is maintained in a single location per package.

## Refinement & Evolution

To ensure these agents work effectively together, we are implementing:
1.  **Artifact Standards**: Each agent produces specific documents (TADs, UX Reports, etc.) to ensure clear handoffs.
2.  **Conflict Resolution**: The "Triad" protocol handles disagreements between Product, Technical, and UX leads.
3.  **Standardized Lifecycles**: A clear path from issue intake to implementation and validation.
4.  **Modular Skill System**: Skills are treated as context chips to prevent context pollution and maintain agent focus.
5.  **Automated Installation**: A dedicated script for deploying skills and agents into target repositories (see [Install Script Design](install-script.md)).

---
*This document is a living design record and will be updated as the project evolves.*
