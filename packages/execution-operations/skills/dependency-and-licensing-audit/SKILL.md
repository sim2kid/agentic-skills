---
name: dependency-and-licensing-audit
description: Dependency audit, license review, FOSS compliance. Use when a new third-party package or library must be evaluated for licensing, security, maintenance, and modular fit.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: execution-operations
  track: engineering
---

# Dependency And Licensing Audit

## Purpose
Protect the system from bloated, insecure, duplicate, or licensing-problematic dependencies before they become structural liabilities.

## Use This Skill When
- A new dependency is proposed during architecture or implementation.
- A library choice needs validation before becoming part of the approved stack.
- Architectural design needs a FOSS and modularity check.

## Inputs
- Dependency name, target version, and intended use.
- Existing stack context and any policy or licensing constraints.

## Workflow
1. Identify the dependency, version, and intended architectural role.
2. Review license terms and reject restrictive, proprietary, or open-core traps.
3. Review maintenance health, dependency weight, and known security posture.
4. Check whether the capability already exists elsewhere in the approved stack.
5. Produce an explicit approval or rejection with rationale.

## Rules And Constraints
- Reject dependencies with restricted commercial use or hidden enterprise-only limitations.
- Reject dependencies with critical unresolved security issues.
- Do not approve libraries that materially damage portability or modular clarity without explicit justification.

## Validation
- Confirm the dependency does not duplicate already approved capabilities.
- Confirm the recommendation is based on explicit licensing and maintenance evidence.

## Output Expectations
Produce a dependency audit log with `Approved` or `Rejected`, rationale, and architectural impact notes.
