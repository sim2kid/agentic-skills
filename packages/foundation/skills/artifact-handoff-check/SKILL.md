---
name: artifact-handoff-check
description: Artifact handoff validation, readiness check, downstream completeness. Use when an upstream artifact must be checked for completeness before routing it to another agent.
license: MIT
compatibility:
  - opencode
metadata:
  owner: studio-operations
  package: foundation
  scope: shared
---

# Artifact Handoff Check

## Purpose
Prevent downstream waste by verifying that artifacts are complete, internally consistent, and appropriate for the next specialist before handoff.

## Use This Skill When
- A PRD, TAD, UX flow, test plan, defect ticket, security report, or deployment artifact is about to be routed onward.
- Work repeatedly stalls because upstream artifacts are incomplete or ambiguous.
- A coordinating agent needs to confirm handoff readiness before assigning work.

## Inputs
- The artifact to be handed off.
- The downstream agent role and its expected inputs.

## Workflow
1. Check the artifact for required sections, clarity, and internal consistency.
2. Compare it against the downstream role's expected inputs.
3. Identify missing details, ambiguous boundaries, or unresolved contradictions.
4. Return either `Ready For Handoff` or `Needs Revision` with precise reasons.

## Rules And Constraints
- Do not route artifacts with known missing prerequisites.
- Do not require perfection where the downstream role legitimately owns the next layer of detail.
- Focus on handoff readiness, not authorship critique.

## Validation
- The downstream agent should be able to start work without guessing core context.
- Any missing input should be explicit and actionable.

## Output Expectations
Produce a handoff readiness note with status, missing items if any, and recommended next owner.
