---
name: define-acceptance-criteria
description: Acceptance criteria, Gherkin, Given When Then. Use when a story or PRD needs binary pass-fail scenarios for engineering and QA validation.
license: MIT
compatibility:
  - opencode
metadata:
  owner: product-management
  package: core
  triad_role: product
---

# Define Acceptance Criteria

## Purpose
Translate product intent into clear, testable acceptance criteria that Engineering and QA can use to determine whether a story or feature is complete.

## Use This Skill When
- A user story exists but lacks concrete pass-fail conditions.
- Engineering needs clarity on expected behavior or edge cases.
- QA needs a baseline for validation planning.
- A PRD requirement must be converted into implementation-ready scenarios.

## Inputs
- The relevant user story, feature description, or PRD section.
- Any known constraints, permissions, error conditions, or edge cases.

## Workflow
1. Read the story and its surrounding product context.
2. Identify the primary happy path.
3. Identify at least one invalid, failure, or edge condition.
4. Write scenarios using `Given`, `When`, `Then` statements.
5. Keep each scenario focused on observable behavior.
6. Append or return the criteria in a format ready for the downstream artifact.

## Rules And Constraints
- Criteria must be binary and testable.
- Avoid subjective words such as `intuitive`, `easy`, `fast`, or `seamless` unless a measurable threshold is provided elsewhere.
- Do not specify the testing framework or implementation method.
- Frame scenarios from the user's perspective or the system's externally visible behavior.

## Validation
- Every `When` must have a corresponding `Then`.
- At least one non-happy-path scenario should be included when failure is possible.
- Criteria should cover the story's core success case without duplicating implementation details.

## Output Expectations
Produce a concise Markdown list of Gherkin-style scenarios ready to attach to a story or PRD.

## Example Format
```markdown
## Acceptance Criteria
- Given [context], when [action], then [outcome]
- Given [context], when [action], then [outcome]
```
