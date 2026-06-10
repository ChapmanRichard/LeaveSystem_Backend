---
name: asl-dotnet-generate-models-from-db-design
description: "Use when: 需要根据任意数据库设计文档（Markdown/表格/文本）生成或更新 C# Model/Entity，包含字段类型、可空性、关系、约束校验与差异报告。关键词: 数据库设计, Model 生成, Entity, EF Core, schema to model"
---

# 根据数据库设计文档生成 Model

## 意图

该 Skill 用于基于任意数据库设计文档生成模型定义，适用于首次建模、模型同步、结构变更补丁。

目标结果:
- 从文档中提取表、字段、主外键、可空、长度、精度、默认值和值域。
- 生成或更新对应 C# Model/Entity，保证字段语义与数据库设计一致。
- 输出差异报告、假设说明和验证结果，降低遗漏与误改风险。

## 输入

必需输入:
- 一份数据库设计文档路径（建议为 Markdown）

可选输入:
- 目标输出层（如 `Api.Contract/Model`、`Api.DAL`）
- 已存在模型目录（用于差异更新）
- 技术约束（例如：是否允许 enum、是否使用 DataAnnotations、是否有 nullable context）

默认示例输入（仅示例，不是绑定规则）:
- `Spec/Db.md`

## 规则

1. 单一事实来源
- 始终以“用户指定的数据库设计文档”作为主依据。
- 如果现有代码与文档冲突，优先遵循文档并输出差异说明。

2. 命名与结构
- 表名转类名：默认使用单数 PascalCase（可按项目约定覆盖）。
- 字段名转属性名：PascalCase。
- 禁止无依据重命名；若命名冲突需显式说明处理策略。

3. 类型映射（MySQL 8 -> C#）
- `INT` -> `int`
- `VARCHAR(n)` -> `string`
- `DATETIME` -> `DateTime`
- `DECIMAL(5,1)` -> `decimal`
- 其他数据库类型按最近似 C# 类型映射，并在输出中写明映射依据。

4. 可空性映射
- 数据库非空字段 -> C# 非可空属性（或按项目约定使用 required/init）。
- 数据库可空字段 -> C# 可空属性，如 `int?`、`string?`、`DateTime?`。
- 若文档未明确可空性，必须在“假设说明”中标记待确认。

5. 精度与领域约束
- 小数精度字段必须使用 `decimal`，不得降级为 float/double。
- 长度约束、范围约束、枚举值约束应在模型或校验器体现。
- 若约束只能在数据库层实现，也需在输出中标注。

6. 关系建模
- 明确 1-1、1-n、n-n 关系及其外键来源。
- 必须补充导航属性；否则保留外键字段并说明。

7. 状态和值域一致性
- 文档定义的固定值集合应优先生成 enum 或常量集合。
- 如果受兼容性约束无法用 enum，保留 string 并增加校验逻辑。

8. 时间字段
- 统一识别创建时间、更新时间、软删除时间等语义字段。
- 默认值（如 `CURRENT_TIMESTAMP`）需通过迁移或 DB 配置对齐。

9. 兼容性
- 未经明确迁移说明，不要对既有公开契约模型做破坏性重命名。
- 命名空间与文件落位必须符合当前解决方案约定。
- 当数据库设计收敛了历史上的多余字段时，优先以数据库设计为准同步删除或替换实体属性；若会影响现有编译或运行链路，必须同步调整依赖代码，并在差异说明中标注兼容处理方案。
- 不要为了“先让编译通过”而保留未出现在设计文档中的持久化字段；若确实需要临时兼容，必须显式标记为过渡性方案并说明后续移除计划。

## 工作流

1. 读取并解析结构
- 读取用户指定文档并结构化提取 schema 信息。
- 若文档格式不标准，先进行字段标准化再映射。

2. 盘点现有代码
- 检查 Contract 和 DAL 是否已存在对应模型。
- 识别现有命名风格、nullable 上下文与注解习惯。

3. 生成模型定义
- 按“表 -> 类、字段 -> 属性”生成或更新模型。
- 对主键、外键、索引相关字段保留清晰语义。

4. 增加校验与领域约束
- 按当前架构选择 DataAnnotations 或 Validator。
- 确保字符串长度、可空性、值域与目标文档一致。

5. 校验映射一致性
- 确保 DTO/Entity 映射可覆盖所有字段。
- 确保高精度小数字段未被误用为 float/double。
- 检查新增字段是否影响序列化、映射与查询逻辑。

