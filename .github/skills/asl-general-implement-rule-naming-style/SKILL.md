---
name: asl-general-implement-rule-naming-style
description: "Use when: you need to constrain naming standards and code style consistency in API implementation. Keywords: naming standards, code style, readability, consistency"
---

# Naming Standards and Style Guard Sub-Skill

## Intent
Ensure new code has clear naming, consistent style, and minimized changes.

## Input
- Target changed file list

## Rules
- Project naming baseline: Controllers end with `Controller`; Commands end with `Command`; Queries end with `Query`; Handlers end with `CommandHandler/QueryHandler`; Service interfaces are prefixed with `I` and paired with implementations of the same name.
- API action naming follows “verb + business object”, such as `CreateLeaveApplication` and `ApproveLeave`; avoid vague abbreviations and names without business semantics.
- C# style guard: types/members use PascalCase, local variables use camelCase, private readonly fields use `_camelCase`. When historical code differs, keep new code locally consistent and unified within the same file.
- Single responsibility guard: Controllers only handle routing/authorization/protocol conversion and do not host business rules; complex flows are moved down to Services.
- Change minimization guard: do not perform unrelated renames or large-scale formatting; ensure the diff focuses on the target API.
- Reference standards: .NET Runtime Coding Style, Microsoft C# Coding Conventions, and Clean Code naming readability principles.

## Minimum Validation Checklist
- [ ] New symbol names comply with project conventions.
- [ ] No unrelated renames or large-scale formatting were introduced.
- [ ] Key action names directly communicate business meaning.
