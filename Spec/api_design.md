# 请假申请管理系统 API 设计文档

## 1. 文档范围
本文档基于《Req.md》与《Pages.md》，定义请假申请管理系统 Web API 设计。

- API Base URL: `/api/v1`
- 数据格式: `application/json; charset=utf-8`
- 鉴权方式(模拟): 通过请求头传递当前用户身份

## 2. 角色与权限

- Employee(普通员工)
  - 提交请假单、编辑/删除自己的草稿、撤回审批中申请、查看自己的请假记录与余额
- Manager(经理)
  - 查看待审批/已处理列表，审批通过或打回(必须填写意见)
- Admin(系统管理员)
  - 查看所有员工额度，调整员工假期额度
  - 无权查看请假单详情与审批数据

## 3. 鉴权与上下文(模拟登录)
由于当前系统不做真实登录，建议每个请求带以下 Header:

- `X-User-Name`: 用户名，例如 `Employee1001`
- `X-User-Role`: `Employee | Manager | Admin`

后端基于该 Header 执行 RBAC 越权拦截。

## 4. 统一枚举

### 4.1 请假类型 LeaveType
- `Annual` 年假
- `Sick` 病假
- `Personal` 事假
- `Compensatory` 调休

### 4.2 单据状态 LeaveStatus
- `Draft` 草稿
- `Pending` 待审批
- `Approved` 已批准
- `Rejected` 已打回
- `Cancelled` 已撤销

## 5. 统一响应规范

### 5.1 成功响应
```json
{
  "code": "0",
  "message": "success",
  "data": {}
}
```

### 5.2 失败响应
```json
{
  "code": "LEAVE_QUOTA_EXCEEDED",
  "message": "年假额度不足",
  "errors": [
    {
      "field": "leaveType",
      "detail": "Annual leave is not enough"
    }
  ],
  "traceId": "3bfbf6dcdf9c4f5ab0fbe2db9042f6d6"
}
```

### 5.3 常见错误码
- `VALIDATION_FAILED` 参数校验失败
- `FORBIDDEN` 越权访问
- `NOT_FOUND` 资源不存在
- `INVALID_STATUS_TRANSITION` 非法状态流转
- `LEAVE_QUOTA_EXCEEDED` 年假额度不足
- `APPROVAL_COMMENT_REQUIRED` 审批意见必填

## 6. 数据模型

### 6.1 LeaveApplication
```json
{
  "id": "L202604070001",
  "employeeId": "U1001",
  "employeeName": "张三",
  "leaveType": "Annual",
  "startAt": "2026-04-10T09:00:00+08:00",
  "endAt": "2026-04-11T18:00:00+08:00",
  "durationDays": 2.0,
  "reason": "家中有事需请假处理，预计两天",
  "status": "Pending",
  "managerComment": null,
  "createdAt": "2026-04-07T10:00:00+08:00",
  "updatedAt": "2026-04-07T10:30:00+08:00"
}
```

### 6.2 LeaveQuota
```json
{
  "employeeId": "U1001",
  "employeeName": "张三",
  "leaveType": "Annual",
  "totalDays": 10.0,
  "usedDays": 2.0,
  "remainingDays": 8.0,
  "updatedAt": "2026-04-07T09:00:00+08:00"
}
```

## 7. API 设计

---

## 7.1 员工端 API (Employee)

### API-E1 创建请假单(草稿)
- Method: `POST`
- Path: `/leaves`
- 权限: Employee

#### 输入
```json
{
  "leaveType": "Annual",
  "startAt": "2026-04-10T09:00:00+08:00",
  "endAt": "2026-04-11T18:00:00+08:00",
  "reason": "家中有事需请假处理，预计两天",
  "saveAsDraft": true
}
```

#### 输入校验
- `leaveType` 必填，枚举值合法
- `startAt/endAt` 必填，`endAt > startAt`
- `startAt >= 当前时间`
- `reason` 长度 10~500
- `durationDays` 后端自动计算，最小 0.5

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "id": "L202604070001",
    "status": "Draft"
  }
}
```

### API-E2 编辑请假单
- Method: `PUT`
- Path: `/leaves/{leaveId}`
- 权限: Employee(仅本人，且状态为 Draft/Rejected)

#### 输入
```json
{
  "leaveType": "Personal",
  "startAt": "2026-04-12T09:00:00+08:00",
  "endAt": "2026-04-12T18:00:00+08:00",
  "reason": "办理个人事务，预计一天",
  "saveAsDraft": true
}
```

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "id": "L202604070001",
    "status": "Draft",
    "updatedAt": "2026-04-07T11:00:00+08:00"
  }
}
```

