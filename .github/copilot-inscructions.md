## General

* Make only high confidence suggestions when reviewing code changes.
* Always use the latest version C#, currently C# 13 features.
* Never change global.json unless explicitly asked to.
* Never change package.json or package-lock.json files unless explicitly asked to.
* Never change NuGet.config files unless explicitly asked to.
* Follow the [.NET coding guidelines](https://github.com/dotnet/runtime/blob/main/docs/coding-guidelines/coding-style.md) unless explicitly overridden below.
* Write code that is clean, maintainable, and easy to understand.
* Favor readability over brevity, but keep methods focused and concise.
* Only add comments rarely to explain why a non-intuitive solution was used. The code should be self-explanatory otherwise.
* Don't add the UTF-8 BOM to files unless they have non-ASCII characters.

## Formatting

* Prefer file-scoped namespace declarations and single-line using directives.
* Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
* Ensure that the final return statement of a method is on its own line.
* Use pattern matching and switch expressions wherever possible.
* Use `nameof` instead of string literals when referring to member names.
* Ensure that XML doc comments are created for any public APIs. When applicable, include `<example>` and `<code>` documentation in the comments.
* Use spaces for indentation (4 spaces).
* Use braces for all blocks including single-line blocks.
* Limit line length to 140 characters.
* Trim trailing whitespace.
* All declarations must begin on a new line.
* Use a single blank line to separate logical sections of code when appropriate.
* Insert a final newline at the end of files.

### C# Specific Guidelines

* Use `var` for local variables
* Use expression-bodied members where appropriate
* Prefer using collection expressions when possible
* Use `is` pattern matching instead of `as` and null checks
* Prefer `switch` expressions over `switch` statements when appropriate
* Prefer field-backed property declarations using field contextual keyword instead of an explicit field.
* Prefer range and index from end operators for indexer access
* The projects use implicit namespaces, so do not add `using` directives for namespaces that are already imported by the project
* When verifying that a file doesn't produce compiler errors rebuild the whole project

### Naming Conventions

* Use PascalCase for:
  * Classes, structs, enums, properties, methods, events, namespaces, delegates
  * Public fields
  * Static private fields
  * Constants
* Use camelCase for:
  * Parameters
  * Local variables
* Use `_camelCase` for instance private fields
* Prefix interfaces with `I`
* Prefix type parameters with `T`
* Use meaningful and descriptive names

### Nullable Reference Types

* Declare variables non-nullable, and check for `null` at entry points.
* Always use `is null` or `is not null` instead of `== null` or `!= null`.
* Trust the C# null annotations and don't add null checks when the type system says a value cannot be null.
* Use the null-conditional operator (`?.`) and null-coalescing operator (`??`) when appropriate

### Testing

* We use xUnit SDK v3 for tests.
* Do not emit "Act", "Arrange" or "Assert" comments.
* Use Moq for mocking in tests.
* Copy existing style in nearby files for test method names and capitalization.
