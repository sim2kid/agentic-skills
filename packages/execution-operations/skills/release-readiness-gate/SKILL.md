---
name: release-readiness-gate
description: Release gate, ship readiness, residual risk summary. Use when QA must determine whether a tested feature or build is ready to ship, hold, or require mitigation.
license: MIT
compatibility:
  - opencode
metadata:
  owner: quality-assurance
  package: execution-operations
  track: operations
---

# Release Readiness Gate

## Purpose
Provide an explicit release decision based on tested coverage, open defects, and residual operational risk.

## Use This Skill When
- A feature or release candidate needs final QA disposition.
- Leadership needs a clear ship, hold, or conditional-release recommendation.

## Inputs
- Test results, defect lists, coverage summary, and release criteria.

## Workflow
1. Summarize completed validation against requirements.
2. Identify unresolved defects and their severity.
3. Assess residual risk and missing coverage.
4. Return a clear recommendation: Ship, Hold, or Ship With Explicit Risk Acceptance.

## Rules And Constraints
- Do not hide unresolved critical or blocker issues inside summary text.
- Make the recommendation unambiguous.
- Keep risk statements evidence-based.

## Validation
- Recommendation should be traceable to executed tests and known defect state.
- Coverage gaps should be visible to decision-makers.

## Output Expectations
Produce a release readiness summary with status, evidence, and residual risks.
