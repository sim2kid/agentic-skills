---
name: write-release-notes
description: Synthesize aggregated pull requests and commit text into a structured, audience-aware release notes document following the 60-second rule.
license: MIT
compatibility:
  - opencode
metadata:
  owner: operations-team
  package: git-writing
  triad_role: operations
---

# Purpose
Transforms a collection of technical changes into an organized, scannable release notes document that highlights user features, bug fixes, breaking changes, and migration instructions.

## Use This Skill When
- A new release tag needs notes.
- A release readiness review needs documentation.
- Production deployment requires audience-facing change communication.

## Workflow
1. Ingest the raw aggregated release items via `git-release-aggregation`.
2. Draft a concise summary of the release theme and goals.
3. Group changes into standard categories.
4. Curate out purely internal modifications unless they affect behavior.
5. Extract operational prerequisites and place them in Upgrade Notes.
6. Compile the result into the standard release template.

## Rules and Constraints
- Never turn release notes into a git commit log.
- Never bury breaking changes.
- Focus on user-facing outcomes rather than internal mechanics.

## Validation
- Apply the 60-Second Rule.
- Verify every breaking change has a corresponding migration note.

## Output Expectations
A Markdown `RELEASE_NOTES.md` file ready for distribution.
