---
name: asl-general-implement-rule-api-contract-consistency
description: "Use when: you need to ensure consistency between API implementation and API design documentation in methods, paths, fields, and error codes. Keywords: API contract, consistency, OpenAPI, compatibility"
---

# API Contract Consistency Guard Sub-Skill

## Intent
Ensure the implementation aligns with the API contract and control backward compatibility risks.

## Input
- API design document path (default: `Spec/api_design.md`)
- Target interface identifier

## Rules
- Contract source guard: the implementation must align with the Method, Path, request fields, response structure, and error codes in `api_design.md`. If full alignment is impossible because of historical compatibility, record deviations and migration recommendations in the output.
- Field semantics guard: request/response field names, nullability, enum value ranges, and default-value semantics must be consistent. Do not allow “same field name with different meanings”.
- Version compatibility guard: avoid breaking changes to published APIs, such as field deletion, type narrowing, or semantic inversion. When necessary, use backward-compatible strategies such as adding optional fields or keeping old fields during transition.
- State-machine contract guard: state transitions and error codes must be stable; key code values depended on by the frontend must not be changed casually.
- Example consistency guard: sample requests and responses in the documentation should be reproducible by the current implementation. At least one success sample and one failure sample must run successfully.
- Reference standards: OpenAPI Specification, RFC 9110 (HTTP Semantics), and Postel's Law (be liberal in what you accept and conservative in what you send).

## Minimum Validation Checklist
- [ ] Implementation and documentation are aligned item by item.
- [ ] Contract deviations are recorded with migration recommendations.
- [ ] Sample requests/responses are reproducible.
