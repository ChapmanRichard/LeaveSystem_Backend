---
name: asl-general-implement-rule-validation
description: "Use when: you need to implement validation for input parameters and business preconditions, ensuring API input is safe and errors are machine-readable. Keywords: data validation, Validator, boundary validation"
---

# Data Validation Rules Sub-Skill

## Intent
Establish a layered validation mechanism to ensure inputs are reliable and errors are diagnosable.

## Input
- Request model
- Business preconditions

## Rules
- Parameter completeness, including required fields, length, chronological order, and format, is implemented in a Validator or equivalent validation layer and should be completed before entering core business logic.
- Business validation, such as quota, unauthorized access, resource ownership, and state preconditions, is performed again in the Service layer to avoid relying only on the frontend.
- Apply whitelist and boundary validation to enums, pagination, time ranges, and numeric ranges, and reject ambiguous inputs caused by implicit type conversion.
- Unified error expression: validation failures should return stable, machine-readable error codes and field-level error information.
- Reference standards: OWASP ASVS V5 (Validation, Sanitization and Encoding), RFC 9457 (Problem Details for HTTP APIs), and JSON Schema validation practices.

## Minimum Validation Checklist
- [ ] Both parameter and business validation are covered.
- [ ] Boundary-value and illegal-value paths have been considered.
- [ ] Validation error responses are machine-readable and structurally stable.