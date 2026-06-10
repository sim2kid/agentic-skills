---
name: agent-skill-curation
description: Curate sub-agent skills, agent capability design, skill selection. Use when defining which skills a sub-agent should load without making it overly broad or redundant.
license: MIT
compatibility:
  - opencode
metadata:
  owner: studio-authoring
  package: studio-authoring
  scope: authoring
---

# Agent Skill Curation

## Purpose
Select a small, coherent set of skills for a sub-agent so it can perform its role effectively without turning into a general-purpose agent.

## Use This Skill When
- A new sub-agent needs an initial skill set.
- An existing agent has accumulated too many or poorly matched skills.
- Package maintainers want to rationalize agent capability boundaries.

## Inputs
- The target sub-agent role and its defined responsibilities.
- Candidate skills and their intended usage.
- Adjacent agents and neighboring responsibilities.

## Workflow
1. Start from the agent's singular responsibility.
2. Identify the minimum set of skills needed to perform that responsibility well.
3. Remove skills that belong to adjacent roles or create overlap.
4. Prefer small composable skills over large umbrella skills.
5. Document why each retained skill belongs on the agent.

## Rules And Constraints
- Do not use skill lists to compensate for a vague agent role.
- Do not attach skills that pull the agent into another decision domain.
- Prefer shared foundational skills only when they support the role without changing its ownership.

## Validation
- The resulting skill set should strengthen the agent's specialization.
- The agent should still have clear boundaries after curation.

## Output Expectations
Produce a curated skill list with rationale and any recommended additions or removals.
