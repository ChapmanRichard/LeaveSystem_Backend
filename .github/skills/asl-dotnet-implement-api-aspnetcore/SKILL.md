---
name: asl-dotnet-implement-api-aspnetcore
description: "Use when: you need to implement a specified API in the current project according to requirements and API design documents, such as API-E1 create leave request draft, while following the existing layered architecture and coding standards, and verifying the industry standard <<GBG_Tech_Standards.md>> and project acceptance standard <<DOD.md>>. Keywords: Req.md, api_design.md, API implementation, Controller, CommandHandler, Service, Validator"
---

# Implement a Specified API from Req and API Design

## Intent

This Skill converts “documented requirements” into “runnable code” while ensuring the implementation follows the current project's existing style.

Applicable scenarios:
- The user provides an API identifier, such as `API-E1`, or a path, such as `POST /leaves`
- Requirements documentation and API design documentation already exist
- The API implementation needs to be delivered within the current layered architecture

Expected outcomes:
- Accurately implement the specified API according to `Spec/Req.md` and `Spec/api_design.md`
- Prefer reusing existing Models/infrastructure to avoid reinventing the wheel
- Provide complete changes and the smallest verifiable result

## Input

Required input:
- Specified API identifier or API path, for example: `API-E1 Create leave request (draft)`
- Requirements document path (default: `Spec/Req.md`)
- API document path (default: `Spec/api_design.md`)

Optional input:
- Whether adding new entity Models is allowed
- Response structure strategy: keep the existing `JsonResultFactory` style, or switch to the new unified `code/message/data`
- Whether to include the smallest joint-debugging test sample

## Project Architecture Conventions (Must Follow)

1. Layered path conventions
- Web layer: `src/Api.Web/Controllers`
- Contract layer:
  - Commands: `src/Api.Contract/Commands`
  - Queries: `src/Api.Contract/Querie` or `src/Api.Contract/Queries`
  - Models: `src/Api.Contract/Model`
  - Service interfaces: `src/Api.Contract/ServiceInterfaces`
- BLL layer:
  - CommandHandler: `src/Api.BLL/CommandHandlers`
  - QueryHandler: `src/Api.BLL/QueryHandlers`
  - Validator: `src/Api.BLL/CommandValidators`
  - Service: `src/Api.BLL/Services`
- DAL layer:
  - `UnitOfWorkLeaveSystem`
  - `LeaveSystemDBRepository<T>`
  - `LeaveSystemDBContext`

2. Execution chain conventions
- Controllers do not contain business logic
- Write APIs preferably use `IWebCommandBus -> ICommandHandler -> Service -> UnitOfWork/Repository`
- Query APIs preferably use `IQueryProcessor -> QueryHandler -> Service`

3. DI and registration conventions
- Handlers/services are automatically registered through `Scan + Decorate` in `Program.cs`
- New classes must ensure their namespaces are within the scan scope

4. Style and compatibility conventions
- Keep the minimal-change principle and avoid unrelated refactoring
- If the documented response format conflicts with the existing `JsonResultFactory` style, prefer project consistency and explain the difference in the output
- Do not modify unrelated API behavior

5. Project business role conventions (from Req.md)
- The role set is Employee, Manager, and Admin.
- Employee focuses on leave request applications and personal data access; Manager focuses on approvals; Admin focuses on quota management.
- This role matrix is a project-specific business constraint. During implementation, use `Req.md` and `api_design.md` as the source of truth.

## Rules

1. Sub-Skill list
- Fully load and use the following latest Skill indexes, based on the workspace root `skills/` directory rather than `.github/skills/` under this directory:
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


## Workflow

1. Parse the target API
- Locate the target entry in `api_design.md`, such as `API-E1`
- Extract: Method, Path, input, output, validation, permissions, and state transitions
- Check role permissions and business rules in `Req.md`

2. Inventory existing code
- Check whether a Controller Action with the same name/route already exists
- Check whether corresponding Command/DTO/Model already exists in Contract
- Check whether reusable Handler/Service/Validator already exists in BLL

3. Design the minimal change plan
- Clarify the list of files to add/modify
- Decide whether to add Model fields and `DbContext` mappings
- Decide response wrapping, either existing style or new standard

4. Layered implementation
- Controller: add Action, bind route and authorization
- Contract: add Command/Query/DTO
- Validator: add input validation
- Handler: orchestrate command processing
- Service: implement core business and state machine
- DAL: supplement entity fields/repository extensions when necessary

5. Self-check and validation
- Compilation check
- Validate route, permissions, state transitions, and quota checks item by item
- Provide minimal request/response samples
- Check whether validation complies with `Spec/AccessControl.md`
- Check whether validation complies with `DomainKnowledge/GBG_Tech_Standards.md` and `ProjectCustomize/DOD.md`

