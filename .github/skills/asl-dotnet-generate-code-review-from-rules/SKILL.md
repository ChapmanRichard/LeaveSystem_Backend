---
name: asl-dotnet-generate-code-review-from-rules
description: "Use when: you need to perform a structured Code Review on a user-specified code scope, combine 12 governance rules for vulnerability identification, automatic fixes, and severity-based recommendation output, while also checking readability and potential bugs. Keywords: Code Review, security review, rule combination, automatic fix, severity classification, readability, potential bugs"
---

# Composable Code Review Skill

## Intent

This Skill converts a “user-specified code scope” into an executable, reviewable, and reusable Code Review workflow.

Applicable scenarios:
- Need to perform a fast but high-quality Code Review on one or more directories, modules, or files
- Need to cover governance requirements such as security, contracts, authorization, validation, state machines, storage, and logging in the same review round
- Need to output a review report grouped by severity, with sufficient rationale and clear actionable fix suggestions
- Need to attempt automatic fixes after identifying high-risk vulnerabilities, and provide fix results and residual risks

Expected outcomes:
- Improve review stability through rule-driven review and reduce “missed review” and “subjective fluctuation”
- Output a fix checklist that developers can directly execute, rather than generic descriptions
- Improve review efficiency in time-constrained scenarios while keeping high-risk issues prioritized

## Input

Required input:
- Code scope: files, directories, modules, or change sets, such as a PR diff
- Review goal: security-first, contract-first, performance-first, or comprehensive review

Optional input:
- Rule set selection: enable 12 rules by default, with optional enable/disable of sub-rules
- Whether automatic fixes are allowed: `true/false`
- Automatic-fix boundary: low-risk fixes only / include medium- and high-risk verifiable fixes
- Output granularity: brief version (issues and suggestions only) / standard version / strict version (including evidence and review steps)
- Scan depth: `quick`, `medium`, `thorough`

## Rule Set (Default 12 Rules)

This Skill combines the following latest sub-Skills by default as review rule sources, based on the workspace root `skills/` directory:

- General rules: `skills/asl-general-implement-rule-general/`
- Naming standards and style guard: `skills/asl-general-implement-rule-naming-style/`
- Authentication and authorization: `skills/asl-general-implement-rule-authz/`
- State machine consistency: `skills/asl-general-implement-rule-state-machine/`
- Data validation rules: `skills/asl-general-implement-rule-validation/`
- API contract consistency guard: `skills/asl-general-implement-rule-api-contract-consistency/`
- Service-layer processing standards: `skills/asl-general-implement-rule-service-processing/`
- Storage processing standards: `skills/asl-general-implement-rule-storage-processing/`
- Exception and logging standards: `skills/asl-general-implement-rule-exception-logging/`
- Sensitive information standards: `skills/asl-general-implement-rule-sensitive-data/`
- Component reference standards: `skills/asl-general-implement-rule-component-reference/`
- Vulnerability prevention standards: `skills/asl-general-implement-rule-vulnerability-prevention/`

Unified review constraints:
- Use “verifiable evidence” as the basis and avoid unsupported inferences.
- Report risks before fixes; report high-risk issues before medium- and low-risk issues.
- Every issue must include: location, triggering conditions, impact, evidence, and suggestion.
- Do not fabricate low-value issues for the sake of coverage.
- Readability checks must cover: naming expressiveness, function complexity, single responsibility, comment usefulness, duplicate code, and maintainability.
- Potential bug identification must cover: null/out-of-bounds issues, missing states, concurrency race conditions, missing exception branches, resource leaks, and timing and boundary-condition errors.
- For legacy system Enhancements, adjust the authorization rule “severity” to "Medium" and add risk notification and audit completion.

## Quality and Efficiency Targets (SLO)

- High-risk vulnerability identification target: 100%
- Code Review time reduction target: >50%, compared with a manual file-by-file review baseline of the same scope
- Automatic fix success rate target after discovering high-risk issues: >=90%

Notes:
- These metrics are Target SLOs and must be recorded and reviewed during actual execution.
- If the current round cannot meet targets, the output must include “reason for not meeting target + remediation suggestions + next-round improvement actions”.

## Severity Definition and Determination

- `Critical`: can directly cause data leakage, remote execution, bulk privilege escalation, or core transaction destruction
- `High`: can cause permission bypass, sensitive information leakage, state machine destruction, or severe injection risk
- `Medium`: can cause business exceptions, distorted error handling, or exploitable issues with many prerequisites
- `Low`: standards/maintainability issues with limited impact on security and correctness
- `Info`: optimization suggestions or potential improvement points

Determination principles:
- Prefer assessment by “impact scope × exploitability × recoverability”
- Security issues have higher priority than style issues
- Issues of the same level are sorted by fix cost and regression risk

## Automatic Fix Strategy

When `Critical/High` issues are found, execute the following strategy:

