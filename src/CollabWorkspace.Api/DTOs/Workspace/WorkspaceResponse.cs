namespace CollabWorkspace.Api.DTOs.Workspace;

public record WorkspaceResponse(
    Guid Id,
    string Name,
    Guid OwnerId,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc
);