# BookfyApi - Agent Instructions

## Architecture

The project follows this architecture:

Controller → Service → EF Core → Database

- Controllers handle HTTP requests and responses.
- Services contain business logic and perform database access.
- ApplicationDbContext is used through Services.
- DTOs are used for API request and response contracts.

## Rules

- Controllers must not access ApplicationDbContext directly.
- Business logic must remain in Services.
- Do not introduce a Repository layer.
- Use DTOs for API contracts.
- Make the smallest necessary change for each task.
- Do not modify unrelated files.
- Do not change existing API routes or contracts unless the task explicitly requires it.