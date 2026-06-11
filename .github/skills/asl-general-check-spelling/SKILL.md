---
name: asl-general-check-spelling
description: "Use when: you need to run spell checks on a specified code scope, covering Markdown, comments, log copy, and readable strings; output a reviewable misspelling list and fix suggestions, and batch-fix after confirmation. Keywords: spell check, spell check, typo, cspell, typos, markdown, comments"
argument-hint: "Enter the check scope and rules, for example: check Spec/ and Api.Web/Controllers, enable strict mode, and keep the product terminology whitelist"
---

# Spell Check Skill

## Intent

Turn “manually finding typos” into a repeatable spell-check workflow that balances accuracy and implementable fixes.

The goals of this Skill are:
- Identify spelling errors, common typos, casing errors, and terminology mistakes within the user-specified scope
- Scan inappropriate punctuation in code blocks or comments, such as duplicated punctuation, mixed full-width/half-width punctuation, and semantically conflicting punctuation
- Identify spelling accuracy of technical terms, such as Async, Middleware, Repository, and DTO
- Check whether variable names conform to English grammar logic, such as preferring is/has/can prefixes for Boolean variables
- Output an evidence-backed location list, including file, line number, original word, suggested word, and context
- After user confirmation, perform minimal fixes to avoid breaking code behavior

## Applicable Scenarios

- Documentation and comment quality gates before PR merge
- Consistency checks for API response copy, log copy, and prompts
- English spelling and terminology consistency checks in requirements documents, test documents, and technical standards
- Batch typo corrections after refactoring to reduce maintenance noise

## Input

Required input:
- Check scope: files, directories, globs, or change sets, such as a PR diff

Optional input:
- Check mode: `quick` / `standard` / `strict`
- Language and dictionaries: English, Chinese terminology whitelist, project-specific term whitelist
- Check targets: documents + comments + strings by default, with optional identifier grammar checking
- Auto-fix: `true/false` (default: false)
- Output granularity: issue list only / issues + fix patch

## Decision Branches

1. Scope branch
- If the user provides explicit paths, check those paths
- If no scope is provided, check recently changed files by default; then fall back to documentation and interface-layer directories

2. Strictness branch
- `quick`: high-confidence typos only (low false positives)
- `standard`: balanced recall and false positives (default)
- `strict`: includes style consistency (casing, hyphens, abbreviation expansion) + punctuation standards + variable naming grammar logic

3. Dictionary branch
- If a project terminology table exists, load the term whitelist first
- If none exists, first generate a candidate whitelist and ask the user to confirm before persisting it

4. Fix branch
- Auto-fix is disabled by default
- Automatically fix only high-confidence text that does not affect semantics
- For code identifiers, protocol fields, and database field names, provide suggestions only and do not change them automatically

5. Naming grammar branch
- By default, check only documents, comments, and strings
- If the user enables identifier grammar checking, additionally check English semantics in variable naming
- Recommended Boolean variable naming: `is/has/can/should` + past participle or adjective, such as `isDeleted` being preferable to `isDelete`

## Execution Flow

1. Parse the task
- Parse scope, mode, language, and whether auto-fix is enabled
- Identify directories to exclude, such as bin, obj, node_modules, and generated files

2. Collect checkable text
- Extract Markdown body text, code comments, log text, exception messages, and user-facing prompt copy
- Extract punctuation fragments from code blocks and comments for punctuation reasonableness checks
- By default, skip URLs, hashes, GUIDs, version numbers, and secret fragments

3. Run spell scanning
- Prefer stable spell-checking tools, such as cspell or typos
- Merge project terminology whitelist and ignore rules before running the scan

4. Terminology and punctuation special checks
- Use a technical terminology dictionary to validate term spelling, such as Async, Middleware, and Kubernetes
- Identify inappropriate punctuation: duplicated punctuation, such as `。。`, mixed full-width/half-width punctuation, and punctuation combinations in comments that conflict with semantics

5. Normalize and deduplicate
- Merge duplicate issues and normalize word forms, including casing, pluralization, and tense
- Mark confidence and suggested replacements

6. Naming grammar logic check (optional)
- Run English grammar and semantic pattern checks on variable names in the enabled scope
- Key rules: Boolean variable naming, verb-object structure, and tense consistency

7. Generate report
- Sort by severity and fixability
- Each issue includes: file, line number, issue type, original word/original name, suggestions, context, and fix risk

8. Execute fixes (optional)
- Handle only high-confidence, low-risk items
- Generate minimal change patches
- Re-scan after fixing to confirm that issue count decreases and no new parsing errors are introduced

## Output Format

Must output:
1. Check scope and exclusions
2. Spelling issue list, sorted by confidence descending
3. Auto-fix results, if enabled
4. Remaining items requiring manual confirmation
5. Project terms recommended for whitelist inclusion

Recommended issue fields:
- Level: `High` / `Medium` / `Low`
- Location: file + line number
- Issue type: `Spelling` / `Punctuation` / `Terminology` / `NamingGrammar`
- Original word and suggested word
- Context snippet
- Handling recommendation: auto-fix / manual confirmation / add to whitelist

## Quality Standards (Definition of Done)

- Every record in the issue list can be located to a specific file and line number
- Punctuation issues in code blocks and comments can be identified and classified
- Technical terminology mistakes can be identified by the terminology dictionary and given standard spellings
- Variable naming grammar issues, such as `isDelete`, can be identified and suggestions such as `isDeleted` can be provided
- Auto-fix covers only high-confidence, low-risk text and does not change protocol fields or identifiers
- The second scan should introduce no new issues and should show a clear decrease in total issue count
- Output must include “items requiring manual confirmation” and “recommended whitelist items”

## Failure and Fallback Strategy

- If tools are unavailable: provide installation suggestions and first execute a rule-based manual scan, prioritizing comments and documents
- If there are too many false positives: downgrade to quick mode and expand the terminology whitelist
- If an exception occurs after fixing: roll back that batch of changes and keep only the report and candidate patch

## Example Invocations

- /check-spelling Check document spelling under Spec/ and ProjectCustomize/ in standard mode without auto-fix
- /check-spelling Check only comments and strings in Api.Web/Controllers in strict mode and output a fix patch
- /check-spelling Run a quick check on recently changed files and generate terminology whitelist suggestions
- /check-spelling Check variable naming grammar logic under Api.BLL/, identify isDelete-style names, and provide replacement suggestions

## Acceptance Checklist

- [ ] Check scope, mode, and dictionary sources are clearly defined
- [ ] Binary and generated directories are excluded
- [ ] A locatable issue list has been output, including file + line number
- [ ] Punctuation, terminology, and variable naming grammar checks are covered
- [ ] Auto-fix items and manual confirmation items are distinguished
- [ ] Whitelist suggestions and follow-up maintenance strategies are provided
