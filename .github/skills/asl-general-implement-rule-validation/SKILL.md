---
name: asl-general-implement-rule-validation
description: "Use when: 需要实现输入参数和业务前置条件校验，确保接口输入安全且可机读报错。关键词: 数据校验, Validator, 边界校验"
---

# 数据校验规则子 Skill

## 意图
建立分层校验机制，确保输入可靠且错误可诊断。

## 输入
- 请求模型
- 业务前置条件

## 规则
- 参数完整性（必填、长度、时间先后、格式）在 Validator 或等价校验层实现，且应在进入核心业务逻辑前完成。
- 业务校验（额度、越权、资源归属、状态前置条件）在 Service 层二次校验，避免仅依赖前端。
- 对枚举、分页、时间区间、数值范围执行白名单与边界校验，拒绝隐式类型转换导致的歧义输入。
- 统一错误表达: 校验失败应返回稳定、可机读的错误码和字段级错误信息。
- 参考标准: OWASP ASVS V5（Validation, Sanitization and Encoding）、RFC 9457（Problem Details for HTTP APIs）、JSON Schema 校验实践。

## 最小验证清单
- [ ] 参数与业务校验均已覆盖。
- [ ] 边界值与非法值路径已考虑。
- [ ] 校验错误响应可机读且结构稳定。