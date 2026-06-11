---
name: asl-general-implement-rule-component-reference
description: "Use when: you need to constrain component reference boundaries, whitelists, and blacklists to avoid cross-layer dependency contamination. Keywords: component references, whitelist, blacklist, layered dependencies"
---

# Component Reference Standards Sub-Skill

## Intent
Ensure component references keep clear layer boundaries and controllable dependencies.

## Input
- Dependencies involved in this change

## Rules
- New code should preferably reuse existing layered components and infrastructure (`IWebCommandBus`, `IQueryProcessor`, `UnitOfWorkLeaveSystem`, `LeaveSystemDBRepository<T>`) and avoid direct cross-layer connections.
- Prefer interface injection for dependency injection; do not directly `new` foundational services. Namespaces and directories must fall within the existing scanning rules so that `Program.cs` can discover them automatically.
- For third-party libraries, follow the “minimum necessary dependency” principle: prefer libraries already introduced by the project; before adding dependencies, evaluate maintenance status, license, CVE risk, and alternatives.
- Keep reference directions one-way: Controller -> Handler/Service -> Repository. Reverse dependencies and circular dependencies are prohibited.
- Component reference whitelist (allowed by default):
  - Web layer: `ControllerBase`, `[ApiController]`, `[Authorize]/[AllowAnonymous]`, `IWebCommandBus`, `IQueryProcessor`
  - BLL layer: `ICommandHandler<,>`, `IQueryHandler<,>`, `ISingleQueryHandler<,>`, `IValidator<>`, business Service interfaces
  - DAL layer: `UnitOfWorkLeaveSystem`, `LeaveSystemDBRepository<T>`, `LeaveSystemDBContext`, EF Core `DbSet`
  - Common layer: `JsonResultFactory`, `IAppLogger`, AutoMapper `MapperProvider`/Profile
- Component reference blacklist (prohibited by default):
  - Controller directly accesses `DbContext/Repository`
  - Cross-layer bypasses, such as Controller directly calling DAL or Handler directly concatenating SQL
  - Directly introducing unreviewed third-party packages into the production path

## Minimum Validation Checklist
- [ ] Component references comply with layered boundaries.
- [ ] No blacklisted dependency pattern is triggered.
- [ ] New dependencies have purpose and risk explanations.
