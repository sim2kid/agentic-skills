---
name: secret-handling-and-runtime-config
description: Secret management, runtime config, environment injection. Use when infrastructure or applications need secure secret delivery and safe environment-specific configuration rules.
license: MIT
compatibility:
  - opencode
metadata:
  owner: devops-team
  package: specialized-support
  track: specialized-support
---

# Secret Handling And Runtime Config

## Purpose
Ensure secrets and environment-specific settings are delivered securely at runtime without leaking into source control, images, or build artifacts.

## Use This Skill When
- A service needs environment variables, credentials, tokens, or certificates.
- Deployment pipelines or runtime environments need secure configuration rules.
- Existing infra work risks unsafe secret handling.

## Inputs
- Deployment topology, application config requirements, and security constraints.

## Workflow
1. Identify which values are secrets versus normal configuration.
2. Define where each class of config should live and how it is injected.
3. Ensure build artifacts remain secret-free.
4. Document rotation, scope, and least-privilege expectations for runtime credentials.

## Rules And Constraints
- Never place secrets in tracked files, container images, or example configs without explicit placeholder treatment.
- Prefer short-lived, scoped credentials where supported.
- Separate secret concerns from non-sensitive runtime config.

## Validation
- The resulting approach should keep source control and build outputs free of live credentials.
- Runtime config rules should be understandable by operators and developers.

## Output Expectations
Produce a secure runtime configuration plan with secret boundaries, injection paths, and handling rules.
