---
name: specify-ui-components
description: UI component spec, accessibility states, design tokens. Use when front-end implementation needs exact component states, tokens, and a11y requirements.
license: MIT
compatibility:
  - opencode
metadata:
  owner: design-team
  package: core
  triad_role: ux
---

# Specify UI Components

## Purpose
Define reusable UI components with explicit states, token references, and accessibility requirements so front-end implementation can proceed without inventing interaction behavior or accessibility rules.

## Use This Skill When
- A user flow is approved and front-end work needs component-level specifications.
- A new component must be introduced with clear interaction and accessibility behavior.
- Engineering needs exact state definitions for complex or reusable UI elements.

## Inputs
- Approved user flow and relevant product requirements.
- Existing design system, token set, or UI conventions.
- Technical constraints from the implementation environment.

## Workflow
1. Identify the components implied by the user flow.
2. For each component, define default, hover, focus, active, disabled, error, and loading states as applicable.
3. Reference approved design tokens for spacing, color, typography, elevation, and sizing.
4. Define accessibility requirements including roles, labels, keyboard behavior, and screen reader announcements.
5. Document any responsive or platform-specific variations.
6. Compile the result into a structured component specification.

## Rules And Constraints
- Reuse the project's existing design system whenever it exists.
- Do not invent arbitrary visual values when tokens or scale references are available.
- Every interactive component must define a keyboard-visible focus state.
- Icon-only controls must have explicit accessible names.

## Validation
- Token usage should support WCAG AA contrast where relevant.
- Accessibility attributes and keyboard behavior should be explicit for interactive controls.
- Required states should align with the user flow's success, loading, and error conditions.

## Output Expectations
Produce a Markdown UI specification organized by component, including states, token references, and accessibility expectations.

## Component Spec Template
```markdown
# UI Component Specification

## Component: [Name]
- Purpose: [Why it exists]
- States: Default, Hover, Focus, Active, Disabled, Error, Loading
- Tokens:
  - Color: [token]
  - Spacing: [token]
  - Typography: [token]
- Accessibility:
  - Role: [role]
  - Labeling: [aria-label or visible label rules]
  - Keyboard: [focus and key behavior]
  - Announcements: [screen reader messaging if needed]
```
