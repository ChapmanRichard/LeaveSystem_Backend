---
name: asl-dotnet-generate-models-from-db-design
description: "Use when: you need to generate or update C# Models/Entities from any database design document (Markdown/table/text), including field types, nullability, relationships, constraint validation, and difference reports. Keywords: database design, Model generation, Entity, EF Core, schema to model"
---

# Generate Models from Database Design Documents

## Intent

This Skill is used to generate model definitions from any database design document. It is suitable for first-time modeling, model synchronization, and structural change patches.

Expected outcomes:
- Extract tables, fields, primary/foreign keys, nullability, length, precision, default values, and value domains from the document.
- Generate or update corresponding C# Models/Entities, ensuring field semantics are consistent with the database design.
- Output difference reports, assumption notes, and validation results to reduce missed items and accidental changes.

## Input

Required input:
- A database design document path, preferably Markdown

Optional input:
- Target output layer, such as `Api.Contract/Model` or `Api.DAL`
- Existing model directory, used for differential updates
- Technical constraints, such as whether enum is allowed, whether DataAnnotations are used, and whether nullable context exists

Default sample input (example only, not a binding rule):
- `Spec/Db.md`

## Rules

1. Single source of truth
- Always use the “user-specified database design document” as the primary basis.
- If existing code conflicts with the document, prefer the document and output difference notes.

2. Naming and structure
- Table name to class name: singular PascalCase by default, overrideable by project conventions.
- Field name to property name: PascalCase.
- Renaming without basis is prohibited; if naming conflicts occur, explicitly explain the handling strategy.

3. Type mapping (MySQL 8 -> C#)
- `INT` -> `int`
- `VARCHAR(n)` -> `string`
- `DATETIME` -> `DateTime`
- `DECIMAL(5,1)` -> `decimal`
- Map other database types to the closest C# type and state the mapping rationale in the output.

4. Nullability mapping
- Non-null database fields -> non-null C# properties, or required/init according to project conventions.
- Nullable database fields -> nullable C# properties, such as `int?`, `string?`, and `DateTime?`.
- If the document does not explicitly state nullability, mark it as pending confirmation in the “Assumptions” section.

5. Precision and domain constraints
- Decimal precision fields must use `decimal` and must not be downgraded to float/double.
- Length constraints, range constraints, and enum value constraints should be represented in models or validators.
- If a constraint can only be implemented at the database layer, note it in the output.

6. Relationship modeling
- Clearly identify 1-1, 1-n, and n-n relationships and their foreign key sources.
- Navigation properties must be added; otherwise, keep foreign key fields and explain why.

7. State and value-domain consistency
- Fixed value sets defined by the document should preferably generate enums or constant collections.
- If compatibility constraints prevent using enum, keep string and add validation logic.

8. Time fields
- Uniformly identify semantic fields such as created time, updated time, and soft-deleted time.
- Default values such as `CURRENT_TIMESTAMP` must be aligned through migrations or DB configuration.

9. Compatibility
- Do not perform breaking renames on existing public contract models without explicit migration instructions.
- Namespaces and file placement must comply with the current solution conventions.
- When the database design converges historically redundant fields, prioritize the database design and synchronously delete or replace entity properties. If this affects existing compilation or runtime chains, update dependent code as well and note the compatibility handling solution in the difference explanation.
- Do not keep persistent fields that do not appear in the design document merely to “make compilation pass first”. If temporary compatibility is truly needed, explicitly mark it as a transitional solution and explain the later removal plan.

## Workflow

1. Read and parse structure
- Read the user-specified document and structurally extract schema information.
- If the document format is non-standard, normalize fields before mapping.

2. Inventory existing code
- Check whether corresponding models already exist in Contract and DAL.
- Identify existing naming style, nullable context, and annotation habits.

3. Generate model definitions
- Generate or update models according to “table -> class, field -> property”.
- Preserve clear semantics for fields related to primary keys, foreign keys, and indexes.

4. Add validation and domain constraints
- Choose DataAnnotations or Validator according to the current architecture.
- Ensure string length, nullability, and value domains match the target document.

5. Validate mapping consistency
- Ensure DTO/Entity mappings can cover all fields.
- Ensure high-precision decimal fields are not mistakenly used as float/double.
- Check whether newly added fields affect serialization, mapping, and query logic.

6. Output result summary
- List created/modified files.
- List differences from the target database design document.
- List assumptions requiring confirmation.

## Reference Cases

### Case 1: First-time model generation (any project)
Prompt example:
- "Read docs/database-design.md and create corresponding models in the Contract layer. Types, nullability, and length constraints must be accurate."

Expected results:
- Generate model classes for all tables in the document.
- Fully map field constraints.

### Case 2: Synchronize existing models
Prompt example:
- "Compare existing entities with the new database design document, fix only missing or incorrect properties, and keep the diff minimal."

Expected results:
- Complete fixes with minimal changes.
- Do not introduce unrelated naming or style changes.

### Case 3: Safely introduce enums
Prompt example:
- "If compatible with the existing API contract, convert fixed-value fields to enum; otherwise keep string and add constant validation."

Expected results:
- Prefer a type-safe solution.
- Ensure backward compatibility.

## Validation Checklist

Check each item before submission:

- [ ] All tables in the target database design document have been mapped to model classes.
- [ ] Documented fields are covered one by one at the model layer with no duplicates or omissions.
- [ ] C# types match database types, such as `DECIMAL(5,1)` -> `decimal` and `DATETIME` -> `DateTime`.
- [ ] Nullable fields are marked correctly.
- [ ] Length constraints are correctly represented.
- [ ] Fixed-value constraints are implemented, through enum or validator.
- [ ] Relationship properties align with foreign key definitions.
- [ ] No breaking contract changes are introduced.
- [ ] File paths and namespaces comply with existing project conventions.
- [ ] If existing code differs from the target document, the output includes difference notes.
- [ ] If removing or adding fields affects existing callers, dependent code has been updated and compatibility strategy has been recorded.
- [ ] There are no properties that are not defined in the database design document but are mistakenly retained as persistent fields.

## Output Template

When running this Skill, organize output using the following structure:

1. Created/modified files
- List exact paths

2. Model summary
- One paragraph per class explaining key fields

3. Constraint coverage notes
- Type, nullability, length, value domain, and relationship validation results

4. Differences and assumptions
- Differences from the target database design document
- Assumptions requiring user confirmation

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
