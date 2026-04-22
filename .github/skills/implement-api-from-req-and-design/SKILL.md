---
name: implement-api-from-req-and-design
description: "Use when: 需要根据需求文档与 API 设计文档，在当前 工程中实现指定接口（例如 API-E1 创建请假单草稿），并遵循现有分层架构与编码规范，并且验证行业标准<<GBG_Tech_Standards.md>>,项目收货标准<<DOD.md>>。关键词: Req.md, api_design.md, API实现, Controller, CommandHandler, Service, Validator"
---

# 根据 Req 与 API 设计实现指定接口

## 意图

该 Skill 用于把“文档需求”转化为“可运行代码”，并确保实现过程遵循当前工程既有风格。

适用场景:
- 用户给出某个 API 标识（如 `API-E1`）或路径（如 `POST /leaves`）
- 已有需求文档与接口设计文档
- 需要在当前分层架构中落地接口实现

目标结果:
- 按 `src/Markdown/Req.md` 与 `src/Markdown/api_design.md` 精准实现指定 API
- 优先复用现有 Models/基础设施，避免重复造轮子
- 给出完整改动与最小可验证结果

## 输入

必需输入:
- 指定接口标识或接口路径（例如: `API-E1 创建请假单(草稿)`）
- 需求文档路径（默认: `src/Markdown/Req.md`）
- API 文档路径（默认: `src/Markdown/api_design.md`）

可选输入:
- 是否允许新增实体 Model
- 返回结构策略: 保持现有 `JsonResultFactory` 风格，或切换到新版统一 `code/message/data`
- 是否包含最小联调测试样例

## 本工程架构约定（必须遵循）

1. 分层路径约定
- Web 层: `src/Api.Web/Controllers`
- Contract 层:
  - 命令: `src/Api.Contract/Commands`
  - 查询: `src/Api.Contract/Querie` 或 `src/Api.Contract/Queries`
  - 模型: `src/Api.Contract/Model`
  - 服务接口: `src/Api.Contract/ServiceInterfaces`
- BLL 层:
  - CommandHandler: `src/Api.BLL/CommandHandlers`
  - QueryHandler: `src/Api.BLL/QueryHandlers`
  - Validator: `src/Api.BLL/CommandValidators`
  - Service: `src/Api.BLL/Services`
- DAL 层:
  - `UnitOfWorkLeaveSystem`
  - `LeaveSystemDBRepository<T>`
  - `LeaveSystemDBContext`

2. 执行链路约定
- Controller 不写业务逻辑
- 写入类接口优先走 `IWebCommandBus -> ICommandHandler -> Service -> UnitOfWork/Repository`
- 查询类接口优先走 `IQueryProcessor -> QueryHandler -> Service`

3. DI 与注册约定
- 处理器/服务通过 `Program.cs` 的 `Scan + Decorate` 自动注册
- 新增类要保证命名空间在扫描范围中

4. 风格与兼容约定
- 保持最小变更原则，避免无关重构
- 如果文档响应格式与现有 `JsonResultFactory` 风格冲突，优先保持工程一致，并在输出中说明差异
- 不修改无关接口行为

5. 本工程业务角色约定（来自 Req.md）
- 角色集合为 Employee、Manager、Admin。
- Employee 侧重请假单申请与个人数据访问；Manager 侧重审批；Admin 侧重额度管理。
- 该角色矩阵属于本工程特定业务约束，实现时以 `Req.md` 与 `api_design.md` 为准。

## 规则

1. 子 Skill清单
- 全量加载和使用以下子 Skill 索引:
  - 通用规则: `.github/skills/implement-rule-general/SKILL.md`
  - 命名规范和风格守卫: `.github/skills/implement-rule-naming-style/SKILL.md`
  - 认证与鉴权: `.github/skills/implement-rule-authz/SKILL.md`
  - 状态机一致性: `.github/skills/implement-rule-state-machine/SKILL.md`
  - 数据校验规则: `.github/skills/implement-rule-validation/SKILL.md`
  - API 契约一致性守卫: `.github/skills/implement-rule-api-contract-consistency/SKILL.md`
  - 服务层处理规范: `.github/skills/implement-rule-service-processing/SKILL.md`
  - 存储处理规范: `.github/skills/implement-rule-storage-processing/SKILL.md`
  - 异常与日志规范: `.github/skills/implement-rule-exception-logging/SKILL.md`
  - 敏感信息规范: `.github/skills/implement-rule-sensitive-data/SKILL.md`
  - 组件引用规范: `.github/skills/implement-rule-component-reference/SKILL.md`
  - 漏洞防范规范: `.github/skills/implement-rule-vulnerability-prevention/SKILL.md`


## 工作流

1. 解析目标接口
- 在 `api_design.md` 中定位目标条目（如 `API-E1`）
- 抽取: Method、Path、输入、输出、校验、权限、状态流转
- 在 `Req.md` 中核对角色权限和业务规则

2. 盘点现有代码
- 检查是否已有同名/同路由 Controller Action
- 检查 Contract 中是否已有对应 Command/DTO/Model
- 检查 BLL 中是否已有 Handler/Service/Validator 可复用

3. 设计最小改动方案
- 明确新增/修改文件清单
- 决定是否新增 Model 字段和 `DbContext` 映射
- 决定返回包装（沿用现有或按新规范）

