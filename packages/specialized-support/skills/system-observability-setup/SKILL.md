---
name: system-observability-setup
description: Observability, structured logging, telemetry, alerting. Use when deployed systems need logs, metrics, health checks, and alerts for reliable operation and debugging.
license: MIT
compatibility:
  - opencode
metadata:
  owner: devops-team
  package: specialized-support
  track: specialized-support
---

# System Observability Setup

## Purpose
Define the logging, health, telemetry, and alerting baseline needed to operate and debug a deployed system safely.

## Use This Skill When
- A service is being prepared for production or staging.
- Engineering or QA needs structured runtime signals.
- Performance or reliability monitoring must be added.

## Inputs
- Runtime architecture, service topology, error modes, and operational requirements.

## Workflow
1. Identify the application's runtime surfaces and standard output behavior.
2. Define structured logging expectations and log-safe fields.
3. Specify lightweight health checks and readiness signals.
4. Define metrics, dashboards, and alert thresholds appropriate to the system.
5. Return the observability configuration and guidance artifacts.

## Rules And Constraints
- Logs must not include PII, credentials, or plaintext tokens.
- Health checks must be lightweight and safe under load.
- Alerting should be actionable rather than noisy.

## Validation
- Logging formats should be parsable by standard aggregators.
- Health endpoints and telemetry should align with deployment needs.

## Output Expectations
Produce telemetry configuration, health-check definitions, and alerting or dashboard rules.
