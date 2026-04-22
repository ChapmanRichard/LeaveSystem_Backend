# 权限控制说明

系统不做真实登录，默认通过请求头模拟当前身份：

- `X-User-Name`: 当前用户名
- `X-User-Role`: `Employee | Manager | Admin`

权限判断遵循“角色 + 归属 + 状态”三层原则：

- 先判断角色是否允许访问接口
- 再判断资源是否属于当前用户
- 再判断单据状态是否允许当前操作

其中，年假提交还需要额外做额度校验，前后端都要拦截。

# 需求中的权限定义

| 角色 | 功能点 | 权限说明 |
| --- | --- | --- |
| Employee(普通员工) | 提交请假单 | 只能创建自己的请假单，可保存为草稿并提交为待审批 |
| Employee(普通员工) | 编辑请假单 | 仅允许编辑自己名下且状态为 Draft 或 Rejected 的单据 |
| Employee(普通员工) | 删除请假单 | 仅允许删除自己名下且状态为 Draft 的单据 |
| Employee(普通员工) | 撤回申请 | 仅允许撤回自己名下且状态为 Pending 的单据 |
| Employee(普通员工) | 查询请假记录 | 只能查看自己的请假列表和详情 |
| Employee(普通员工) | 查询假期余额 | 只能查看自己的余额信息 |
| Employee(普通员工) | 年假提交校验 | 提交年假时必须先检查剩余额度，额度不足时前端禁用提交，后端再次校验 |
| Manager(经理) | 查看待审批请假单 | 可以查看所有待审批单据，以及按条件筛选的待审批列表 |
| Manager(经理) | 查看已处理请假单 | 可以查看自己权限范围内已审批或已打回的单据 |
| Manager(经理) | 审批通过 | 仅允许处理 Pending 单据，且必须填写审批意见 |
| Manager(经理) | 打回申请 | 仅允许处理 Pending 单据，且必须填写审批意见 |
| Manager(经理) | 查看审批详情 | 可以查看审批弹窗所需的完整请假单信息 |
| Manager(经理) | 调整额度 | 不允许，额度调整属于管理员权限 |
| Admin(系统管理员) | 查看全员额度 | 可以查看所有员工的假期余额与额度明细 |
| Admin(系统管理员) | 调整员工额度 | 可以设置、增加或减少指定员工的假期总额度 |
| Admin(系统管理员) | 查看请假单详情 | 不允许查看具体请假单细节与审批数据 |
| Admin(系统管理员) | 参与审批流程 | 不参与审批流转，不允许审批通过或打回 |

补充约束：

- Employee 不能调用任何审批接口
- Employee 和 Manager 不能调用任何额度调整接口
- Admin 不能调用任何请假单详情接口与审批接口
- 所有越权访问统一按 `FORBIDDEN` 处理

# 接口中的权限管理

| 接口 | Employee | Manager | Admin | 约束说明 |
| --- | --- | --- | --- | --- |
| `POST /leaves` | 允许 | 不允许 | 不允许 | 创建请假单，员工专属；年假需后端二次额度校验 |
| `PUT /leaves/{leaveId}` | 允许 | 不允许 | 不允许 | 仅本人，且状态必须是 Draft 或 Rejected |
| `DELETE /leaves/{leaveId}` | 允许 | 不允许 | 不允许 | 仅本人，且状态必须是 Draft |
| `POST /leaves/{leaveId}/submit` | 允许 | 不允许 | 不允许 | 仅本人，且状态必须是 Draft；年假需额度校验 |
| `POST /leaves/{leaveId}/cancel` | 允许 | 不允许 | 不允许 | 仅本人，且状态必须是 Pending |
| `GET /leaves/my` | 允许 | 不允许 | 不允许 | 仅返回自己的请假列表 |
| `GET /leaves/{leaveId}` | 允许 | 不允许 | 不允许 | 仅返回自己的请假详情；Admin 也不能访问 |
| `GET /quotas/my` | 允许 | 不允许 | 不允许 | 仅返回自己的余额信息 |
| `GET /approvals/pending` | 不允许 | 允许 | 不允许 | 经理查看待审批列表，可按 employeeId 过滤 |
| `GET /approvals/history` | 不允许 | 允许 | 不允许 | 经理查看已处理列表 |
| `GET /approvals/{leaveId}` | 不允许 | 允许 | 不允许 | 经理查看审批详情；Admin 不允许访问 |
| `POST /approvals/{leaveId}/approve` | 不允许 | 允许 | 不允许 | 仅 Pending，且 comment 必填 |
| `POST /approvals/{leaveId}/reject` | 不允许 | 允许 | 不允许 | 仅 Pending，且 comment 必填 |
| `GET /quotas` | 不允许 | 不允许 | 允许 | 管理员查看全员额度列表 |
| `POST /quotas/adjust` | 不允许 | 不允许 | 允许 | 管理员专属额度调整接口，建议记录审计备注 |

接口级别的统一要求：

- 角色不匹配时直接返回 `FORBIDDEN`
- 资源不属于当前员工时，不能仅靠前端隐藏，后端必须再次校验
- 状态不满足操作条件时，返回 `INVALID_STATUS_TRANSITION`
- 审批意见缺失时，返回 `APPROVAL_COMMENT_REQUIRED`
- 年假额度不足时，返回 `LEAVE_QUOTA_EXCEEDED`