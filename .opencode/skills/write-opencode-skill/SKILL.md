---
name: write-opencode-skill
description: Write OpenCode skills, SKILL.md authoring, skill template. Use when a new skill must be created with correct OpenCode frontmatter, narrow scope, procedural workflow, and clear validation.
license: MIT
compatibility:
  - opencode
metadata:
  owner: studio-authoring
  package: studio-authoring
  scope: authoring
---

# Write OpenCode Skill

## Purpose
Author a high-quality OpenCode skill that is narrow, discoverable, procedural, and ready to be loaded on demand by an agent.

## Use This Skill When
- A new `SKILL.md` must be written from scratch.
- Existing documentation or SOPs need to be converted into a reusable OpenCode skill.
- A team needs help structuring a skill correctly for OpenCode discovery and execution.

## Inputs
- The task the skill should cover.
- Any existing SOPs, notes, templates, or examples.
- The intended package or domain owner.

## Workflow
1. Identify the single coherent problem the skill should solve.
2. Name the skill using lowercase hyphen-separated naming that matches the directory name.
3. Write a discovery-oriented `description` that clearly states what the skill does and when it should be used.
4. Draft the body using this standard structure:
   - `# Purpose`
   - `## Use This Skill When`
   - `## Inputs` when useful
   - `## Workflow`
   - `## Rules And Constraints`
   - `## Validation`
   - `## Output Expectations`
5. Keep the content procedural and actionable rather than encyclopedic.
6. Add examples or templates only when they materially improve execution.
7. Verify the result follows OpenCode frontmatter and naming requirements.

## Rules And Constraints
- The skill must solve one narrow class of tasks.
- The skill must describe process and decisions, not serve as a reference dump.
- The `name` must match the containing directory and use only lowercase alphanumeric segments separated by single hyphens.
- The `description` must be specific enough for OpenCode to discover the skill reliably.
- If procedural assets are required, place them alongside `SKILL.md` in the same skill directory.

## OpenCode Requirements
- `SKILL.md` must begin with YAML frontmatter.
- Include at least:
  - `name`
  - `description`
- Common optional fields:
  - `license`
  - `compatibility`
  - `metadata`
- Recommended name pattern:
  - `^[a-z0-9]+(-[a-z0-9]+)*$`
- The skill directory name and `name` value must match.

## Validation
- Confirm the skill has a single, coherent scope.
- Confirm the workflow is ordered and executable.
- Confirm rules and validation are explicit.
- Confirm the naming and frontmatter satisfy OpenCode expectations.
- Confirm the description clearly communicates activation conditions.

## Output Expectations
Produce a complete OpenCode `SKILL.md` that is ready to place in `packages/<package>/skills/<skill-name>/SKILL.md`.
