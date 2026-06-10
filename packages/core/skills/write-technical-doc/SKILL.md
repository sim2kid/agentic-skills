---
name: write-technical-doc
description: Technical documentation, codebase map, component ownership, runtime behavior. Use when developers need implementation-level understanding without turning docs into source commentary.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: core
  triad_role: engineering
---

# Write Technical Documentation

## Purpose
Produce technical documentation that explains how the codebase realizes a system: which components exist, what they own, how they collaborate, what contracts and state transitions matter, and how developers can safely extend or debug the implementation.

## Use This Skill When
- Developers need a programming-level map of an existing subsystem or implementation.
- A feature already exists architecturally, but the code structure, runtime behavior, and ownership boundaries need to be documented.
- A subsystem has non-obvious component interactions, events, state machines, extension points, or failure behavior.
- The goal is to help engineers understand and modify code safely, not to justify the design choice or plan a migration.

## Inputs
- Relevant code, configuration, tests, and existing architecture context.
- Existing domain terminology, module boundaries, and implementation constraints.
- Known state transitions, event flows, failure behavior, and extension mechanisms.

## Workflow
1. Summarize the subsystem's role in the codebase and the problem space it serves.
2. Define the key concepts and states developers need before reading component details.
3. Identify the major logical components and describe responsibilities, non-responsibilities, direct collaborators, and authority boundaries.
4. Document the most important runtime interaction flows, including state transitions, events, and data movement.
5. Explain key contracts such as interfaces, guarantees, assumptions, and non-guarantees.
6. Document dependency direction: upstream callers, downstream collaborators, and external dependencies.
7. Describe extension points and how to add or modify behavior safely.
8. Include failure paths, retries, cancellation, recovery, idempotency, or timeout behavior when relevant.
9. Add focused diagrams for component relationships, sequences, state machines, dependency direction, or event flow when useful.

## Rules And Constraints
- Organize around logical concepts and behaviors, not directory listings or file-by-file commentary.
- Focus on ownership, interactions, contracts, state, events, and extension guidance.
- Do not duplicate design rationale that belongs in a design document unless brief context is necessary.
- Do not restate source code line by line, API reference specs, or implementation task lists.
- Include enough implementation detail to guide safe modification, but not so much that the document becomes a brittle code walkthrough.
- Prefer behavior and boundary explanations that survive moderate refactoring.

## Validation
- A developer unfamiliar with the subsystem should be able to identify where behavior lives, how components interact, what assumptions exist, and how to change the system safely.
- Major components should have clear responsibilities, non-responsibilities, and collaborators.
- Important state transitions, events, or failure paths should be documented when they exist.
- Extension points and contracts should be explicit enough to reduce accidental misuse.
- The document should describe the implementation structure without devolving into source commentary.

## Output Expectations
Produce Markdown technical documentation focused on ownership, runtime behavior, contracts, dependencies, and safe extension. Include Mermaid or text diagrams when they clarify structure or flow.

## Suggested Template
```markdown
# Technical Documentation

## Overview
- [Subsystem purpose and scope]

## Key Concepts
- [Concept]: [Definition]

## Component Architecture
### [Component Name]
- Responsibilities:
- Does Not Own:
- Collaborators:

## Interaction Flows
1. [Important sequence]

## Contracts
- [Interface or guarantee]

## State Machine
- [Valid states and transitions]

## Events And Data Flow
- [Producer -> consumer flow]

## Dependencies
- Upstream:
- Downstream:
- External:

## Extension Points
- [How to add or modify behavior safely]

## Failure Behavior
- [Failure mode and handling]
```
