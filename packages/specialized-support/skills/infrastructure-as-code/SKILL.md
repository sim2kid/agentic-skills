---
name: infrastructure-as-code
description: Infrastructure as code, Docker, CI/CD, deployment automation. Use when repositories need declarative infrastructure, containerization, or automated pipelines for build and deployment.
license: MIT
compatibility:
  - opencode
metadata:
  owner: devops-team
  package: specialized-support
  track: specialized-support
---

# Infrastructure As Code

## Purpose
Turn runtime and deployment requirements into version-controlled infrastructure, container, and pipeline artifacts that can provision and deliver systems reliably.

## Use This Skill When
- A project needs Dockerfiles, compose files, or container runtime configuration.
- CI or CD automation must be added or updated.
- Cloud or self-hosted infrastructure requires declarative provisioning.

## Inputs
- TAD, system modeling artifacts, runtime dependencies, deployment targets, and security constraints.

## Workflow
1. Review the runtime and deployment requirements.
2. Draft containerization files using minimal, secure, and reproducible build patterns.
3. Draft declarative infrastructure templates appropriate to the hosting target.
4. Construct CI or CD workflows that lint, test, build, and deploy in the correct order.
5. Return the resulting configuration files in repository-ready structure.

## Rules And Constraints
- Never bake secrets or fixed environment-specific credentials into tracked artifacts.
- Prefer build once, deploy many patterns.
- Ensure runtime containers do not default to root where avoidable.

## Validation
- Pipelines must include a testing gate before deployment.
- Infrastructure and container files should reflect the declared runtime needs accurately.
- Deployment artifacts should support parity across environments.

## Output Expectations
Produce production-ready configuration files such as workflow definitions, Dockerfiles, compose manifests, or infrastructure templates.
