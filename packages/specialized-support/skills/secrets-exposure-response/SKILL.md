---
name: secrets-exposure-response
description: Secret exposure, credential leak triage, incident response. Use when hardcoded credentials, leaked tokens, or unsafe secret handling paths are discovered in code or configuration.
license: MIT
compatibility:
  - opencode
metadata:
  owner: security-team
  package: specialized-support
  track: specialized-support
---

# Secrets Exposure Response

## Purpose
Provide an immediate, structured response path when credentials or secret material are exposed in code, config, logs, or build artifacts.

## Use This Skill When
- Hardcoded credentials are discovered.
- Tokens, certificates, or secrets appear in logs, diffs, or tracked files.
- Unsafe secret handling creates an incident-level risk.

## Inputs
- Evidence of the exposure, file locations, affected systems, and current credential scope.

## Workflow
1. Confirm the exposed secret type and where it appears.
2. Classify the exposure scope and severity.
3. Recommend immediate containment steps such as revoke, rotate, remove, and audit downstream use.
4. Document longer-term prevention steps for engineering and DevOps.

## Rules And Constraints
- Treat real credential exposure as time-sensitive and high priority.
- Do not suggest leaving exposed secrets in place temporarily without explicit risk acceptance.
- Separate containment from root-cause prevention.

## Validation
- The response should identify both immediate containment and follow-up remediation.
- Impacted systems and credentials should be named as precisely as the evidence allows.

## Output Expectations
Produce a secrets exposure response note with severity, containment actions, remediation steps, and affected surfaces.
