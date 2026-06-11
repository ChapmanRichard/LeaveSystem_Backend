---
name: asl-dotnet-generate-webapi-tests
description: "Use when: you need to automatically generate unit tests, API tests, or database integration tests from existing WebAPI code and requirements documents, focusing on API contracts, Mock standards, database state cleanup, and repeatable execution. Keywords: WebAPI, unit tests, integration tests, Mock, API contract, Database-State-Cleanup, Req.md"
---

# WebAPI Test Generation Skill

## Intent

This Skill converts existing WebAPI code, requirements documents, and layered implementations into executable, maintainable, and repeatable test code.

Applicable scenarios:
- Need to automatically generate unit tests, controller tests, service tests, or database integration tests from existing WebAPI code
- Need to verify interface behavior against requirements documents without binding test logic to one specific requirement number or fixed API
- Need to complete testing standards, Mock standards, contract validation, and database state cleanup strategy

Expected outcomes:
- Generate high-value tests based on existing code structure, rather than only superficial assertions
- Prioritize coverage of business rules, API contracts, exception paths, and data isolation
- Generated tests are repeatable, do not depend on execution order, and do not contaminate database state

## Input

Required input:
- Requirements document path or conventional document path, such as `Spec/Req.md`
- Target WebAPI code scope, such as a Controller, Service, QueryHandler, CommandHandler, or Repository

Optional input:
- Target test type: `unit`, `controller`, `integration`, or mixed mode
- Expected test framework: reuse the repository's established framework if it exists; otherwise choose the minimum viable option according to current repository state
- Whether adding a test project, test toolkit, and test fixtures is allowed
- Whether database integration tests and cleanup strategy are required

## Rules

1. Basic testing standards
- Each test verifies only one primary behavior and avoids covering too many branches in one test.
- Test names must express scenario, condition, and result so failures can be located easily.
- Assertions should preferably check business results, state changes, and call constraints rather than meaningless implementation details.
- Tests must be repeatable; do not depend on execution order, global static state, or random values.
- All time, random numbers, IDs, and external responses must be controllable or replaceable.
- Test data should be minimized and keep only fields required to trigger the target behavior.

2. Web API specific rules
- Prefer validating routes, HTTP Method, input binding, response structure, status codes, and error codes.
- Cover combination constraints for Header, Query, Route, and Body separately.
- Cover success and failure paths for authorization, roles, resource ownership, idempotency, and state transitions.
- Controller tests focus on contract and orchestration and do not put core business logic into Controller assertions.
- Assertions for return values must distinguish successful responses, business errors, validation errors, and system exceptions.

3. API contract validation guard
- During test generation, current WebAPI method, path, request fields, response fields, status codes, and error codes must be checked.
- If requirements documents and existing code differ, tests should explicitly identify compatible behavior rather than ignoring it by default.
- Whenever the contract defines field type, requiredness, enum, range, format, or default value, corresponding tests must be generated.
- For pagination, sorting, filtering, search, batch operations, and similar APIs, cover boundary values and illegal values.

4. External service integration Mock standards
- All external dependencies must be isolated. Unit tests must not access real networks, message queues, third-party APIs, or shared external systems.
- Prefer mocking interfaces, adapters, HttpClient wrappers, or port abstractions; do not directly mock business core classes.
- Mocks simulate behavior only and must not turn tests into implementation copies.
- Explicitly verify interaction counts, key arguments, and failure branches for external dependencies.
- When an external dependency returns errors, timeouts, or empty results, cover each with separate tests.
- When generating tests for new features, directly mocking complex business classes from legacy systems is strictly prohibited. Extract interfaces or adapters first, then mock those adapters.

5. Database integration test state cleanup (Database-State-Cleanup)
- Database integration tests must have clear pre-initialization and post-cleanup.
- Before each test run, data isolation should be ensured; after each run, state should be restored to clean.
- Transaction rollback, test-specific databases, table-by-table cleanup, prefix-based cleanup, or fixture rebuild may be used, but rules must be explicit.
- Multiple tests must not share persistent data that can contaminate each other.
- For auto-increment primary keys, unique indexes, foreign keys, and soft-delete fields, compatibility with repeated execution after cleanup must be considered.
- If tests depend on seed data, seed data must be explicitly declared and rebuildable.
- For legacy system Enhancements only, the test prefix must include the Enhancement_2026_ marker, and cleanup logic must use “prefix-based local cleanup” rather than physical full cleanup.

6. Other supplementary rules
- Prefer testing public behavior and do not bind tests to private implementation details.
- Assertions on exception messages should be stable and meaningful; avoid depending on the full content of volatile text.
- For logs, audit, and sensitive information, verify only existence and masking principles; do not output real sensitive values.
- Do not add fragile tests just to improve coverage; low-value tests should be rejected.
- If duplicate tests already exist for the target code, prefer completing missing scenarios rather than copying existing assertions.

7. Coverage
- For the target test scope, generated unit test code should have an explicit goal of reaching more than 90% coverage.
- Coverage priority order is: critical business branches, exception branches, boundary branches, secondary branches. Do not sacrifice assertion quality to chase numbers.

8. Boundary tests
- Must cover minimum values, maximum values, critical values, null values, default values, and format boundaries.
- For inputs such as pagination, quantity, amount, date, string length, enum, status code, and permission role, check at least one valid boundary and one invalid boundary.
- Boundary tests must explicitly state whether the boundary is a contract boundary, business boundary, or data boundary.
- If a boundary triggers a different implementation path, split it into an independent test and avoid mixing it into the same case.

