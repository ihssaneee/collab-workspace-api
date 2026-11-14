using CollabWorkspace.Api.DTOs.WorkspaceMember;
namespace CollabWorkspace.Api.DTOs.Workspace;

public record WorkspaceResponse(
    Guid Id,
    string Name,
    Guid OwnerId,
    IReadOnlyList<WorkspaceMemberResponse> Members,
    DateTime CreatedAt,
    DateTime UpdatedAt
);