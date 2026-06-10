---
name: asl-general-implement-rule-exception-logging
description: "Use when: 需要统一异常分层处理和日志策略，保证问题可追踪且不泄露敏感信息。关键词: 异常处理, 日志规范, 可观测性, 审计"
---

# 异常与日志规范子 Skill

## 意图
规范异常语义和日志行为，提升排障与审计能力。

## 输入
- 目标接口或服务
- 关键异常路径

## 规则
- 异常分层守卫: 验证异常使用 `ValidationException` 或领域明确异常；业务冲突使用可识别异常类型；未知异常统一转换为通用错误响应，避免将堆栈回传给客户端。
- 日志最小充分守卫: 记录 `traceId/requestId`、接口名、命令/查询名、关键上下文，不记录敏感明文；日志级别区分 `Debug/Info/Error`。
- 记录系统已有的 Session 或 Cookie 中的用户标识进行关联日志，避免在日志中直接记录敏感用户信息。
- 统一处理守卫: 优先复用现有装饰器链路（如 `ErrorLogCommandHandlerDecorator`、`ErrorLogQueryHandlerDecorator`）与 `WebMemoryCommandBus` 统一错误出口，避免每个 Controller 重复 try-catch。
- 可观测性守卫: 对关键失败路径补充可检索日志关键词（接口标识、实体主键、状态变化前后值）。
- 审计守卫: 审批、额度调整等高风险操作应记录操作者、时间、对象、动作、结果。
- 参考标准: OWASP Logging Cheat Sheet、SRE 可观测性实践、RFC 5424（日志级别语义）。

## 最小验证清单
- [ ] 异常分层明确，客户端无堆栈泄露。
- [ ] 日志级别与上下文信息合理。
- [ ] 关键操作审计字段完整。