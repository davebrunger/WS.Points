---
description: "Use when writing, reviewing, or refactoring C# code. Covers coding standards, naming conventions, and design patterns for this solution."
applyTo: "**/*.cs"
---
# Coding Standards

## Design Principles

- Use immutability wherever possible; use `init`-only properties and records where appropriate
- Keep methods small and single-purpose

## Code Style

- Always use braces `{}` for control flow blocks (`if`, `else`, `for`, `foreach`, `while`, `do`) even when the body is a single statement
- Use expression-bodied members for simple properties and methods where it enhances readability
- Use `var` when the type is obvious from the right-hand side of the assignment
- Prefer raw string literals (`"""..."""`) or verbatim string literals (`@"..."`) over string concatenation for multiline strings

## Naming Conventions

- Classes, methods, properties, and constants: PascalCase
- Parameters and local variables: camelCase
- Private fields: camelCase with no underscore prefix
- Interfaces: prefixed with `I` (e.g. `IMyService`)
- Type parameters: descriptive names prefixed with `T` (e.g. `TSize`, `TRows`)
- Always use British English spelling for identifiers (e.g. `Colour` instead of `Color`) to maintain consistency across the solution

## File & Namespace Layout

- Place all source project files under a `src/` directory and all test project files under a `test/` directory at the solution root
- All types (except `IsInternalInit`) should be contained in a `namespace` that matches the project name (e.g. `WS.Matrices`, `WS.Kinematics`) plus the folder structure (e.g. `WS.Matrices.Extensions`)
- Place all `using` directives in a separate `GlobalUsings.cs` file at the root of each project
- Ensure that the `GlobalUsings.cs` file includes all necessary namespaces for the project and does not contain any unused usings (use an IDE feature or tool to identify and remove unused usings)
- Ensure that each source file contains no unused `using` directives
- Keep the `GlobalUsings.cs` file organized by grouping related namespaces together and adding comments to separate groups (e.g. `// Roslyn APIs`, `// System namespaces`, `// WS.Dimensions namespaces`), maintain a consistent order (e.g. System namespaces first, then third-party, then project namespaces) and use alphabetical ordering within each group
- Always use file scoped namespaces (`namespace WS.Matrices.Extensions;`) instead of block-scoped namespaces (`namespace WS.Matrices.Extensions { ... }`)
- All program files (executable entry points) should use top-level statements (no `Program` class). Keep the file concise; place small helper functions as local functions within the same file when appropriate.

## Type & Member Organisation

- Order members in the following sequence: fields, properties, constructors, indexers, methods, explicit interface implementations, operators, extension methods
- For member groups except methods, order by accessibility: private, protected, internal, public
- For methods, order by functionality (e.g. public API methods first, then private helper methods)
- Use alphabetical ordering as a tiebreaker within each accessibility level and functionality group

## Documentation

- All public types, members, properties, and extension methods in `src/` projects must have XML documentation comments (`<summary>`, `<param>`, `<returns>`, `<exception>` where applicable)
- Do not add XML documentation comments to test projects
- The solution should have an overall XML documentation file generated on build (set `<GenerateDocumentationFile>true</GenerateDocumentationFile>` in the .csproj files) and all public APIs should be documented to ensure the generated XML is useful for consumers of the library
- The solution should be configured to treat XML documentation warnings as errors (set `<WarningsAsErrors>$(WarningsAsErrors);1591</WarningsAsErrors>` in the .csproj files) to enforce documentation standards
- The solution should have an up to date `README.md` at the root of the repository that provides an overview of the project, its purpose, and how to get started with it.

## Error Handling

- When `WS.DomainModelling.Common` is referenced, use railway-oriented programming (ROP) patterns: prefer `Result<T, TError>` over exceptions for expected failure cases
- Reserve exceptions for truly exceptional/unrecoverable situations

## Testing

- Always use a TDD approach for new features and bug fixes
- Test file mirrors source path under the test project
- One test class per production class