---
name: analyze-branch-diff
description: Compare two git references to extract the net changes, categorizing structural shifts, mechanical updates, and architectural impact.
license: MIT
compatibility:
  - opencode
metadata:
  owner: integration-team
  package: git-writing
  triad_role: engineering
---

# Purpose
Provides the context needed to write a PR description by analyzing the raw delta between branches and filtering out noise.

## Use This Skill When
- Preparing to write a PR description and needing to understand the exact scope of changes.
- The user asks what changed between their branch and a base branch.
- A code review needs a high-level summary of impacted systems before diving into the code.

## Workflow
1. Identify the target base branch and current source branch.
2. Execute a git diff between the two heads.
3. Filter out noise such as lockfiles, binaries, and purely formatting changes.
4. Categorize remaining diffs into architectural, mechanical, infrastructure, and testing buckets.
5. Identify the center of gravity of the PR.

## Rules and Constraints
- Do not output raw diff lines.
- Flag database schema changes and external API contracts as high impact.

## Validation
- Verify the summary accounts for all major directories modified in the diff.
- Ensure the analysis separates intent from syntax.

## Output Expectations
A structured Diff Analysis Report categorizing changes by impact, listing key files, and summarizing the net delta between the two branches.
