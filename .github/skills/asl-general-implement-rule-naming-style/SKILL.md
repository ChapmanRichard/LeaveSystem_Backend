---
name: asl-general-implement-rule-naming-style
description: "Use when: 需要约束 API 实现中的命名规范与代码风格一致性。关键词: 命名规范, 代码风格, 可读性, 一致性"
---

# 命名规范和风格守卫子 Skill

## 意图
确保新增代码命名清晰、风格一致、改动最小化。

## 输入
- 目标改动文件列表

## 规则
- 本工程命名基线: Controller 以 `Controller` 结尾，Command 以 `Command` 结尾，Query 以 `Query` 结尾，Handler 以 `CommandHandler/QueryHandler` 结尾，Service 接口以 `I` 前缀命名并与实现同名配对。
- API 动作命名遵循“动词+业务对象”，例如 `CreateLeaveApplication`、`ApproveLeave`；避免模糊缩写和无业务语义命名。
- C# 风格守卫: 类型/成员使用 PascalCase，局部变量 camelCase，私有只读字段使用 `_camelCase`（历史代码存在差异时，新增代码保持局部一致并在同文件内统一）。
- 单一职责守卫: Controller 仅处理路由/鉴权/协议转换，不落业务规则；复杂流程下沉至 Service。
- 变更最小化守卫: 不做无关重命名与大规模格式化，确保 diff 聚焦目标 API。
- 参考标准: .NET Runtime Coding Style、Microsoft C# Coding Conventions、Clean Code 命名可读性原则。

## 最小验证清单
- [ ] 新增符号命名符合项目约定。
- [ ] 未引入无关重命名或大面积格式化。
- [ ] 关键动作命名可直接理解业务含义。
