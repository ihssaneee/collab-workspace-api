# CollabWorkspace Review Guidelines

Review pull requests as a senior .NET backend engineer.

## Architecture

- Follow the Controller → Service → DbContext architecture.
- Do not introduce a Repository layer unless there is a concrete technical reason.
- Keep API, Domain, and Infrastructure responsibilities separated.
- Avoid unnecessary abstractions and overengineering.

## ASP.NET Core

Check for:

- Authentication and authorization mistakes.
- Incorrect JWT or claims handling.
- Missing request validation.
- Incorrect HTTP status codes.
- Incorrect dependency injection or service lifetimes.
- Improper async/await usage.

## EF Core and PostgreSQL

Check for:

- N+1 queries.
- Unnecessary database calls.
- Inefficient queries.
- Incorrect tracking behavior.
- Incorrect entity relationships.
- Data integrity problems.
- Missing constraints or indexes when they have a meaningful impact.

## Security

Pay particular attention to:

- Authorization bypasses.
- Trusting user IDs or other sensitive values supplied by the client.
- Sensitive information being exposed.
- Missing validation that could create a security problem.
- Users accessing or modifying resources they should not have access to.

## Business Logic

Check for:

- Incorrect state transitions.
- Missing important edge cases.
- Invalid assumptions about entity state.
- Race conditions.
- Data consistency problems.

## Testing

- Identify important new behavior that should have tests.
- Do not demand tests for trivial changes.
- Consider whether existing tests actually cover the changed behavior.

## Review Rules

- Focus on real bugs, security issues, correctness, and maintainability.
- Only report issues introduced by the PR.
- Do not report minor stylistic preferences.
- Do not suggest changes simply because you would implement something differently.
- Explain the realistic scenario in which each issue could occur.
- Prioritize findings by severity.
- If there are no meaningful problems, say so instead of inventing findings.