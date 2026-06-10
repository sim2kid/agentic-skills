---
name: bottleneck-risk-mitigation
description: Bottleneck mitigation, deadlock resolution, stale workflow recovery. Use when work stalls due to blocked reviews, triad deadlocks, or repeated unresolved discussion loops.
license: MIT
compatibility:
  - opencode
metadata:
  owner: project-management
  package: execution-operations
  track: operations
---

# Bottleneck Risk Mitigation

## Purpose
Detect and break workflow stalls before they turn into repeated agent loops, stale tasks, or uncontrolled delivery drag.

## Use This Skill When
- A task is stuck in review or discussion for multiple cycles.
- Triad participants are repeating incompatible positions without resolution.
- A work item is blocked by missing information or ownership drift.

## Inputs
- Blocked task state, artifact history, ownership status, and discussion context.

## Workflow
1. Identify the exact blocker and the owner of the unresolved dependency.
2. Determine whether the issue is missing information, role conflict, or true decision deadlock.
3. Apply the smallest effective intervention:
   - route missing artifacts
   - trigger triad negotiation
   - reduce the decision to a minimal option set for the user
4. Update task status and mitigation ownership clearly.

## Rules And Constraints
- Do not allow repeated circular discussion to continue indefinitely.
- Do not resolve cross-domain conflicts by silently rewriting requirements.
- Escalate to the user when the system cannot produce a legitimate autonomous decision.

## Validation
- The task should exit a stale blocked state and move to an owned next step.
- The intervention should be explicit and traceable.

## Output Expectations
Produce an operational resolution summary with blocker, mitigation, owner, and updated status.
