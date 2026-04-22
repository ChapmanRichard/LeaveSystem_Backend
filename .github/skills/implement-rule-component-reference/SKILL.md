---
name: implement-rule-component-reference
description: "Use when: 需要约束组件引用边界、白名单和黑名单，避免跨层依赖污染。关键词: 组件引用, 白名单, 黑名单, 分层依赖"
---

# 组件引用规范子 Skill

## 意图
确保组件引用保持分层边界清晰、依赖可控。

## 输入
- 本次改动涉及的依赖关系

## 规则
- 新代码优先复用现有分层组件与基础设施（`IWebCommandBus`、`IQueryProcessor`、`UnitOfWorkLeaveSystem`、`LeaveSystemDBRepository<T>`），避免跨层直连。
- 依赖注入优先使用接口注入，不直接 `new` 基础服务；命名空间与目录要落在现有扫描规则范围内，确保 `Program.cs` 能自动发现。
- 对第三方库遵循“最小必要依赖”原则：优先使用项目已引入库；新增依赖前评估维护状态、许可证、CVE 风险和替代方案。
- 引用关系保持单向: Controller -> Handler/Service -> Repository，禁止反向依赖与循环依赖。
- 组件引用白名单（默认允许）:
  - Web 层: `ControllerBase`、`[ApiController]`、`[Authorize]/[AllowAnonymous]`、`IWebCommandBus`、`IQueryProcessor`
  - BLL 层: `ICommandHandler<,>`、`IQueryHandler<,>`、`ISingleQueryHandler<,>`、`IValidator<>`、业务 Service 接口
  - DAL 层: `UnitOfWorkLeaveSystem`、`LeaveSystemDBRepository<T>`、`LeaveSystemDBContext`、EF Core `DbSet`
  - 通用层: `JsonResultFactory`、`IAppLogger`、AutoMapper `MapperProvider`/Profile
- 组件引用黑名单（默认禁止）:
  - Controller 直接访问 `DbContext/Repository`
  - 跨层绕过（Controller 直接调用 DAL，Handler 直接拼接 SQL）
  - 未审查第三方包直接引入生产链路

## 最小验证清单
- [ ] 组件引用符合分层边界。
- [ ] 未触发黑名单依赖模式。
- [ ] 新依赖已有用途和风险说明。