### API-E3 删除草稿
- Method: `DELETE`
- Path: `/leaves/{leaveId}`
- 权限: Employee(仅本人，且状态必须是 Draft)

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "deleted": true
  }
}
```

### API-E4 提交请假单
- Method: `POST`
- Path: `/leaves/{leaveId}/submit`
- 权限: Employee(仅本人，状态 Draft)

#### 输入
```json
{}
```

#### 业务校验
- 状态必须为 `Draft`
- 若 `leaveType = Annual`，必须进行额度校验(后端二次校验)

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "id": "L202604070001",
    "status": "Pending"
  }
}
```

#### 额度不足错误示例
```json
{
  "code": "LEAVE_QUOTA_EXCEEDED",
  "message": "年假额度不足",
  "errors": [
    {
      "field": "leaveType",
      "detail": "申请 3.0 天, 剩余 2.0 天"
    }
  ]
}
```

### API-E5 撤回请假单
- Method: `POST`
- Path: `/leaves/{leaveId}/cancel`
- 权限: Employee(仅本人，状态 Pending)

#### 输入
```json
{
  "reason": "计划有变，取消请假"
}
```

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "id": "L202604070001",
    "status": "Cancelled"
  }
}
```

### API-E6 我的请假列表
- Method: `GET`
- Path: `/leaves/my`
- 权限: Employee

#### Query 参数
- `status` 可选，`Draft|Pending|Approved|Rejected|Cancelled`
- `pageNo` 默认 `1`
- `pageSize` 默认 `10`

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "items": [
      {
        "id": "L202604070001",
        "leaveType": "Annual",
        "startAt": "2026-04-10T09:00:00+08:00",
        "endAt": "2026-04-11T18:00:00+08:00",
        "durationDays": 2.0,
        "status": "Pending",
        "updatedAt": "2026-04-07T10:30:00+08:00"
      }
    ],
    "pageNo": 1,
    "pageSize": 10,
    "total": 1
  }
}
```

### API-E7 我的请假单详情
- Method: `GET`
- Path: `/leaves/{leaveId}`
- 权限: Employee(仅本人)

#### 输出
- 返回 `LeaveApplication` 对象，结构见 6.1。

### API-E8 我的余额查询
- Method: `GET`
- Path: `/quotas/my`
- 权限: Employee

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": [
    {
      "leaveType": "Annual",
      "totalDays": 10.0,
      "usedDays": 2.0,
      "remainingDays": 8.0
    },
    {
      "leaveType": "Sick",
      "totalDays": 8.0,
      "usedDays": 1.0,
      "remainingDays": 7.0
    }
  ]
}
```

---

## 7.2 经理端 API (Manager)

### API-M1 待审批列表
- Method: `GET`
- Path: `/approvals/pending`
- 权限: Manager

#### Query 参数
- `employeeId` 可选
- `pageNo` 默认 `1`
- `pageSize` 默认 `10`

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "items": [
      {
        "id": "L202604070001",
        "employeeId": "U1001",
        "employeeName": "张三",
        "leaveType": "Annual",
        "durationDays": 2.0,
        "startAt": "2026-04-10T09:00:00+08:00",
        "endAt": "2026-04-11T18:00:00+08:00",
        "status": "Pending"
      }
    ],
    "pageNo": 1,
    "pageSize": 10,
    "total": 1
  }
}
```

### API-M2 已处理列表
- Method: `GET`
- Path: `/approvals/history`
- 权限: Manager

#### Query 参数
- `result` 可选，`Approved|Rejected`
- `pageNo` 默认 `1`
- `pageSize` 默认 `10`

#### 输出
- 与 API-M1 类似，增加 `managerComment` 与 `approvedAt/rejectedAt` 字段。

