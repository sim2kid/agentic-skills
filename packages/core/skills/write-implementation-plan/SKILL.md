---
name: write-implementation-plan
description: Implementation plan, migration plan, phased rollout, validation strategy. Use when a change needs a concrete path from current state to target state before execution.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: core
  triad_role: engineering
---

# Write Implementation Plan

## Purpose
Produce an implementation plan that explains how to evolve the current system into the target state safely, incrementally, and verifiably, with emphasis on deltas, hookups, sequencing, and validation.

## Use This Skill When
- An approved design or scoped request needs an execution-ready change plan before implementation.
- A migration, refactor, integration, or multi-step delivery needs sequencing and dependency clarity.
- Multiple engineers or agents need a shared plan for what changes, in what order, and how the pieces hook together.
- The task requires rollout, rollback, compatibility, acceptance criteria, or validation strategy beyond a simple task list.

## Inputs
- Approved design documents, product scope, and relevant technical context.
- Current implementation reality in code, configuration, tests, and operations.
- Constraints, dependencies, risks, rollout conditions, and expected verification paths.

## Workflow
1. Summarize the change, related reference artifacts, and overall implementation strategy.
2. Describe the current state in concrete behavioral and structural terms.
3. Describe the target state after the change is complete.
4. For each major subsystem, document the delta: what exists now, what must change, and what integration hookups are required.
5. Break the work into incremental phases or milestones that leave the system in a valid state.
6. Document dependency ordering, blockers, parallelizable work, and migration constraints.
7. Create an explicit hookup checklist for cross-component integrations, startup registration, configuration wiring, event subscriptions, test updates, or compatibility shims.
8. Add implementation notes and gotchas such as initialization order, idempotency, concurrency, feature flags, temporary compatibility paths, or cleanup requirements.
9. Define acceptance criteria, validation strategy, rollout plan, rollback plan, risks, and open questions.

## Rules And Constraints
- Focus on the delta between current state and target state, not on re-explaining the design from scratch.
- Emphasize integrations and hookups, since these are often where implementation fails.
- Organize by subsystem, phase, or migration step rather than by individual commits or developer assignments.
- Do not turn the plan into a project-management staffing checklist or a PR diff summary.
- Acceptance criteria should describe observable outcomes, not merely the existence of code artifacts.
- Each phase should be understandable and leave the system in a stable, testable condition when possible.

## Validation
- The reader should understand current state, target state, required deltas, ordering, dependencies, and verification strategy.
- Major integration points and hookups should be explicit rather than assumed.
- Acceptance criteria should be testable by someone other than the author.
- Validation steps should map directly to the acceptance criteria and migration risks.
- The document should enable multiple contributors to work independently without losing coherence.

## Output Expectations
Produce a Markdown implementation plan that is delta-focused, integration-aware, incremental, and validation-driven. Include concise diagrams or tables only when they materially improve sequencing or hookup clarity.

## Suggested Template
```markdown
# Implementation Plan

## Overview
- [Change summary and overall strategy]

## References
- [Design doc, technical doc, ADR, API doc]

## Current State
1. [How the system works today]

## Target State
1. [How the system should work after the change]

## Detailed Change List
### [Subsystem]
- Current:
- Changes:
- Hookups:
- Notes:

## Phases
1. [Phase and outcome]

## Dependencies And Ordering
- Depends On:
- Blocks:
- Parallelizable:

## Integration Checklist
- [Required hookup]

## Acceptance Criteria
- [Observable outcome]

## Validation Strategy
- Unit Tests:
- Integration Tests:
- Manual Validation:
- Regression Validation:

## Rollout And Rollback
- Rollout:
- Rollback:

## Risks And Open Questions
- [Risk or open question]
```
