---
name: write-design-doc
description: Design doc, architecture narrative, system flows, trade-offs. Use when a feature or system needs a shared mental model before implementation begins.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: core
  triad_role: engineering
---

# Write Design Document

## Purpose
Produce a design document that explains what is being built, why the design was chosen, how the major parts fit together, and what trade-offs or consequences matter before implementation starts.

## Use This Skill When
- A feature, subsystem, refactor, or migration needs a shared architecture narrative before implementation.
- Reviewers need to understand major flows, responsibilities, and system interactions without reading code.
- The task requires explicit goals, non-goals, alternatives, risks, and high-level architecture.
- A planning artifact should explain system behavior and decisions rather than code structure or task sequencing.

## Inputs
- Problem statement, requested change, or approved product scope.
- Existing repository context, current architecture, and known operational or business constraints.
- Relevant UX flows, technical constraints, and prior decisions when available.

## Workflow
1. State the problem and current shortcomings before describing the solution.
2. Define the proposed solution in plain system terms that non-implementers can follow.
3. Capture explicit goals and non-goals to control scope.
4. Describe the most important user and system flows in step-by-step order, including asynchronous boundaries and failure behavior where relevant.
5. Identify the major components, ownership boundaries, and sources of truth.
6. Document key technical considerations only when they materially affect architecture, UX, performance, security, or operations.
7. Summarize alternatives considered, why they were rejected or chosen, and the consequences of the decision.
8. Call out risks, open questions, and likely future extensions.
9. Add focused diagrams when they clarify flows, boundaries, states, or ownership.

## Rules And Constraints
- Focus on enduring system behavior, responsibilities, interactions, constraints, and decisions.
- Prefer flow descriptions over lists of implementation facts.
- Do not turn the document into pseudo-code, class walkthroughs, API specs, schema dumps, or task checklists.
- Keep implementation details out unless changing them would materially change the architecture or user experience.
- Define domain-specific terms early when they may be unfamiliar to readers outside the immediate team.
- Use multiple small diagrams when needed rather than one overloaded architecture picture.

## Validation
- A reader should be able to answer what is being built, why it is designed this way, how the major pieces interact, and what trade-offs were made.
- Flows should cover major happy paths and relevant failure or recovery paths.
- Goals and non-goals should make scope boundaries explicit.
- Alternatives, risks, and open questions should be visible rather than implied.
- The document should remain useful even if implementation details later change.

## Output Expectations
Produce a Markdown design document that is problem-first, flow-oriented, responsibility-driven, and implementation-light. Include Mermaid or simple text diagrams when they improve clarity.

## Suggested Template
```markdown
# Design Document

## Summary
- [Problem, proposal, affected users, major change]

## Problem Statement
- [Current state and why it is insufficient]

## Goals
- [Goal]

## Non-Goals
- [Explicit exclusion]

## Glossary
- [Term]: [Definition]

## User Experience And User Flows
1. [Step-by-step user and system behavior]

## High-Level Architecture
- [Major component and responsibility]

## System Interaction And Data Flow
1. [Important runtime sequence]

## Key Technical Considerations
- [Constraint or rule that materially affects the design]

## Alternatives Considered
- [Option]: [Pros, cons, decision]

## Risks And Open Questions
- [Risk or open question]

## Future Extensions
- [Likely future direction]
```