### API-M3 审批详情
- Method: `GET`
- Path: `/approvals/{leaveId}`
- 权限: Manager

#### 输出
- 返回完整 `LeaveApplication`，用于审批弹窗展示。

### API-M4 审批通过
- Method: `POST`
- Path: `/approvals/{leaveId}/approve`
- 权限: Manager(状态必须 Pending)

#### 输入
```json
{
  "comment": "同意，请安排好工作交接"
}
```

#### 输入校验
- `comment` 必填，建议 2~200 字

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "id": "L202604070001",
    "status": "Approved",
    "managerComment": "同意，请安排好工作交接"
  }
}
```

### API-M5 打回申请
- Method: `POST`
- Path: `/approvals/{leaveId}/reject`
- 权限: Manager(状态必须 Pending)

#### 输入
```json
{
  "comment": "请补充更详细请假原因"
}
```

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "id": "L202604070001",
    "status": "Rejected",
    "managerComment": "请补充更详细请假原因"
  }
}
```

#### 说明
根据需求状态机，`Pending -> Rejected` 后，业务语义上回到员工可编辑状态(前端按草稿能力处理)。

---

## 7.3 管理员端 API (Admin)

### API-A1 全员额度列表
- Method: `GET`
- Path: `/quotas`
- 权限: Admin

#### Query 参数
- `employeeId` 可选
- `leaveType` 可选
- `pageNo` 默认 `1`
- `pageSize` 默认 `20`

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "items": [
      {
        "employeeId": "U1001",
        "employeeName": "张三",
        "leaveType": "Annual",
        "totalDays": 10.0,
        "usedDays": 2.0,
        "remainingDays": 8.0,
        "updatedAt": "2026-04-07T09:00:00+08:00"
      }
    ],
    "pageNo": 1,
    "pageSize": 20,
    "total": 1
  }
}
```

### API-A2 调整员工额度
- Method: `POST`
- Path: `/quotas/adjust`
- 权限: Admin

#### 输入
```json
{
  "employeeId": "U1001",
  "leaveType": "Annual",
  "adjustMode": "SetTotal",
  "value": 12.0,
  "remark": "年度额度调整"
}
```

#### 输入说明
- `adjustMode`: `SetTotal | Increase | Decrease`
- `value`:
  - `SetTotal` 时表示新总额度
  - `Increase/Decrease` 时表示增减天数
- `remark` 建议必填，便于审计

#### 输出
```json
{
  "code": "0",
  "message": "success",
  "data": {
    "employeeId": "U1001",
    "leaveType": "Annual",
    "totalDays": 12.0,
    "usedDays": 2.0,
    "remainingDays": 10.0,
    "updatedAt": "2026-04-07T14:00:00+08:00"
  }
}
```

---

## 8. 状态流转约束

- `Draft -> Pending` 员工提交
- `Pending -> Approved` 经理通过
- `Pending -> Rejected` 经理打回
- `Pending -> Cancelled` 员工撤回
- `Rejected -> Draft` 通过员工编辑行为进入草稿态(实现可在更新接口内处理)

非法流转统一返回:
```json
{
  "code": "INVALID_STATUS_TRANSITION",
  "message": "当前状态不允许该操作"
}
```

## 9. 页面与 API 对应关系

- 员工工作台 `/employee/dashboard`
  - `GET /quotas/my`
  - `GET /leaves/my`
  - `POST /leaves`
  - `PUT /leaves/{leaveId}`
  - `DELETE /leaves/{leaveId}`
  - `POST /leaves/{leaveId}/submit`
  - `POST /leaves/{leaveId}/cancel`

- 经理审批中心 `/manager/approvals`
  - `GET /approvals/pending`
  - `GET /approvals/history`
  - `GET /approvals/{leaveId}`
  - `POST /approvals/{leaveId}/approve`
  - `POST /approvals/{leaveId}/reject`

- 管理员额度管理 `/admin/quotas`
  - `GET /quotas`
  - `POST /quotas/adjust`

## 10. 兼容与扩展建议

- 建议增加 `GET /meta/leave-types` 暴露枚举，减少前后端硬编码
- 建议在审批与额度调整接口写入审计日志(AuditLog)
- 建议所有列表接口统一分页结构，便于前端复用
