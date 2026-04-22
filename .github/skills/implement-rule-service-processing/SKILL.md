---
name: implement-rule-service-processing
description: "Use when: 需要约束服务层业务编排、异常语义和幂等一致性。关键词: 服务层, 业务编排, 幂等, 一致性"
---

# 服务层处理规范子 Skill

## 意图
确保服务层承载业务核心并维持边界清晰。

## 输入
- 服务层改动范围
- 相关命令/查询

## 规则
- 服务层是业务编排核心，负责执行业务规则、状态转换、跨实体一致性，不将核心业务逻辑下沉到 Controller。
- 服务层应通过接口解耦并复用现有调用链（`IWebCommandBus -> ICommandHandler -> Service -> UnitOfWork/Repository`），保持单向依赖。
- 服务层必须保证幂等性与一致性: 对关键操作定义幂等键或去重策略，对多步写入确保事务边界。
- 服务层异常应语义化（验证失败、业务冲突、资源不存在等），并交由统一错误出口处理，避免吞异常。
- 服务层应避免 N+1 查询和不必要的大对象加载，遵循“最小读取、最小写入”原则。
- 参考标准: DDD 应用服务实践、Clean Architecture Use Case Boundary、12-Factor Backing Services 原则。

## 最小验证清单
- [ ] Controller 未承载核心业务逻辑。
- [ ] 服务层异常语义明确且可统一处理。
- [ ] 多步业务操作具备一致性保障。
