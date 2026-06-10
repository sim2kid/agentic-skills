---
name: threat-modeling
description: Threat modeling, STRIDE, architecture security review. Use when TADs, APIs, or data flows need structured pre-implementation security analysis.
license: MIT
compatibility:
  - opencode
metadata:
  owner: security-team
  package: specialized-support
  track: specialized-support
---

# Threat Modeling

## Purpose
Catch architectural security flaws early by applying a structured threat analysis process to system boundaries, trust zones, and data flows.

## Use This Skill When
- A new TAD, API design, or data model is created.
- Sensitive user data or privileged workflows are introduced.
- Cross-system or cross-service data flow is being defined.

## Inputs
- Architecture diagrams, API contracts, schemas, and trust-boundary context.

## Workflow
1. Ingest the architecture and identify key assets, actors, and trust boundaries.
2. Apply the STRIDE lens to major flows and interfaces.
3. Identify practical mitigations for each credible threat.
4. Return the threat model as an addendum to the architecture.

## Rules And Constraints
- Do not block on abstract risk without a concrete mitigation path.
- Focus especially on data ingress, cross-boundary communication, and privilege changes.
- Make mitigations implementable by architecture or engineering teams.

## Validation
- Every identified threat should have a corresponding mitigation requirement.
- The model should cover both direct abuse and operational misuse paths.

## Output Expectations
Produce a threat model addendum with threats, affected boundaries, and required defenses.
