---
name: asl-general-implement-rule-service-processing
description: "Use when: you need to constrain service-layer business orchestration, exception semantics, and idempotent consistency. Keywords: service layer, business orchestration, idempotency, consistency"
---

# Service-Layer Processing Standards Sub-Skill

## Intent
Ensure the service layer carries the business core while maintaining clear boundaries.

## Input
- Service-layer change scope
- Related commands/queries

## Rules
- The service layer is the core of business orchestration. It is responsible for business rules, state transitions, and cross-entity consistency; do not place core business logic in Controllers.
- The service layer should be decoupled through interfaces and reuse existing call chains (`IWebCommandBus -> ICommandHandler -> Service -> UnitOfWork/Repository`) while preserving one-way dependencies.
- The service layer must ensure idempotency and consistency: define idempotency keys or deduplication strategies for key operations, and ensure transaction boundaries for multi-step writes.
- Service-layer exceptions should be semantic, such as validation failures, business conflicts, and resource-not-found cases, and should be handled by the unified error outlet to avoid swallowing exceptions.
- The service layer should avoid N+1 queries and unnecessary large object loading, following the principle of “minimum reads, minimum writes”.
- Reference standards: DDD Application Service practices, Clean Architecture Use Case Boundary, and the 12-Factor Backing Services principle.

## Minimum Validation Checklist
- [ ] Controllers do not carry core business logic.
- [ ] Service-layer exception semantics are clear and can be uniformly handled.
- [ ] Multi-step business operations have consistency guarantees.
