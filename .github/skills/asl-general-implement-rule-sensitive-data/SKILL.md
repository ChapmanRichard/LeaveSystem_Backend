---
name: asl-general-implement-rule-sensitive-data
description: "Use when: you need to constrain how sensitive information is handled in code, configuration, logs, and error responses. Keywords: sensitive information, masking, credential management, information leakage"
---

# Sensitive Information Standards Sub-Skill

## Intent
Prevent sensitive information leakage and ensure compliant handling of credentials and personal data.

## Input
- Changes involving configuration, logs, or error responses

## Rules
- Do not hardcode or output sensitive information in plaintext in code, logs, comments, or sample requests, such as passwords, keys, Tokens, connection strings, ID numbers, phone numbers, emails, and bank card numbers.
- Configuration items should preferably use environment variables or secure configuration centers. Do not commit real credentials to the repository; sample values must use masked placeholders such as `***` or `<SECRET>`.
- When returning errors externally, avoid leaking internal details such as SQL, stacks, paths, or framework versions. Internal diagnostic information should only remain in controlled logs.
- Reference standards: OWASP ASVS, OWASP Logging Cheat Sheet, and CWE-200 (Information Exposure).

## Minimum Validation Checklist
- [ ] No plaintext keys/Tokens/connection strings are committed.
- [ ] Sample and log data are masked.
- [ ] Error responses do not expose internal details such as SQL, stacks, or paths.
