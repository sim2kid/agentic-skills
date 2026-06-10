---
name: quality-assurance-strategy
description: Test matrix, QA strategy, acceptance validation. Use when a feature, story set, or release candidate needs structured quality verification against requirements.
license: MIT
compatibility:
  - opencode
metadata:
  owner: quality-assurance
  package: execution-operations
  track: operations
---

# Quality Assurance Strategy

## Purpose
Establish a structured quality plan that maps requirements to deterministic verification activities and clear pass-fail results.

## Use This Skill When
- A PRD or feature scope is ready for QA planning.
- A branch, build, or release candidate requires formal validation.
- Regression coverage must be organized for a larger release.

## Inputs
- Acceptance criteria, stories, TAD context, target build, and available test environment details.

## Workflow
1. Build a test matrix covering happy paths, unhappy paths, and boundary conditions.
2. Define integration, end-to-end, or flow validation steps as appropriate.
3. Execute or specify the validation activities.
4. Mark pass-fail results clearly and log gaps or failures.

## Rules And Constraints
- Do not pass work with untested or ambiguous acceptance criteria.
- Every recorded bug must include reproducible steps.
- Keep validation tied directly to requirement evidence.

## Validation
- Coverage should map back to the PM's acceptance criteria.
- Boundary and failure conditions should be included, not just success cases.

## Output Expectations
Produce a formal test matrix with pass-fail results and uncovered-risk notes.
