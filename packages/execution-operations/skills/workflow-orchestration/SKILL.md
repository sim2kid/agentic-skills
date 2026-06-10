---
name: workflow-orchestration
description: Workflow orchestration, backlog routing, dependency coordination. Use when multi-agent work needs assignments, sequencing, and artifact handoffs managed explicitly.
license: MIT
compatibility:
  - opencode
metadata:
  owner: project-management
  package: execution-operations
  track: operations
---

# Workflow Orchestration

## Purpose
Maintain continuous flow across the studio by assigning work clearly, sequencing dependencies correctly, and routing artifacts to the right downstream agents.

## Use This Skill When
- A new feature or release effort is starting.
- An upstream artifact is complete and needs routing.
- A backlog or board needs active coordination.

## Inputs
- Work items, dependencies, priorities, statuses, and completed artifacts.

## Workflow
1. Review the current critical path and active tasks.
2. Identify blockers, missing prerequisites, and next available work.
3. Route completed artifacts to the appropriate downstream owners.
4. Update priorities and statuses to reflect current reality.
5. Return an explicit work board state.

## Rules And Constraints
- Do not assign implementation work before prerequisite architecture or requirement artifacts exist.
- Do not leave blocked or unassigned work without a named mitigation path.
- Keep routing aligned with role boundaries.

## Validation
- Every active task should have an owner and dependency state.
- No downstream task should begin on invalid or incomplete upstream artifacts.

## Output Expectations
Produce an updated work board with owners, priorities, dependencies, and next actions.
