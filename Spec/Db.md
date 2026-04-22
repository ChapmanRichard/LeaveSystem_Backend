### 1. 数据库表结构设计

#### 表 1：Users (用户表 - 用于硬编码预设账号)
虽然不做用户管理模块，但系统直接在底层预设5个测试账号，包括3名普通员工、1名经理和1名系统管理员。我们需要一个基础表来存储这些角色身份。

| 字段名 | 数据类型 (MySQL 8) | 约束 / 备注 |
| ------ | ------ | ------ |
| **Id** | INT | 主键, **AUTO_INCREMENT** (替换了 SQL Server 的 IDENTITY) |
| **Username** | **VARCHAR(50)** | 必填项 (例如: Employee1, Manager1, Admin1) |
| **Role** | **VARCHAR(20)** | 必填项，存储角色标识 (例如: 'Employee', 'Manager', 'Admin') |

#### 表 2：LeaveQuotas (假期额度表)
系统需要支持假期额度管理，管理员负责手动设置或调整员工的假期总额度，而员工可以查询自己的剩余假期。此表主要用于“年假专项”等额度的双端校验。

| 字段名 | 数据类型 (MySQL 8) | 约束 / 备注 |
| ------ | ------ | ------ |
| **Id** | INT | 主键, **AUTO_INCREMENT** |
| **UserId** | INT | 外键，关联 Users.Id |
| **LeaveType** | **VARCHAR(20)** | 必填项，请假类型（如：'年假', '调休'） |
| **TotalDays** | DECIMAL(5,1) | 必填项，管理员设定的该假期总额度 |
| **RemainingDays** | DECIMAL(5,1) | 必填项，当前可用/剩余的天数（每次审批通过后扣减） |

#### 表 3：LeaveRequests (请假申请单表)
这张表是核心业务表，完整映射了需求中的数据字段、基础校验规范以及审批流转的记录需求。

| 字段名 | 数据类型 (MySQL 8) | 约束 / 备注 |
| ------ | ------ | ------ |
| **Id** | INT | 主键, **AUTO_INCREMENT** |
| **ApplicantId** | INT | 外键，关联 Users.Id (申请人/普通员工) |
| **LeaveType** | **VARCHAR(20)** | 必填项，枚举值：'年假'、'病假'、'事假'、'调休' |
| **StartTime** | **DATETIME** | 必填项，开始时间 |
| **EndTime** | **DATETIME** | 必填项，结束时间 |
| **Duration** | DECIMAL(5,1) | 必填项，自动计算的时长，单位：天 (最小 0.5 天) |
| **Reason** | **VARCHAR(500)** | 必填项，请假原因（代码层面控制最小 10 个字符，最大 500 字） |
| **Status** | **VARCHAR(20)** | 必填项，状态：'Draft'(草稿)、'Pending'(待审批)、'Approved'(已批准)、'Rejected'(已打回)、'Cancelled'(已撤销) |
| **ApproverId** | INT | 外键，关联 Users.Id (审批人/经理)，允许为 NULL |
| **ApprovalComment** | **VARCHAR(500)** | 允许为 NULL，经理执行“审批通过/打回”时必须填写的审批意见 |
| **CreatedAt** | **DATETIME** | 默认值 **CURRENT_TIMESTAMP** (替换了 SQL Server 的 GETDATE())，创建时间 |
| **UpdatedAt** | **DATETIME** | 记录最后一次更新（如修改草稿、审批完成等）的时间 |

---

### 2. 核心关系与业务落地说明

1.  **关系映射 (Relationships)**：
    *   一个 User (普通员工) 对应多个 LeaveQuotas (多种假期类型的余额)。
    *   一个 User (普通员工) 对应多个 LeaveRequests (申请记录)。
    *   一个 LeaveRequests 记录，除了拥有 ApplicantId，在流转到经理审批后，还会被标记上 ApproverId，以此记录是谁处理了这条申请。
2.  **数据精度把控与 MySQL 8 适配**：
    *   对于请假时长，由于需求明确规定“最小 0.5 天”，**我们继续沿用了 `DECIMAL(5,1)` 数据类型，这样可以完美支持如 1.5、0.5 这样的半天请假数值，防止精度丢失**。
    *   **字符串类型调整**：将 SQL Server 特有的 `NVARCHAR` 统一调整为了 MySQL 的 `VARCHAR`。
    *   **高精度时间与默认值**：所有时间均从 SQL Server 的 `DATETIME2` 调整为了 MySQL 的 `DATETIME` 类型。同时，由于更换了数据库环境，获取当前时间的函数也从 `GETDATE()` 变更为 MySQL 标准的 `CURRENT_TIMESTAMP`。