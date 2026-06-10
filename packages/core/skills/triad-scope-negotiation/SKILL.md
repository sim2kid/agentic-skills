---
name: triad-scope-negotiation
description: Triad scope negotiation, scope conflict, product trade-off. Use when product scope conflicts with engineering constraints or UX concerns and a documented compromise is needed.
license: MIT
compatibility:
  - opencode
metadata:
  owner: product-management
  package: core
  triad_role: product
---

# Triad Scope Negotiation

## Purpose
Guide the Product Manager's role in the Triad Protocol when scope, engineering feasibility, or UX quality are in tension and a documented compromise is required.

## Use This Skill When
- Engineering flags a requirement as high risk, overly expensive, or architecturally unsound.
- UX identifies a requirement that harms usability or creates an unacceptable flow.
- Product direction must be reduced or phased without losing the core user outcome.
- A conflict between Triad roles needs a written decision record.

## Inputs
- The disputed requirement or PRD section.
- Constraint statements from Engineering and or UX.
- The underlying user need the feature is meant to satisfy.

## Workflow
1. Acknowledge the constraint raised by the other Triad participant.
2. Restate the underlying user need in product terms.
3. Explore the smallest compromise that preserves most of the value.
4. Prefer one of these moves when appropriate:
   - Cut scope to the core user outcome.
   - Phase advanced functionality into a later increment.
   - Replace a custom requirement with an implementation-neutral alternative.
   - Preserve safety and modularity even if it reduces initial ambition.
5. Record the agreed decision and update the PRD scope boundaries.

## Rules And Constraints
- Never ignore a legitimate security, architecture, compliance, or accessibility risk.
- You may cut or defer scope, but you may not silently redefine the product objective.
- Optimize for shipping a coherent, modular slice rather than an oversized first release.

## Validation
- The compromise must preserve a meaningful user outcome.
- The updated scope must be reflected in `In Scope` and `Out of Scope` sections.
- The decision record must make the trade-off understandable to downstream agents.

## Output Expectations
Produce two artifacts when needed:
- An updated PRD section or scope statement.
- A short decision record explaining what changed, why it changed, and who raised the constraint.

## Decision Record Template
```markdown
## Triad Decision Record
- Issue: [conflict summary]
- Product Need: [underlying user need]
- Constraint Raised By: [Engineering or UX]
- Decision: [agreed compromise]
- Scope Change: [what moved in or out of scope]
- Rationale: [why this trade-off is acceptable]
```
