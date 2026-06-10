---
name: skill-quality-review
description: Review OpenCode skills, skill QA, skill refinement. Use when a proposed skill needs review for scope, procedural clarity, discovery quality, validation strength, and OpenCode compliance.
license: MIT
compatibility:
  - opencode
metadata:
  owner: studio-authoring
  package: studio-authoring
  scope: authoring
---

# Skill Quality Review

## Purpose
Evaluate a proposed skill and identify whether it is narrow enough, discoverable enough, and operationally useful enough to include in the studio.

## Use This Skill When
- A drafted skill needs quality review before inclusion.
- A skill behaves poorly in practice and may have scope or discovery problems.
- A maintainer wants structured feedback on skill authoring quality.

## Inputs
- The proposed `SKILL.md`.
- Any related context about expected usage or package placement.

## Workflow
1. Review the frontmatter for OpenCode compliance.
2. Check whether the skill has one coherent responsibility.
3. Evaluate the `description` for discoverability and activation clarity.
4. Review the workflow for procedural usefulness and completeness.
5. Check whether rules, validation, and output expectations are explicit.
6. Identify excess reference material, role drift, or missing constraints.
7. Return a concise revision recommendation set.

## Rules And Constraints
- Do not confuse a good skill with a good wiki page.
- Flag skills that are too broad, too vague, or too reference-heavy.
- Treat weak discovery metadata as a significant defect.

## Validation
- The review should explain whether the skill is usable as written.
- Findings should be actionable and tied to concrete sections.
- The result should distinguish compliance issues from quality issues.

## Output Expectations
Produce a structured skill review with findings, revision recommendations, and an overall readiness judgment.
