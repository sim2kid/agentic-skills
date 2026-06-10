---
name: review-pull-request
description: Pull request review, diff review, security review. Use when proposed code changes must be checked for architectural integrity, security issues, and alignment with the TAD.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: core
  triad_role: engineering
---

# Review Pull Request

## Purpose
Review proposed code changes for architectural correctness, security posture, implementation risk, and consistency with approved technical and product artifacts.

## Use This Skill When
- A patch, diff, branch, or pull request is ready for technical review.
- QA surfaces a bug that suggests a deeper structural issue.
- A change touches critical paths such as data models, auth, or external integrations.

## Inputs
- Diff, pull request, or changed files.
- Relevant TAD, PRD, and acceptance criteria.
- Test results or validation context if available.

## Workflow
1. Read the proposed changes and identify the affected system areas.
2. Cross-check the changes against the TAD and acceptance criteria.
3. Review for security issues such as auth bypass, injection risks, unsafe secret handling, and broken access boundaries.
4. Review for structural issues such as missing migrations, weak error handling, invalid assumptions, or architectural drift.
5. Review for performance and reliability risks where relevant.
6. Return a clear review outcome with specific findings.

## Rules And Constraints
- Do not approve database schema changes without an explicit migration path.
- Do not approve auth-sensitive changes that weaken access control.
- Do not focus on style-only comments unless they violate explicit project standards.
- Favor concrete, actionable findings over vague preferences.

## Validation
- Newly introduced logic should have test coverage or an explicit justification for missing tests.
- Sensitive files and protected configuration changes should be verified intentionally.
- Findings should reference specific files and lines whenever possible.

## Output Expectations
Produce a structured review report with one of two outcomes: `Approved` or `Changes Requested`, followed by actionable findings.

## Review Format
```markdown
# Code Review Report

## Outcome
Changes Requested

## Findings
- [severity] `path/to/file:line` - [issue and required change]

## Notes
- [optional context]
```
