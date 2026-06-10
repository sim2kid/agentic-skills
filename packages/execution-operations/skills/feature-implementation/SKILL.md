---
name: feature-implementation
description: Feature implementation, code generation, test-first delivery. Use when an approved story or technical task must be translated into production-ready code and tests.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: execution-operations
  track: engineering
---

# Feature Implementation

## Purpose
Implement approved technical work cleanly and locally, with tests and behavior aligned to upstream architecture and acceptance criteria.

## Use This Skill When
- A story or technical task enters active implementation.
- A bug fix requires localized source changes.

## Inputs
- TAD, API specs, stories, acceptance criteria, and relevant defect details.
- Existing codebase patterns and test conventions.

## Workflow
1. Read the relevant technical and product artifacts.
2. Identify the minimal code area required for the change.
3. Create or update tests that capture expected success and failure behavior.
4. Implement the code changes needed to satisfy the approved scope.
5. Format, lint, and verify the result locally when feasible.
6. Return a clean patch or completed file set.

## Rules And Constraints
- Never mix unrelated changes into one implementation task.
- Do not add unapproved product behavior.
- Do not bypass project validation rules without explicit instruction.

## Validation
- New or changed logic should have appropriate automated coverage.
- No hardcoded secrets or environment-specific constants should be introduced.
- The change should remain consistent with the architecture.

## Output Expectations
Produce production-ready source changes with accompanying tests.
