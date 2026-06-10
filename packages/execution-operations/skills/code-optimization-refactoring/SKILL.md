---
name: code-optimization-refactoring
description: Refactoring, optimization, performance cleanup. Use when existing code must be improved for maintainability or efficiency without changing external behavior.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: execution-operations
  track: engineering
---

# Code Optimization Refactoring

## Purpose
Improve existing code where performance, complexity, or maintainability problems exist, while preserving the existing external contract.

## Use This Skill When
- Review flags performance or maintainability problems.
- Legacy implementation must be reshaped to fit architectural boundaries.
- A hotspot or repeated pattern needs targeted cleanup.

## Inputs
- Target files, review findings, performance concerns, and current tests.

## Workflow
1. Isolate the exact code area needing improvement.
2. Establish the current behavior and risk boundaries.
3. Apply focused refactoring or optimization.
4. Run regression validation to ensure behavior stays unchanged.
5. Document any meaningful trade-offs introduced.

## Rules And Constraints
- Never change public behavior without explicit approval.
- Do not optimize prematurely when readability is the bigger concern.
- Keep refactors focused and reversible.

## Validation
- Existing tests should still pass after the change.
- Performance or maintainability improvements should be explainable, not merely asserted.

## Output Expectations
Produce a focused refactoring diff with unchanged external behavior.
