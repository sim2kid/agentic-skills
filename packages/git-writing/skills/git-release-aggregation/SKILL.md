---
name: git-release-aggregation
description: Diff two git tags or branch heads to collect, parse, and filter intermediate pull requests and conventional commits for release note preparation.
license: MIT
compatibility:
  - opencode
metadata:
  owner: operations-team
  package: git-writing
  triad_role: operations
---

# Purpose
Provides refined textual input for release note authoring by collecting and filtering the raw engineering data between release markers.

## Use This Skill When
- Compiling changes since the last production tag.
- Analyzing a release candidate branch against the current main branch.

## Workflow
1. Identify the baseline and comparison targets.
2. Execute a git log or pull request history fetch between those boundaries.
3. Parse PR descriptions for user-focused summaries and breaking changes.
4. Filter out purely mechanical commits.
5. Categorize the payload by conventional commit prefixes or PR labels.
6. Pass the curated payload into the release notes workflow.

## Rules and Constraints
- Preserve metadata for breaking changes.
- Default to all changes since the initial repository commit if no prior tag exists.

## Validation
- Cross-verify that gathered PRs account for all merge commits in the source diff window.

## Output Expectations
A structured text summary or JSON payload sorting intermediate PRs and key commits by functional domain, stripped of stylistic and structural noise.
