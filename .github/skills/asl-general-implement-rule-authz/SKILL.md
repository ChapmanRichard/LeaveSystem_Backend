---
name: asl-general-implement-rule-authz
description: "Use when: you need to implement authentication and authorization rules covering both interface-level and resource-level authorization. Keywords: Authentication, Authorization, RBAC, least privilege"
---

# Authentication and Authorization Sub-Skill

## Intent
Ensure APIs have authentication and authorization boundaries to prevent unauthorized access.

## Input
- Target interface
- Role matrix source (default: `Req.md`)

## Rules
- APIs must satisfy both Authentication and Authorization; do not determine permissions solely by route reachability.
- Apply the Principle of Least Privilege and Deny by Default; access that is not explicitly authorized should be denied.
- Authorization must include at least two layers: interface-level access control + resource-level access control, such as whether the resource may be accessed and whether the action may be performed.
- The project-specific role decisions and authorization boundaries follow the “project business role conventions (from Req.md)”.
- Reference standards: OWASP ASVS V4 (Access Control), NIST SP 800-63 (Digital Identity), and RFC 9110 (HTTP authentication semantics).

## Minimum Validation Checklist
- [ ] Protected APIs have authentication constraints.
- [ ] Resource-level permission checks have been implemented.
- [ ] Unauthorized requests return stable error responses.