6. 输出结果摘要
- 列出创建/修改文件。
- 列出与目标数据库设计文档的差异。
- 列出需要确认的假设。

## 参考用例

### 用例 1：首次生成模型（任意项目）
提示词示例:
- "读取 docs/database-design.md，在 Contract 层创建对应模型，类型、可空、长度约束必须准确。"

预期结果:
- 按文档中的所有表生成模型类。
- 字段约束完整映射。

### 用例 2：同步已存在模型
提示词示例:
- "对比现有实体与新的数据库设计文档，仅修复缺失或错误属性，保持最小 diff。"

预期结果:
- 以最小变更完成修复。
- 不引入无关命名或风格改动。

### 用例 3：安全引入枚举
提示词示例:
- "若与现有 API 契约兼容，将固定值字段改为 enum；否则保留 string 并补充常量校验。"

预期结果:
- 优先类型安全方案。
- 保证向后兼容。

## 验证清单

在提交前逐项核对:

- [ ] 目标数据库设计文档中的所有表都已映射为模型类。
- [ ] 文档字段在模型层逐一覆盖且无重复遗漏。
- [ ] C# 类型与数据库类型匹配（如 `DECIMAL(5,1)` -> `decimal`，`DATETIME` -> `DateTime`）。
- [ ] 可空字段标记正确。
- [ ] 长度约束正确体现。
- [ ] 固定值约束已落实（枚举或校验器）。
- [ ] 关系属性与外键定义一致。
- [ ] 未引入破坏性契约变更。
- [ ] 文件路径与命名空间符合现有项目约定。
- [ ] 若现有代码与目标文档不一致，输出中包含差异说明。
- [ ] 若移除或新增字段会影响现有调用方，已同步更新依赖代码并记录兼容策略。
- [ ] 不存在未在数据库设计文档中定义、却被误当作持久化字段保留的属性。

## 输出模板

运行本 Skill 时，输出按以下结构组织:

1. 创建/修改文件
- 列出精确路径

2. 模型摘要
- 每个类一段，说明关键字段

3. 约束覆盖说明
- 类型、可空、长度、值域、关系校验结果

4. 差异与假设
- 与目标数据库设计文档的差异
- 需要用户确认的假设

## English Version

### Intent

This skill generates or updates C# models/entities from any database design document (Markdown/table/text), and is suitable for first-time modeling, schema sync, and incremental patching.

Expected outcomes:
- Extract tables, columns, PK/FK, nullability, length, precision, default values, and fixed value sets.
- Generate or update C# models/entities that stay aligned with the schema design.
- Produce mismatch reports, assumptions, and validation results.

### Rules

1. Source of truth
- Always use the user-provided database design document as the primary source.
- If existing code conflicts with the document, prefer the document and report the mismatch.

2. Naming
- Table to class: singular PascalCase by default (override with project conventions if needed).
- Column to property: PascalCase.

3. Type mapping
- Keep precise numeric fields as `decimal`.
- Preserve DB-to-C# mapping rationale in output when types are ambiguous.

4. Nullability
- Non-null DB columns map to non-null C# properties.
- Nullable DB columns map to nullable C# properties.
- If nullability is not explicit in the document, record it as an assumption.

5. Constraints
- Reflect length/range/fixed-value constraints via annotations or validators.
- If constraints can only be enforced at DB level, explicitly note that in the output.

6. Relationships
- Model 1-1, 1-n, and n-n based on FK definitions.
- Add navigation properties only if that matches current project style.

7. Compatibility
- Avoid breaking renames in public contract models unless migration is explicitly requested.
- Prefer minimal diffs when updating existing models.

### Workflow

1. Parse schema from the given design document.
2. Inspect existing model/entity code and conventions.
3. Generate or patch model classes.
4. Add validations and domain constraints.
5. Verify mapping consistency and precision safety.
6. Output changed files, mismatch report, and assumptions.

### Validation Checklist

- [ ] All tables in the target design document are represented as models.
- [ ] All documented columns are covered exactly once.
- [ ] Type mapping is correct, especially decimal/time fields.
- [ ] Nullability is correctly represented.
- [ ] Length and fixed-value constraints are enforced.
- [ ] Relationship properties align with FK definitions.
- [ ] No unintended breaking contract changes are introduced.
- [ ] Output includes mismatch notes and assumptions.
