---
name: design-user-flow
description: User flow, journey map, screen transitions. Use when a feature needs a step-by-step interaction flow with system feedback before engineering begins.
license: MIT
compatibility:
  - opencode
metadata:
  owner: design-team
  package: core
  triad_role: ux
---

# Design User Flow

## Purpose
Translate product requirements and stories into a concrete interaction flow that defines how users move through a feature, what decisions they make, and how the system responds at each step.

## Use This Skill When
- A new feature or epic requires a user journey before implementation.
- The team needs to understand screen transitions, branching behavior, or feedback states.
- Engineering needs explicit flow context to build the right screens and interactions.

## Inputs
- PRD, stories, and acceptance criteria.
- Existing navigation patterns, platform constraints, and known usability constraints.

## Workflow
1. Read the product scope and identify the entry point.
2. Map the user's actions step by step.
3. Define system feedback for each step, including loading, success, error, and empty states where relevant.
4. Map alternate and unhappy paths explicitly.
5. Ensure the user can recover, go back, cancel, or retry when appropriate.
6. Output the flow in a clear sequential format.

## Rules And Constraints
- Every meaningful user action should have a corresponding system response.
- Do not create dead ends without an intentional escape or recovery path.
- Keep flows aligned to the approved scope rather than inventing extra product behavior.

## Validation
- Acceptance criteria should be satisfiable within the flow.
- Happy paths and unhappy paths should both be represented where failure is possible.
- The sequence should be understandable by designers, engineers, and QA.

## Output Expectations
Produce a Markdown flow map or text-based flowchart showing `Screen -> Action -> Feedback -> Next Screen`.

## Flow Template
```markdown
# User Flow

1. Screen: [Entry screen]
   Action: [What the user does]
   Feedback: [System response]
   Next: [Destination]
```
