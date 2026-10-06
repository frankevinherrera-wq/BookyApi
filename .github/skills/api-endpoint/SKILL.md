---
name: api-endpoint
description: Guide for creating REST API endpoints following the project's architecture.
---

# API Endpoint Development

## Procedure

1. Inspect the existing Controller.
2. Inspect the corresponding Service.
3. Inspect the relevant DTOs.
4. Follow the project's existing architecture.
5. Make the minimum required change.
6. Add or update tests when appropriate.
7. Run the build and relevant tests.
8. Review the git diff.
9. Report the files modified and the verification performed.

## Verification

After implementing the endpoint:

1. Run `dotnet build --no-restore`.
2. Run relevant tests if they exist.
3. If the endpoint is HTTP-based, verify the endpoint behavior when appropriate.
4. Review `git diff`.
5. Report which files were modified.
6. Report the verification performed and its result.

## Constraints

- Do not introduce a Repository layer.
- Controllers must not access ApplicationDbContext directly.
- Keep existing API routes unless the task requires changing them.
- Avoid unrelated refactors.