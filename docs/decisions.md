Three-project backend
Api
Infrastructure
Domain
Domain entities don't depend on EF Core
User, Workspace, WorkspaceMember, etc. remain plain C# classes.
WorkspaceMember represents workspace roles
A role belongs to the user's membership.
A user can therefore be Owner in one workspace and Viewer in another.
WorkspaceRole is an enum
Owner
Editor
Viewer
Kept in Domain/Enums.
Custom authentication instead of ASP.NET Identity
We'll implement JWT authentication ourselves when we reach authentication.
No unnecessary identity framework for the project's current requirements.
Soft deletion
Workspace and Document have DeletedAt.
We preserve records rather than immediately physically deleting them.
Document versioning
Document.Version will support optimistic concurrency when we implement collaborative editing.
PostgreSQL
PostgreSQL is the project's database.
EF Core + Npgsql are used for persistence.
User Secrets
Local database credentials aren't stored in appsettings.json.