9. Exception scenario tests
- Every core behavior should include at least one exception test group, covering parameter exceptions, state exceptions, permission exceptions, and dependency exceptions.
- Exception tests must validate error codes, status codes, error fields, or exception types rather than only verifying “throws exception”.
- External dependency failure, database failure, concurrency conflict, empty result, and timeout must be tested separately.
- For expected exceptions, check whether recovery behavior or side effects meet expectations; avoid only verifying thrown exceptions while ignoring later contamination.

## Workflow

1. Identify target scope
- Parse constraints related to the target API or business scenario from the requirements document.
- Locate the corresponding WebAPI entry point, service-layer method, query handler, command handler, or repository method.
- Determine which test types should be generated: unit, Controller contract test, integration test, or a combination.

2. Clarify dependency boundaries
- List direct dependencies of the system under test, including external interfaces, database access, time sources, configuration, and messaging components.
- Mark which dependencies need Mocks, which need real execution, and which need test doubles.
- Find the most stable test entry point and prefer generating tests from the entry point where public behavior is most concentrated.

3. Design the test matrix
- Design success path, validation failure, state conflict, permission failure, and external dependency failure cases for each target behavior.
- Complete assertions for Method, Path, Header, Body, StatusCode, ErrorCode, and Schema for API contract tests.
- Complete the three phases of initialization, execution, and cleanup for database integration tests.

4. Generate test code
- Create or supplement test files according to the repository's existing test framework, naming style, and directory structure.
- Extract common fixtures, builders, helpers, fakes, or mock configurations to avoid repeated piles of setup code.
- Reuse existing test infrastructure as much as possible, such as test base classes, data builders, in-memory databases, factory methods, and so on.

5. Validate and converge
- Check whether tests are independent, stable, readable, and maintainable.
- Check whether key contracts, boundary values, exception paths, and cleanup logic are missing.
- If tests introduce brittleness, roll back to a more robust assertion style.

6. Output results
- List added or modified test files.
- Briefly describe the behavior and dependency boundaries covered by each test group.
- Clearly state database cleanup strategy and Mock boundaries.

## Reference Cases

### Case 1: Controller contract tests
Goal: generate tests for a WebAPI Controller and verify routes, parameter binding, and response structure.

Example targets:
- Create, edit, submit, cancel, and query actions of `LeaveController`
- Any API facade layer similar to `BaseController`

Recommended coverage:
- Normal request returns `200` or the agreed status code
- Missing required Header, Query, or Body returns validation error
- Invalid route parameter format returns route or validation error
- Response body field names, hierarchy, and error codes comply with contract

### Case 2: Service-layer business tests
Goal: generate tests for business services and verify state transitions, calculation logic, and exception semantics.

Example targets:
- Services such as `LeaveService` that directly orchestrate state, quota, ownership relationships, and persistence

Recommended coverage:
- State changes such as draft creation, edit, submit, and revoke
- Illegal state transitions are blocked
- Clear business exceptions are thrown when quota or permission is insufficient
- Persistence calls occur at the correct time and with correct arguments

### Case 3: Database integration tests
Goal: generate real database tests for Repository, UnitOfWork, or cross-table queries, and ensure state cleanup.

Example targets:
- `UnitOfWork` + `Repository` chain
- Methods depending on entity relationships, query sorting, pagination, or filtering logic

Recommended coverage:
- Data can be correctly queried after writing
- Relationship navigation properties and projections are correct
- Pagination, sorting, and filtering are consistent in the real database
- Database is restored to a clean state after each test, and repeated executions produce consistent results

## Generation Strategy Template

When the input is a WebAPI entry point, generate tests preferably in the following order:
1. Contract tests: verify method, path, status code, error code, and response structure
2. Business unit tests: verify core rules, state transitions, and calculation logic
3. Integration tests: verify database, repository, and real combined behavior

When the input is a service class, generate tests preferably in the following order:
1. Rule unit tests: input validation, state machine, permissions, boundary values
2. Dependency interaction tests: verify repository or external dependency calls
3. Integration tests: generate only when there is real database behavior or cross-component behavior

## Validation Checklist

Check each item before submission:

- [ ] Test scope has been located according to the requirements document and existing WebAPI code.
- [ ] Test type selection is correct, without mixing contract tests, unit tests, and integration tests together.
- [ ] Key API contracts cover method, path, input, output, status code, and error code.
- [ ] Business rules, state transitions, permissions, and boundary conditions are covered.
- [ ] All external dependencies are isolated; no real networks or shared external systems are accessed.
- [ ] Mock boundaries are clear and tests are not implementation copies.
- [ ] Database integration tests have a clear initialization and cleanup strategy.
- [ ] Database-State-Cleanup has been verified as repeatable.
- [ ] Test names are clear and can quickly locate failure causes.
- [ ] Tests are independent and do not depend on execution order.
- [ ] No brittle assertions, hardcoded time, or random values are introduced.
- [ ] No unrelated business code was modified and no unnecessary refactoring was introduced.
- [ ] Generated results are consistent with the repository's existing test framework, naming, and directory structure.

## Output Template

When running this Skill, output using the following structure:

1. Test scope
- Target code scope, requirements source, test type

2. Generation results
- Added or modified test files
- Used Mocks, Fixtures, TestData, and cleanup strategy

3. Coverage notes
- Which API contracts, business rules, exception paths, and database behaviors are covered

4. Risks and limitations
- Boundary scenarios not yet covered
- Parts that require a real environment for validation

5. Validation results
- Compilation or test execution results
- Whether database cleanup passed repeated-run validation