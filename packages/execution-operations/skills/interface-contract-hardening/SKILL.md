---
name: interface-contract-hardening
description: Interface hardening, versioning, error contracts. Use when schemas or APIs exist but need stricter boundary definitions, compatibility rules, and failure semantics before implementation.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: execution-operations
  track: engineering
---

# Interface Contract Hardening

## Purpose
Strengthen already-defined interfaces so downstream engineers do not guess versioning rules, ownership boundaries, or failure behavior.

## Use This Skill When
- An API or schema draft exists but leaves boundary behavior ambiguous.
- Cross-team or cross-module work requires stronger contract guarantees.
- A review reveals unclear error semantics, ownership, or backward-compatibility rules.

## Inputs
- Existing API specs, schemas, event contracts, or module interfaces.
- System modeling artifacts and relevant architectural constraints.

## Workflow
1. Review the current contract for ambiguity in fields, ownership, lifecycle, and versioning.
2. Add explicit error behaviors, invariants, and compatibility expectations.
3. Clarify required, optional, nullable, and deprecated elements.
4. Document consumer and producer responsibilities.
5. Return a hardened contract ready for implementation and review.

## Rules And Constraints
- Do not leave backward-compatibility behavior implicit where multiple consumers may exist.
- Define failure cases as first-class contract behavior.
- Keep the contract consistent with upstream architecture and product scope.

## Validation
- The resulting contract should remove ambiguity around lifecycle, ownership, and failure handling.
- Required and optional fields should be distinguishable at a glance.

## Output Expectations
Produce an updated contract specification with clearer invariants, failure semantics, and compatibility notes.
