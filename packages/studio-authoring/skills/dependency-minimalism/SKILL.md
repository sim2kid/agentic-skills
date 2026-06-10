---
name: dependency-minimalism
description: Review package and agent dependencies to ensure only actually needed skills and packages are required. Use when deciding whether a package should depend on studio authoring artifacts or any other shared package.
license: MIT
compatibility:
  - opencode
metadata:
  owner: studio-authoring
  package: studio-authoring
  scope: authoring
---

# Dependency Minimalism

## Purpose
Keep package dependency graphs minimal by requiring only the skills and packages that are truly needed for an agent to do its job.

## Use This Skill When
- Reviewing whether a package should depend on another package.
- Deciding if a skill belongs in a package or should stay out of the dependency graph.
- Checking whether an agent or package is depending on authoring tools it does not actually use.

## Inputs
- The package or agent definition being reviewed.
- The declared dependencies for that package.
- The skills or capabilities the agents in the package actually use.

## Workflow
1. List the concrete responsibilities of the package or agent.
2. Map each declared dependency to a specific required capability.
3. Remove dependencies that only support optional maintenance, convenience, or future work.
4. Treat package-level dependencies as runtime or direct-use requirements, not organizational preference.
5. Prefer keeping authoring and maintenance tools out of downstream package dependency graphs unless they are directly exercised.
6. Return the smallest dependency set that still fully supports the package's actual behavior.

## Rules And Constraints
- If agents in a package do not use the skills from another package, the package should not depend on that package.
- Do not justify dependencies with general usefulness, future reuse, or repository convenience.
- Distinguish actual operational needs from build-time or maintenance-time needs.

## Validation
- Every dependency should be traceable to a concrete use.
- No dependency should remain solely because it is part of the authoring toolchain.
- The final dependency list should be the minimum set that preserves required behavior.

## Output Expectations
Produce a dependency recommendation that clearly identifies which packages stay, which packages are removed, and why each remaining dependency is necessary.
