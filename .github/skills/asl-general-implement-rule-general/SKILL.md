---
name: asl-general-implement-rule-general
description: "Use when: you need to apply general rules in API implementation, including documentation first, reuse first, and observability first. Keywords: general rules, documentation first, reuse first, observability"
---

# General Rules Sub-Skill

## Intent
Before implementing any API, ensure unified foundational rules are followed to reduce rework and behavioral drift.

## Input
- Requirements document path (default: `Spec/Req.md`)
- API design document path (default: `Spec/api_design.md`)

## Rules
- Documentation first: the single source of truth is `Req.md` + `api_design.md`. When documentation and code conflict, prefer the documentation and record compatibility handling.
- Reuse first: first reuse existing Models, DTOs, and infrastructure; add models or fields only when documentation explicitly requires them and existing models are insufficient.
- Observability first: failure causes must be locatable through error codes/messages, and key business actions must be traceable, with audit logs when necessary.


## Minimum Validation Checklist
- [ ] `Req.md` and `api_design.md` have been checked.
- [ ] Existing components have not been reimplemented.
- [ ] Failure paths can be located through error codes and logs.