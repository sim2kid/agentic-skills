---
name: write-prd
description: PRD, product requirements document, feature brief. Use when a new feature or epic needs a formal scope, goals, constraints, and clear out-of-scope boundaries.
license: MIT
compatibility:
  - opencode
metadata:
  owner: product-management
  package: core
  triad_role: product
---

# Write Product Requirements Document

# Purpose
Generate a structured Product Requirements Document (PRD) that defines the user problem, product scope, goals, and constraints without drifting into implementation or UI design detail.

## Use This Skill When
- A new feature, initiative, or epic needs a source-of-truth product artifact.
- A rough idea must be formalized before UX or Engineering begins detailed work.
- Triad review needs a baseline document to discuss trade-offs.
- A user request is broad enough that scope control is required before backlog creation.

## Inputs
- User request, feature brief, roadmap note, or problem statement.
- Relevant business constraints, licensing requirements, or platform limitations.
- Existing product context if the request extends an existing system.

## Workflow
1. Identify the core user problem and express it in one direct sentence.
2. Define the target audience, user segment, or operator role.
3. Describe the primary use case and intended user or business outcome.
4. Draft explicit `In Scope` and `Out of Scope` lists.
5. Write the functional requirements as observable product capabilities.
6. Write non-functional requirements only when they materially constrain the product, such as accessibility, compliance, open-source licensing, or portability.
7. Call out assumptions, dependencies, and unresolved questions.
8. Output the artifact using the PRD template below.

## Rules And Constraints
- Never prescribe technical implementation details, architecture, libraries, or system internals.
- Never define UI layout specifics, screen compositions, or visual styling.
- Every PRD must include an explicit `Out of Scope` section.
- Favor a smaller, shippable scope over an ambitious but ambiguous one.
- If the request is materially vague, state the ambiguity and ask for the smallest necessary clarification.

## Validation
- Ensure the problem statement is user-centric, not system-centric.
- Ensure every functional requirement ties back to the user problem or business objective.
- Ensure nice-to-have ideas are moved to `Out of Scope` unless clearly required for the first release.
- Ensure no proprietary dependency is mandated when the requirement can remain implementation-neutral.

## Output Expectations
Produce a complete Markdown PRD that another agent can use without requiring additional interpretation.

## PRD Template
```markdown
# Product Requirements Document

## Title
[Feature or epic name]

## Problem Statement
[One sentence describing the user problem]

## Target Audience
- [Primary user or stakeholder]

## Goals
- [Desired outcome]

## In Scope
- [Included capability]

## Out of Scope
- [Explicitly excluded capability]

## Functional Requirements
- [Observable system behavior or product capability]

## Non-Functional Requirements
- [Accessibility, compliance, licensing, portability, or performance constraints stated at product level]

## Assumptions
- [Assumption]

## Open Questions
- [Question requiring follow-up]
```
