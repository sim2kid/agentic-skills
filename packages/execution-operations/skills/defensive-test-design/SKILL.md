---
name: defensive-test-design
description: Edge-case tests, regression tests, failure-path coverage. Use when implemented code needs stronger test coverage for unhappy paths, invariants, and regressions.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: execution-operations
  track: engineering
---

# Defensive Test Design

## Purpose
Improve implementation safety by adding focused automated tests around failure paths, edge cases, state transitions, and regression-prone behavior.

## Use This Skill When
- A feature has only happy-path coverage.
- A bug fix needs regression protection.
- Review or QA reveals weak handling of edge conditions.

## Inputs
- Implemented code, acceptance criteria, defect history, and current tests.

## Workflow
1. Identify the invariants, edge cases, and failure modes around the target behavior.
2. Add tests that exercise these conditions directly.
3. Keep test scope local to the changed behavior unless broader regression patterns are proven.
4. Verify the new tests fail before the fix when applicable, then pass after the fix.

## Rules And Constraints
- Avoid redundant tests that only restate existing coverage.
- Prefer explicit names and failure intent over clever test abstractions.
- Focus on behavior that is likely to regress or fail under real conditions.

## Validation
- The resulting tests should cover at least one non-happy-path or regression case.
- Test names and assertions should clearly express the protected behavior.

## Output Expectations
Produce targeted tests that measurably improve safety around the implemented behavior.
