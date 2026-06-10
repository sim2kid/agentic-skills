---
name: epic-to-stories
description: Epic to stories, backlog planning, user stories. Use when an approved PRD or large feature must be decomposed into small, prioritized vertical slices.
license: MIT
compatibility:
  - opencode
metadata:
  owner: product-management
  package: core
  triad_role: product
---

# Break Down Epic To Stories

## Purpose
Convert a broad epic or approved PRD into a set of small, user-centered stories that can populate a backlog without losing the original product intent.

## Use This Skill When
- A PRD has been approved and needs backlog-ready stories.
- A feature is too large to implement as one unit.
- Sprint or milestone planning requires prioritized user-facing slices.

## Inputs
- Approved PRD or equivalent scope artifact.
- Priority context, release goals, or sequencing constraints if available.

## Workflow
1. Read the approved scope and identify distinct user actions or outcomes.
2. Draft stories using `As a [role], I want [action], so that [benefit]`.
3. Break large stories into smaller vertical slices when they span too many capabilities.
4. Assign a simple priority such as High, Medium, or Low.
5. Verify the combined story set covers the in-scope requirements.
6. Return the backlog in a structured Markdown list.

## Rules And Constraints
- Stories must describe user value, not internal technical tasks.
- Avoid horizontal slices such as database-only, API-only, or frontend-only tasks.
- No single story should represent the entire feature if it can reasonably be divided.
- The `benefit` clause must connect to the original product objective.

## Validation
- The story set should collectively satisfy all in-scope PRD requirements.
- Each story should be understandable without requiring hidden context.
- Priorities should reflect the smallest sequence that delivers usable value.

## Output Expectations
Produce a prioritized Markdown backlog that downstream agents can estimate, design, or implement.

## Backlog Template
```markdown
# Backlog

## High Priority
- As a [role], I want [action], so that [benefit].

## Medium Priority
- As a [role], I want [action], so that [benefit].

## Low Priority
- As a [role], I want [action], so that [benefit].
```