1. Automatically fixable scenarios (priority)
- Clear and low-ambiguity fixes, such as parameterized queries, completing missing authorization checks, completing input boundary validation, masking sensitive logs, and constraining dangerous default configuration.

2. Automatic fix execution requirements
- Each fix handles only one type of risk to avoid mixed changes.
- After fixing, minimum validation must be executed: compilation/static checks/related tests.
- Output behavior differences before and after the fix, plus regression risks.

3. Automatic fix failure handling
- If automatic fixing is impossible or validation fails after fixing, the output must include:
  - Failure reason, such as unclear semantics, missing test guardrails, or excessive cross-module impact
  - Manual fix suggestions, including priority and steps
  - Temporary mitigation measures, such as access restriction, configuration switches, and alert hardening

## Workflow

1. Scope identification
- Parse the code scope and review goal from user input.
- Build the review checklist, including files, symbols, APIs, and critical paths.

2. Rule loading
- Load review dimensions according to the default 12 rules.
- If the user specifies a rule subset, override the default configuration according to input.

3. Risk scanning and evidence collection
- Scan each rule and locate violations.
- Bind evidence to every issue, such as code snippets, call chains, configuration items, and behavior paths.
- Run readability scanning and potential bug scanning in parallel, and output reproducible conditions or triggering prerequisites.

4. Severity classification and deduplication
- Classify issues as Critical/High/Medium/Low/Info.
- Merge duplicate issues and retain the most representative evidence and fix path.

5. Automatic fix and validation (optional)
- Execute minimal changes for automatically fixable items.
- Run compilation and related tests, and record success rate and failure reasons.

6. Report generation
- Output the risk list first, then suggestions and the fix plan.
- Clearly state target achievement, including identification rate, time improvement, and automatic fix success rate.

## Reference Cases

### Case 1: Review a specified Controller scope
Goal: review the security and contract consistency of a Controller and its direct call chain.

Recommended coverage:
- Missing authorization and missing resource-level permissions
- Input binding and validation omissions
- Return status codes/error codes inconsistent with contract
- Whether exception handling and logs leak internal details

### Case 2: Review a specified Service scope
Goal: review business orchestration, state transitions, and idempotent consistency.

Recommended coverage:
- Illegal state transitions
- Duplicate submissions/concurrent writes
- Incomplete transaction boundaries
- Missing compensation or rollback after external dependency failures

### Case 3: Review a specified Repository/data access scope
Goal: review storage access security and query performance boundaries.

Recommended coverage:
- SQL injection/concatenation risks
- Missing pagination cap causing resource exhaustion
- N+1 queries and over-fetching
- Excessive return of sensitive fields

## Output Checklist (Required)

1. Review scope
- Input scope, actual coverage, exclusions

2. Issue list (descending severity)
- Each item includes: level, location, issue description, triggering conditions, impact scope, evidence, fix suggestion, fix priority, issue type (security/contract/readability/potential bug)

3. Automatic fix results
- Automatically fixed items, validation results, success rate
- Items not automatically fixed and reasons

4. Target achievement
- High-risk vulnerability identification rate
- Review time reduction ratio
- Automatic fix success rate
- Unmet targets and improvement suggestions

5. Regression and release recommendations
- Tests that need to be added
- Recommended canary/rollback strategy

## Review Result Presentation Standards (For Users)

Results must be “clear, well-reasoned, and recommendation-oriented by severity”, in the following format:

1. `Critical` and `High` (list first)
- Summarize business/security impact in one sentence
- Provide evidence location and triggering path
- Provide executable fix plan and priority (P0/P1)

2. `Medium` and `Low`
- Explain why it is not high-risk
- Provide improvement suggestions and implementation order (P2/P3)
- For readability issues, provide refactoring suggestions, such as naming, splitting functions, extracting shared logic, and expected benefits.

3. `Info`
- Keep only high-value optimization items to avoid noise

4. Conclusion summary
- Whether to recommend blocking merge
- Required fixes and deferrable items
- Next actions and responsibility suggestions

## Validation Checklist

Check each item before submission:

- [ ] Review has been completed according to the user-specified scope, and scope mapping is clear.
- [ ] The 12 rules are loaded, or the enabled subset is explicitly stated.
- [ ] All issues have evidence and impact explanation, with no vague conclusions.
- [ ] Issues are sorted by severity and include priority recommendations.
- [ ] Readability check has been completed and executable improvement suggestions are output.
- [ ] Potential bugs have been identified with triggering conditions and validation suggestions.
- [ ] High-risk issues have been attempted for automatic fixes, with validation results provided.
- [ ] If automatic fixing failed, manual fix suggestions and temporary mitigation plans are provided.
- [ ] The report includes target achievement and improvement suggestions for unmet targets.
- [ ] The output structure is clear and can be used directly for review meetings or remediation tracking.