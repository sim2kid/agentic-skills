---
name: deployment-rollback-planning
description: Rollback planning, deployment safety, release fallback. Use when pipelines or runtime changes need explicit rollback, recovery, and blast-radius control before release.
license: MIT
compatibility:
  - opencode
metadata:
  owner: devops-team
  package: specialized-support
  track: specialized-support
---

# Deployment Rollback Planning

## Purpose
Reduce release risk by defining explicit rollback paths, deployment sequencing, and recovery expectations before changes reach production.

## Use This Skill When
- A new deployment pipeline or runtime change is being prepared.
- A release modifies infrastructure, database dependencies, or critical services.
- Rollback and blast-radius concerns need to be documented.

## Inputs
- Deployment workflow, environment topology, stateful dependencies, and release constraints.

## Workflow
1. Identify the deployment units, order of operations, and failure points.
2. Define rollback triggers, rollback steps, and verification checks.
3. Highlight any irreversible or stateful risks that need mitigation.
4. Recommend safe rollout patterns such as staged release, canarying, or feature gating when appropriate.

## Rules And Constraints
- Do not assume manual operator heroics will handle rollback.
- Call out irreversible migrations or state changes explicitly.
- Keep rollback steps deterministic and environment-aware.

## Validation
- The plan should specify how to detect a bad deploy quickly.
- Recovery steps should be actionable by the operating team.

## Output Expectations
Produce a deployment safety and rollback plan with triggers, actions, and verification steps.
