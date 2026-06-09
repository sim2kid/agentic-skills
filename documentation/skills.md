# Modular Skill System

To prevent "context pollution," skills are treated as modular context chips injected into an agent's prompt only when relevant to the current task.

## OpenCode Compliance

All skills in this repository follow the [OpenCode Skill Format](https://opencode.ai/docs/skills/). 

### Skill Structure
Each skill is contained within its own directory under a package's `skills/` folder:
```
packages/<package-name>/skills/<skill-name>/
└── SKILL.md
```

### `SKILL.md` Requirements
- **Frontmatter**: Must include `name` and `description`.
- **Content**: Detailed instructions and constraints for the specific skill.

## Shared Skills
*Available to all agents in the studio.*

- **Markdown & Artifact Formatting**: Strict adherence to templates for TADs, Bug Reports, and Resolution Memos.
- **Context Condensation**: Ability to summarize long threads into dense, token-efficient briefs before passing the baton.

## Role-Specific Skills
*Equipped only as needed based on the task.*

### Senior Software Engineer
- **Engine-Specific Optimization**: Standards for C# scripting, memory management, and engine-specific lifecycles (e.g., Unity MonoBehaviour, ECS).
- **Clean Coding Practices**: Standards for readability, naming conventions, and localized logic patterns.

### Systems Architect
- **System Decoupling**: Application of SOLID principles, dependency injection, and interface-driven design.
- **Structural Patterns**: Expertise in microservices, modular monoliths, and component-based architecture.

### Lead UX/UI Designer
- **Experience Design**: Principles of user flow, accessibility, and cognitive load management.
- **UI Prototyping**: Standards for layout, color theory, and interaction feedback.

### QA Lead
- **Test Engineering**: Writing unit, integration, and edge-case test scenarios.
- **Diagnostic Triage**: Procedures for isolating bugs and identifying root causes.

### DevOps Specialist
- **Deployment Architecture**: Expertise in containerization (Docker), networking, and volume management.
- **Infrastructure as Code**: Managing environments via scripts and configurations.

### Security Auditor
- **Vulnerability Assessment**: Auditing code and dependencies for security flaws.
- **Compliance & Licensing**: Ensuring strict open-source licensing and legal standards are met.