4. 分层实现
- Controller: 增加 Action，绑定路由与鉴权
- Contract: 增加 Command/Query/DTO
- Validator: 增加输入校验
- Handler: 编排命令处理
- Service: 实现核心业务与状态机
- DAL: 必要时补充实体字段/仓储扩展

5. 自检与验证
- 编译检查
- 路由、权限、状态转换、额度校验逐项验证
- 给出最小请求/响应样例
- 检查验证是否符合 `AccessControl.md`
- 检查验证是否符合 `GBG_Tech_Standards.md` 与 `DOD.md`

6. 输出结果
- 列出改动文件
- 给出关键实现说明
- 给出未覆盖风险与后续建议

## API-E1 参考实现蓝图（示例）

目标: `API-E1 创建请假单(草稿)`

建议改动顺序:
1. Contract 新增命令
- 在 `Api.Contract/Commands` 新增 `CreateLeaveApplicationCommand : ICommand<string>`
- 字段: `LeaveType, StartAt, EndAt, Reason, SaveAsDraft`

2. Model 复用或补充
- 若已有请假实体则复用
- 若无则新增 `LeaveApplication` 到 `Api.Contract/Model`，并在 `LeaveSystemDBContext` 注册 `DbSet`

3. Service 与接口
- `ILeaveService` 增加 `CreateLeaveApplication(CreateLeaveApplicationCommand cmd)`
- `LeaveService` 内实现:
  - 计算 `durationDays`
  - 校验时间范围与最小 0.5 天
  - 设置状态 `Draft`
  - 持久化并返回单号

4. Validator
- 新增 `LeaveCommandValidator : IValidator<CreateLeaveApplicationCommand>`
- 校验必填、长度、时间关系

5. Handler
- 新增 `LeaveCommandHandler : ICommandHandler<CreateLeaveApplicationCommand, string>`
- 仅转发到 Service

6. Controller
- 新增 `LeaveController`
- 增加 `POST /api/Leave/CreateDraft` 或按团队规则映射文档路由
- 调用 `CommandBus.SubmitAndReturnJsonResult(command)`

7. 联调样例
- 输入示例:
```json
{
  "leaveType": "Annual",
  "startAt": "2026-04-10T09:00:00+08:00",
  "endAt": "2026-04-11T18:00:00+08:00",
  "reason": "家中有事需请假处理，预计两天",
  "saveAsDraft": true
}
```
- 输出示例（按现有风格）:
```json
{
  "RESULT": "TRUE",
  "ERROR_CODE": "",
  "DATA": "L202604070001"
}
```

## 参考用例

### 用例 1：实现 API-E1
提示词示例:
- "根据 Req.md 和 api_design.md，实现 API-E1 创建请假单(草稿)，并遵循当前项目 CommandBus 分层。"

预期结果:
- 完成从 Controller 到 Service 的最小闭环
- 支持草稿创建并返回 ID

### 用例 2：实现 API-M4 审批通过
提示词示例:
- "实现 API-M4，经理审批通过，comment 必填。"

预期结果:
- 仅 Manager 可调用
- 状态必须 `Pending`
- 返回新状态与审批意见

### 用例 3：实现 API-A2 调整额度
提示词示例:
- "实现 API-A2，管理员调整额度，支持 SetTotal/Increase/Decrease。"

预期结果:
- 权限控制正确
- 数据计算正确且不可出现负余额

## 验证清单

提交前逐项核对:

- [ ] 目标 API 与 `api_design.md` 的 Method/Path/输入输出一致。
- [ ] 认证与鉴权符合 `Req.md` 与 `api_design.md` 的定义（含接口层与资源层）。
- [ ] 状态机流转规则正确，非法流转已拦截。
- [ ] 参数校验与业务校验都已覆盖。
- [ ] Controller 不包含业务核心逻辑。
- [ ] Command/Handler/Service 调用链可运行。
- [ ] 新增类已被 DI 扫描机制发现。
- [ ] 返回结构与项目现有风格一致（或已明确兼容方案）。
- [ ] 未在代码与日志中泄露敏感信息，示例数据已脱敏。
- [ ] 组件引用符合分层与 DI 规范，无跨层直连和循环依赖。
- [ ] 组件引用满足白名单约束，未触发黑名单违规模式。
- [ ] 已覆盖常见安全风险（注入、越权、XSS、信息泄露、并发重复提交）。
- [ ] 命名与风格符合本工程约定，且未引入无关重构噪声。
- [ ] API 契约与 `api_design.md` 一致，若存在偏差已记录兼容说明。
- [ ] 异常分层与日志记录符合规范，日志不包含敏感信息且可追踪。
- [ ] 存储处理符合规范（事务、一致性、分页限制、数据最小化、删除策略）。
- [ ] 至少提供 1 组成功与 1 组失败请求样例。
- [ ] 编译通过；无无关文件改动。

## 输出模板

运行本 Skill 时，按以下结构输出:

1. 实现范围
- 接口标识、文档来源、采用的返回规范

2. 改动文件
- 精确路径列表（新增/修改）

3. 核心实现说明
- 权限、状态机、校验、持久化、返回值

4. 验证结果
- 编译结果
- 示例请求/响应

5. 风险与后续
- 尚未覆盖的边界场景
- 下一步建议
