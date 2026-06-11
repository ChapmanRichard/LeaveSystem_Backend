---
name: asl-general-implement-rule-state-machine
description: "Use when: you need to implement or modify state-transition-related APIs and ensure state-machine consistency and idempotency. Keywords: state machine, state transition, idempotency, audit"
---

# State Machine Consistency Sub-Skill

## Intent
Implement state changes as explicit, verifiable, and auditable processes.

## Input
- Entity state definitions
- Allowed transition list

## Rules
- Strictly follow state transition rules. Every state transition must explicitly list allowed source states, target states, and triggering actions. “Implicit jumps” or “skipping intermediate states” are not allowed.
- Terminal states, such as approved, cancelled, and closed, should be treated as irreversible or only allow controlled rollback. Rollback rules must be explicitly documented.
- State changes must have idempotency or deduplication strategies to avoid repeated transitions caused by repeated clicks, retries, or message replay.
- State transitions are recommended to be expressed as a “state machine table / transition diagram / Guard conditions”. Complex flows may refer to workflow engines or domain state machine patterns.
- Illegal transitions return unified errors. Errors should be machine-readable and locatable; when necessary, return information in the `RFC 9457 Problem Details` style.
- State changes must be auditable. For key transitions, it is recommended to record the operator, time, old state, new state, and reason.
- Reference standards: State Machine Pattern, Workflow Engine Design, DDD Aggregate State Transition, and RFC 9457 (Problem Details for HTTP APIs).

## Minimum Validation Checklist
- [ ] The transition matrix is complete with no implicit jumps.
- [ ] Illegal transitions have unified error codes.
- [ ] Key state changes are auditable and traceable.
