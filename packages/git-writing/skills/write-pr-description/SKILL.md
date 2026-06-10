---
name: write-pr-description
description: Generate a structured, context-rich Pull Request description based on code diffs, issue context, and the project's standard PR template.
license: MIT
compatibility:
  - opencode
metadata:
  owner: integration-team
  package: git-writing
  triad_role: engineering
---

# Purpose
Transforms raw engineering output into a clear, structured PR description that explains the intent, architecture, and impact of a proposed change.

## Use This Skill When
- A feature branch is complete and ready for review.
- The user requests a PR description for the current working directory.
- A complex refactor needs a reviewer-facing summary before merge.

## Workflow
1. Ingest the problem context from an issue, PRD, or user prompt.
2. Review the code diff summary using `analyze-branch-diff` when needed.
3. Draft the problem and solution summaries at a high level.
4. Categorize changes and highlight the most important reviewer files.
5. Document testing steps, backward compatibility risks, and follow-up work.
6. Output the artifact in the standard PR Markdown template.

## Rules and Constraints
- Never treat the PR description as a changelog.
- Never narrate file-by-file implementation details.
- Always include a Reviewer Guide that separates core logic from mechanical updates.

## Validation
- Apply the 30-Second Rule.
- Ensure explicit pass/fail states for testing are documented.

## Output Expectations
A complete Markdown PR description formatted with Summary, Problem, Solution, Key Changes, Reviewer Guide, Testing, and Risks.
