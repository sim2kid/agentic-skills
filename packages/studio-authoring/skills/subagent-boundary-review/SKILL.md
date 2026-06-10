---
name: subagent-boundary-review
description: Review sub-agent scope, role boundaries, collaboration design. Use when a proposed OpenCode sub-agent needs review for scope creep, role overlap, weak handoffs, or embedded procedural overload.
license: MIT
compatibility:
  - opencode
metadata:
  owner: studio-authoring
  package: studio-authoring
  scope: authoring
---

# Sub-Agent Boundary Review

## Purpose
Evaluate whether a proposed sub-agent is appropriately specialized, bounded, and collaboration-ready within a multi-agent system.

## Use This Skill When
- A new sub-agent draft needs review before inclusion.
- An existing agent has become too broad or inconsistent.
- Multiple agents appear to overlap in ownership.

## Inputs
- The proposed `AGENT.md`.
- Related package context and adjacent agent roles if available.

## Workflow
1. Review the role and purpose for single-responsibility discipline.
2. Check whether the inputs and outputs are explicit.
3. Review available skills for over-broad curation.
4. Evaluate boundaries and non-responsibilities for clarity.
5. Check collaboration and completion sections for handoff quality.
6. Flag embedded procedure that should be moved into skills.
7. Return actionable refinement guidance.

## Rules And Constraints
- Treat unclear ownership as a serious design defect.
- Flag agents that attempt to own an entire workflow rather than a decision domain.
- Flag prompts that embed too much procedural detail better suited to skills.

## Validation
- The review should make clear whether the agent is safe to add as written.
- Findings should be tied to specific structural weaknesses.
- The result should clarify whether the fix is scope reduction, skill extraction, or collaboration redesign.

## Output Expectations
Produce a structured sub-agent review with findings, recommendations, and overall readiness.
