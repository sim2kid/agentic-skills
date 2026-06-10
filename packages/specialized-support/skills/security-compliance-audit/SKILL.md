---
name: security-compliance-audit
description: Security audit, compliance review, CVE scan, OWASP review. Use when code, dependencies, or infrastructure changes must be checked for vulnerabilities, insecure patterns, and policy violations.
license: MIT
compatibility:
  - opencode
metadata:
  owner: security-team
  package: specialized-support
  track: specialized-support
---

# Security Compliance Audit

## Purpose
Review code, configuration, dependencies, and infrastructure for exploitable vulnerabilities, insecure defaults, and compliance violations before release.

## Use This Skill When
- A pull request or feature change touches auth, payments, sensitive data, or privileged operations.
- New dependencies are introduced.
- Infrastructure or deployment configuration changes need security scrutiny.
- Scheduled vulnerability or compliance sweeps are required.

## Inputs
- Diffs, dependency manifests, config files, infrastructure files, and applicable policy context.

## Workflow
1. Review the target changes for OWASP-style vulnerability patterns.
2. Check dependencies for known vulnerabilities and risky license posture.
3. Audit infrastructure and runtime config for exposure, over-permission, and weak cryptography.
4. Categorize findings by severity and provide mitigation guidance.

## Rules And Constraints
- Do not fix the implementation directly as part of this audit role.
- Hardcoded credentials are always critical blockers.
- Findings must be actionable, not vague.

## Validation
- Every reported issue should include exact location and remediation direction when possible.
- Severity should reflect exploitability and impact, not fear.

## Output Expectations
Produce a security audit report with pass-fail status, categorized findings, and remediation instructions.
