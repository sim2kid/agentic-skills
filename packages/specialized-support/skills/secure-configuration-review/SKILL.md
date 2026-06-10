---
name: secure-configuration-review
description: Secure config review, IAM review, exposure audit. Use when infrastructure, deployment, or application configuration needs review for insecure defaults, over-permission, or network exposure.
license: MIT
compatibility:
  - opencode
metadata:
  owner: security-team
  package: specialized-support
  track: specialized-support
---

# Secure Configuration Review

## Purpose
Identify risky configuration defaults before they become exploitable runtime behavior.

## Use This Skill When
- Infrastructure manifests, CI or CD files, or runtime configs are added or changed.
- IAM roles, port exposure, TLS settings, or auth-related configuration need review.
- A deployment path must be evaluated for security posture.

## Inputs
- Infrastructure files, container configs, pipeline definitions, env config rules, and runtime policies.

## Workflow
1. Review network exposure, permissions, trust relationships, and crypto-related settings.
2. Identify insecure defaults, broad privileges, unnecessary exposure, or weak operational safeguards.
3. Document safer configuration expectations and required remediation.

## Rules And Constraints
- Treat public exposure and over-privilege as first-class risks.
- Prefer least privilege, encrypted transport, and explicit allowlists.
- Do not assume internal networks are safe by default.

## Validation
- Findings should tie back to specific configuration locations and risks.
- Remediation guidance should be concrete enough for DevOps or Engineering to implement.

## Output Expectations
Produce a secure configuration review with categorized findings and hardening guidance.
