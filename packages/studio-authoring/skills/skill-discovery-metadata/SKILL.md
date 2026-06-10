---
name: skill-discovery-metadata
description: Skill naming, skill descriptions, OpenCode metadata. Use when a skill's name, description, or metadata must be improved for better discovery and correct package conventions.
license: MIT
compatibility:
  - opencode
metadata:
  owner: studio-authoring
  package: studio-authoring
  scope: authoring
---

# Skill Discovery Metadata

## Purpose
Improve how a skill is discovered by OpenCode by tightening its name, description, and metadata without changing the core procedure unnecessarily.

## Use This Skill When
- A skill is difficult for agents to load consistently.
- A skill name is too vague, too broad, or inconsistent with package conventions.
- A skill description does not clearly express when the skill should be used.

## Inputs
- The current skill name, description, and metadata.
- The actual procedure or task covered by the skill.

## Workflow
1. Identify the most likely trigger phrases users or agents would use.
2. Tighten the skill name so it is specific, valid, and convention-friendly.
3. Rewrite the description so it states what the skill does and when it applies.
4. Adjust metadata only when it improves package clarity or ownership.
5. Verify that the final name remains aligned with the skill directory.

## Rules And Constraints
- Do not optimize for clever names over discoverable ones.
- Keep descriptions task-oriented and explicit about activation conditions.
- Do not add metadata fields that do not help maintenance or discovery.

## Validation
- The updated metadata should make the skill easier to select correctly.
- The final name must remain OpenCode-compliant.

## Output Expectations
Produce revised skill metadata with a short rationale for each meaningful change.
