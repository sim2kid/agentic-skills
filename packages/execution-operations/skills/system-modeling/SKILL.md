---
name: system-modeling
description: ERD, UML, OpenAPI, AsyncAPI, schema design. Use when low-level system boundaries, data models, and service contracts must be specified before implementation.
license: MIT
compatibility:
  - opencode
metadata:
  owner: engineering-team
  package: execution-operations
  track: engineering
---

# System Modeling

## Purpose
Generate precise low-level technical specifications that describe data models, component interactions, and formal interface boundaries for implementation teams.

## Use This Skill When
- The Lead Engineer requests concrete module or service specifications.
- Persistent storage schemas must be defined or updated.
- Module, service, or event communication patterns require formal documentation.

## Inputs
- TAD, PRD, acceptance criteria, and existing architectural context.
- Naming conventions, schema standards, and interface constraints.

## Workflow
1. Parse the technical architecture context and extract concrete data and interaction needs.
2. Model the relevant entities, relationships, and constraints.
3. Draft standards-compliant OpenAPI, AsyncAPI, or equivalent formal contracts where needed.
4. Produce Mermaid or text diagrams for ERDs, dependency flows, or sequences.
5. Package the results in implementation-ready documentation.

## Rules And Constraints
- Every externally visible endpoint or interface must define failure responses, not only success paths.
- Avoid tight temporal coupling between otherwise separate domains.
- Keep names and structures aligned with project standards.

## Validation
- Schema naming should match project conventions.
- Formal specs should be lintable and structurally valid.
- Boundary ownership should be explicit across modules and services.

## Output Expectations
Produce a spec bundle containing schema definitions, formal interface contracts, and supporting diagrams.
