---
name: asl-general-implement-rule-authz
description: "Use when: 需要实现认证与鉴权规则，覆盖接口层和资源层授权。关键词: Authentication, Authorization, RBAC, 最小权限"
---

# 认证与鉴权子 Skill

## 意图
确保接口具备认证与授权边界，防止越权访问。

## 输入
- 目标接口
- 角色矩阵来源（默认 `Req.md`）

## 规则
- 接口必须同时满足认证（Authentication）与鉴权（Authorization），不得仅依赖路由可达性判断权限。
- 采用“最小权限原则（Least Privilege）”与“默认拒绝（Deny by Default）”，未明确授权的访问应被拒绝。
- 鉴权至少包含两层: 接口层访问控制 + 资源级访问控制（如是否可访问该资源、是否可执行该动作）。
- 本工程具体角色判定与授权边界按“本工程业务角色约定（来自 Req.md）”执行。
- 参考标准: OWASP ASVS V4（Access Control）、NIST SP 800-63（Digital Identity）、RFC 9110（HTTP 认证语义）。

## 最小验证清单
- [ ] 受保护接口有认证约束。
- [ ] 已做资源级权限校验。
- [ ] 未授权请求返回稳定错误响应。