---
name: asl-general-implement-rule-exception-logging
description: "Use when: you need to unify layered exception handling and logging strategy, ensuring issues are traceable without leaking sensitive information. Keywords: exception handling, logging standards, observability, audit"
---

# Exception and Logging Standards Sub-Skill

## Intent
Standardize exception semantics and logging behavior to improve troubleshooting and auditability.

## Input
- Target interface or service
- Key exception paths

## Rules
- Exception layering guard: verify that exceptions use `ValidationException` or domain-explicit exceptions; use recognizable exception types for business conflicts; uniformly convert unknown exceptions into generic error responses to avoid returning stacks to clients.
- Minimum-sufficient logging guard: record `traceId/requestId`, interface name, command/query name, and key context; do not record sensitive plaintext. Distinguish log levels as `Debug/Info/Error`.
- Record user identifiers from existing Session or Cookie data for correlated logging, and avoid directly recording sensitive user information in logs.
- Unified handling guard: preferably reuse existing decorator chains, such as `ErrorLogCommandHandlerDecorator` and `ErrorLogQueryHandlerDecorator`, and the unified error outlet in `WebMemoryCommandBus`; avoid repeated try-catch blocks in each Controller.
- Observability guard: add searchable log keywords for critical failure paths, such as interface identifiers, entity primary keys, and before/after state values.
- Audit guard: high-risk operations such as approval and quota adjustment should record operator, time, object, action, and result.
- Reference standards: OWASP Logging Cheat Sheet, SRE observability practices, and RFC 5424 (log level semantics).

## Minimum Validation Checklist
- [ ] Exception layers are clear, and no stack is leaked to clients.
- [ ] Log levels and context information are reasonable.
- [ ] Audit fields for key operations are complete.