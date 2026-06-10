---
name: bug-triage-root-cause
description: Bug triage, root cause isolation, flaky test diagnosis. Use when failures need to be reproduced, categorized, and reduced to an actionable defect ticket.
license: MIT
compatibility:
  - opencode
metadata:
  owner: quality-assurance
  package: execution-operations
  track: operations
---

# Bug Triage Root Cause

## Purpose
Turn vague or noisy failures into precise defect artifacts with reproducible evidence and clear fault localization.

## Use This Skill When
- Integration, end-to-end, or CI runs fail unexpectedly.
- A user-reported issue needs technical isolation.
- Flaky failures need to be narrowed to a real fault domain.

## Inputs
- Error logs, stack traces, environment details, screenshots, steps, and failing test output.

## Workflow
1. Isolate the failure signal and reconstruct the execution path.
2. Reproduce the issue in the smallest viable context.
3. Identify the likely failing layer, component, or transaction.
4. Assign a severity and capture replication instructions.
5. Produce a structured defect ticket.

## Rules And Constraints
- Do not guess fixes without evidence.
- Do not collapse distinct bugs into one ticket.
- Separate confirmed observations from suspected causes.

## Validation
- Reproduction steps should reliably recreate the issue when possible.
- The defect should identify the impacted component or boundary clearly.

## Output Expectations
Produce a technical defect ticket with context, evidence, reproduction steps, severity, and fault categorization.