6. Output results
- List changed files
- Provide key implementation notes
- Provide uncovered risks and follow-up suggestions

## API-E1 Reference Implementation Blueprint (Example)

Goal: `API-E1 Create leave request (draft)`

Recommended change order:
1. Add a Contract command
- Add `CreateLeaveApplicationCommand : ICommand<string>` under `Api.Contract/Commands`
- Fields: `LeaveType, StartAt, EndAt, Reason, SaveAsDraft`

2. Reuse or supplement Model
- If a leave entity already exists, reuse it
- If none exists, add `LeaveApplication` to `Api.Contract/Model` and register `DbSet` in `LeaveSystemDBContext`

3. Service and interface
- Add `CreateLeaveApplication(CreateLeaveApplicationCommand cmd)` to `ILeaveService`
- Implement inside `LeaveService`:
  - Calculate `durationDays`
  - Validate time range and minimum 0.5 day
  - Set status to `Draft`
  - Persist and return request number

4. Validator
- Add `LeaveCommandValidator : IValidator<CreateLeaveApplicationCommand>`
- Validate required fields, length, and time relationship

5. Handler
- Add `LeaveCommandHandler : ICommandHandler<CreateLeaveApplicationCommand, string>`
- Only forward to Service

6. Controller
- Add `LeaveController`
- Add `POST /api/Leave/CreateDraft` or map the documented route according to team rules
- Call `CommandBus.SubmitAndReturnJsonResult(command)`

7. Joint-debugging sample
- Input sample:
```json
{
  "leaveType": "Annual",
  "startAt": "2026-04-10T09:00:00+08:00",
  "endAt": "2026-04-11T18:00:00+08:00",
  "reason": "I need to take leave for family matters, estimated two days",
  "saveAsDraft": true
}
```
- Output sample (using existing style):
```json
{
  "RESULT": "TRUE",
  "ERROR_CODE": "",
  "DATA": "L202604070001"
}
```

## Reference Cases

### Case 1: Implement API-E1
Prompt example:
- "Implement API-E1 Create leave request (draft) according to Req.md and api_design.md, and follow the current project's CommandBus layering."

Expected results:
- Complete the minimal closed loop from Controller to Service
- Support draft creation and return an ID

### Case 2: Implement API-M4 approval pass
Prompt example:
- "Implement API-M4, manager approval pass, with comment required."

Expected results:
- Only Manager can call it
- Status must be `Pending`
- Return the new status and approval comment

### Case 3: Implement API-A2 quota adjustment
Prompt example:
- "Implement API-A2, admin quota adjustment, supporting SetTotal/Increase/Decrease."

Expected results:
- Permission control is correct
- Data calculation is correct and negative balances cannot occur

## Validation Checklist

Check each item before submission:

- [ ] The target API is consistent with the Method/Path/input/output in `api_design.md`.
- [ ] Authentication and authorization comply with definitions in `Req.md` and `api_design.md`, including interface layer and resource layer.
- [ ] State machine transition rules are correct, and illegal transitions are blocked.
- [ ] Parameter validation and business validation are both covered.
- [ ] Controller does not contain core business logic.
- [ ] Command/Handler/Service call chain is runnable.
- [ ] Newly added classes are discoverable by the DI scanning mechanism.
- [ ] Response structure is consistent with the project's existing style, or a compatibility solution is explicitly stated.
- [ ] No sensitive information is leaked in code or logs, and sample data is masked.
- [ ] Component references comply with layered and DI standards, with no cross-layer direct connections or circular dependencies.
- [ ] Component references satisfy whitelist constraints and do not trigger blacklisted violation patterns.
- [ ] Common security risks are covered, including injection, unauthorized access, XSS, information leakage, and concurrent duplicate submission.
- [ ] Naming and style comply with project conventions, and no unrelated refactoring noise is introduced.
- [ ] API contract is consistent with `api_design.md`; if deviations exist, compatibility notes are recorded.
- [ ] Exception layering and logging comply with standards; logs contain no sensitive information and are traceable.
- [ ] Storage processing complies with standards, including transactions, consistency, pagination limits, data minimization, and deletion strategy.
- [ ] At least one success and one failure request sample are provided.
- [ ] Compilation passes; no unrelated file changes.

## Output Template

When running this Skill, output using the following structure:

1. Implementation scope
- Interface identifier, document sources, adopted response standard

2. Changed files
- Exact path list (added/modified)

3. Core implementation notes
- Permissions, state machine, validation, persistence, return values

4. Validation results
- Compilation result
- Sample request/response

5. Risks and follow-up
- Boundary scenarios not yet covered
- Next-step recommendations
