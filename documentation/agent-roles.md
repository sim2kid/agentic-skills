# Sub-Agent Roles & Skills

This document details the specific roles, key artifacts, and reporting structures for each sub-agent.

## The Core Triad

### 1. Product Manager
- **Role**: Defines "What" and "Why". Sets design standards and overarching product vision.
- **Key Artifacts**: Product Vision, Feature Roadmap, Acceptance Criteria.
- **Reporting**: Collaborates with Lead Engineer and Lead UX/UI Designer in "The Triad".

### 2. Lead Engineer
- **Role**: Defines "How" at a high level. Establishes technical architecture and quality standards.
- **Key Artifacts**: Technical Architecture Document (TAD), System Requirements (SRS).
- **Constraints**: Restricted from UX Design to maintain focus on technical structure.

### 3. Lead UX/UI Designer
- **Role**: Defines the "Feel". Reviews design and advises on user experience and interface flow.
- **Key Artifacts**: UX Review Report, Interaction Flow Maps, UI Style Guides.

## Execution & Operations

### 4. Systems Architect
- **Role**: Designs modular components and clean system sets.
- **Key Artifacts**: Component Diagrams, Interface Definitions, Handoff Summaries.
- **Reporting**: Reports to the **Lead Engineer**.

### 5. Senior Software Engineer
- **Role**: Focuses on implementation, code quality, and localized logic.
- **Key Artifacts**: Implemented Code, Pull Request Descriptions.
- **Reporting**: Executes based on **Systems Architect** designs.

### 6. QA Lead
- **Role**: Manages quality assurance, bug triaging, and test validation.
- **Key Artifacts**: Bug Reports, Test Plans, Rejection Reports.
- **Collaboration**: Sends structured rejections back to **Senior Software Engineer** if criteria aren't met.

### 7. Lead Producer
- **Role**: The "Orchestrator". Routes workflows and unblocks the team.
- **Key Artifacts**: Task Assignments, Workflow Status Updates.

## Specialized Support

### 8. DevOps Specialist
- **Role**: Focuses on deployment, environment configuration, and scalability.
- **Key Artifacts**: Docker Configurations, Deployment Scripts, CI/CD Pipelines.

### 9. Security Auditor
- **Role**: Reviews architecture and dependencies for vulnerabilities and license compliance.
- **Key Artifacts**: Security Audit Report, Compliance Checklist.
