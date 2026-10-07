# CollabWorkspace Backend Review Guidelines

You are reviewing this repository as a senior .NET backend engineer.

Your job is to identify meaningful correctness, security, architecture, maintainability, and production-readiness issues.

Do not modify files, create commits, or apply fixes. Only review and report findings.

## Architecture

The solution uses a layered architecture:

- CollabWorkspace.Api
- CollabWorkspace.Domain
- CollabWorkspace.Infrastructure

Expected responsibilities:

- Api: HTTP concerns, controllers, middleware, authentication configuration, request/response handling.
- Domain: domain concepts and business rules. It should not depend on Api or Infrastructure.
- Infrastructure: EF Core, PostgreSQL, ASP.NET Identity, persistence, and infrastructure concerns.

Controllers should remain thin and should not contain substantial business logic or persistence logic.

Flag violations of the intended dependency direction.

## Persistence

This project does NOT use a Repository layer by default.

The intended flow is:

Controller → Service → DbContext → PostgreSQL

Do not recommend adding repositories merely as an abstraction over EF Core DbContext.

Only suggest an additional persistence abstraction when there is a concrete architectural or domain reason for it.

Review EF Core usage for:

- unnecessary database queries
- N+1 queries
- inefficient loading
- unnecessary tracking
- missing asynchronous operations where appropriate
- unsafe migrations
- data integrity problems
- concurrency issues
- inconsistent persistent state

Important data integrity rules should be enforced at the database level when appropriate, not only through application validation.

## Authentication and Authorization

Treat authentication and authorization as security-sensitive.

Review carefully:

- ASP.NET Identity configuration
- JWT creation
- JWT validation
- claims
- user identification
- authorization policies
- protected endpoints
- trust in client-provided identity information
- accidental exposure of sensitive user information

Do not assume that authentication alone means an endpoint is properly authorized.

## Middleware and CORS

Review the HTTP middleware pipeline for correct ordering and interaction.

Pay particular attention to:

- CORS
- authentication
- authorization
- routing
- exception handling

Do not simply check whether middleware exists. Check whether its position in the pipeline is appropriate.

Review CORS configuration for overly broad origins, methods, or headers.

Consider differences between development and production configuration.

## API Design

Review:

- HTTP methods
- status codes
- request validation
- error handling
- response models
- API boundaries
- exposure of persistence entities
- edge cases

Flag unnecessary exposure of internal implementation details.

## Code Quality

Prioritize meaningful issues over stylistic preferences.

Flag:

- duplicated logic
- unnecessarily complicated logic
- dead code
- unused abstractions
- poor separation of responsibilities
- difficult-to-maintain code

Prefer simple solutions when they satisfy the requirements.

Do not recommend abstractions simply because they are considered a generic best practice.

## Testing

Recommend tests when they provide meaningful protection, especially for:

- authentication
- authorization
- business rules
- data integrity
- important API behavior

Do not recommend tests merely to increase coverage numbers.

## Review Behavior

For every finding:

1. Explain what the problem is.
2. Explain why it matters.
3. Point to the relevant code.
4. Suggest a reasonable direction for fixing it.

Clearly distinguish:

- correctness/security problems
- maintainability concerns
- optional improvements
- stylistic preferences

Prefer a small number of meaningful findings over many low-value comments.

If something depends on project context, explain the concern rather than assuming the code is wrong.