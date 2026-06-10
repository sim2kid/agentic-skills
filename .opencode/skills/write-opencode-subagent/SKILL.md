---
name: write-opencode-subagent
description: Write OpenCode sub-agents, AGENT.md authoring, subagent template. Use when a new focused sub-agent must be created with clear responsibilities, boundaries, collaboration rules, and curated skills.
license: MIT
compatibility:
  - opencode
metadata:
  owner: studio-authoring
  package: studio-authoring
  scope: authoring
---

# Write OpenCode Sub-Agent

## Purpose
Author a high-quality OpenCode `AGENT.md` that defines a focused specialist with clear ownership, hard boundaries, explicit inputs and outputs, and an appropriate skill set.

## Use This Skill When
- A new sub-agent must be created for the studio.
- A role concept needs to be translated into an explicit agent definition.
- A package needs a new specialist with clean collaboration behavior.

## Inputs
- The role concept, responsibility area, and intended package.
- Required inputs, outputs, artifacts, and adjacent collaborators.
- The skills the agent should be able to load.

## Workflow
1. Define the agent's singular responsibility.
2. State the purpose the agent serves in business, engineering, or operational terms.
3. Write invocation conditions in `## Use This Agent When`.
4. Define `## Inputs`, `## Responsibilities`, and `## Outputs`.
5. Add `## Available Skills` with only the curated skills this role should load.
6. Establish `## Boundaries` so the role does not drift into neighboring domains.
7. Add `## Collaboration` and `## Completion Criteria` to support reliable handoffs.
8. Ensure the prompt remains responsibility-driven rather than procedural.

## Rules And Constraints
- The sub-agent should own one domain of decision making.
- The prompt should describe ownership and interaction, not embed SOP-level procedures.
- Inputs and outputs must be explicit enough for artifact-driven collaboration.
- Boundaries should state both what the agent does and what it must not do.
- Keep the skill list curated and avoid turning the agent into a generalist.

## OpenCode Requirements
- Use a valid `AGENT.md` with frontmatter such as:
  - `description`
  - `mode: subagent`
- Keep the body as the prompt content.
- Prefer file-based sub-agents under `packages/<package>/sub-agents/<agent-name>/AGENT.md` in this repository structure.

## Validation
- Confirm the role is singular and well bounded.
- Confirm the agent collaborates through explicit artifacts.
- Confirm the available skills fit the role without expanding it into multiple domains.
- Confirm the final file is compatible with OpenCode agent conventions.

## Output Expectations
Produce a complete `AGENT.md` suitable for inclusion in a package sub-agent directory.
