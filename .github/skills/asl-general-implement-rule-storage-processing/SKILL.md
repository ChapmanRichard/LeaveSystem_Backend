---
name: asl-general-implement-rule-storage-processing
description: "Use when: you need to constrain data access, transactions, consistency, and query efficiency. Keywords: storage processing, transaction, consistency, pagination, indexes"
---

# Storage Processing Standards Sub-Skill

## Intent
Ensure data access is safe, transactions are complete, and queries are controllable.

## Input
- Entities and repositories involved in data access

## Rules
- Data access guard: access data uniformly through `UnitOfWorkLeaveSystem + LeaveSystemDBRepository<T>` or controlled repository extensions. Do not scatter raw SQL throughout the business layer.
- Transaction guard: atomic business operations such as multi-entity writes, state transitions, and quota deduction must run within transaction boundaries, reusing existing transaction decorator strategies to avoid partial commits.
- Consistency guard: use optimistic concurrency fields or conditional updates for concurrency-sensitive data to prevent “last write wins” overwrites that cause business disorder.
- Query guard: list APIs must have pagination limits, such as pageSize caps, to avoid full table scans; add necessary index recommendations according to access scenarios.
- Data minimization guard: read and return only fields required by the business to avoid over-fetching and sensitive field leakage.
- Lifecycle guard: clarify soft-delete/hard-delete strategy. Delete operations should preferably be auditable and traceable; historical data retention strategies must align with compliance requirements.
- Reference standards: ACID transaction principles, OWASP ASVS V9 (Data Protection), and CWE-770 (Allocation of Resources Without Limits or Throttling).


## Minimum Validation Checklist
- [ ] Data access paths comply with Repository/UoW conventions.
- [ ] Key write paths have transaction guarantees.
- [ ] List queries have pageSize limits.
