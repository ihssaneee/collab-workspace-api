using CollabWorkspace.Application.DTOs.WorkspaceMember;

namespace CollabWorkspace.Application.DTOs.Workspace;

public record WorkspaceWithRosterDto(
    Guid Id,
    string Name,
    Guid OwnerId,
    IReadOnlyList<MemberNicknameDto> Members,
    DateTime CreatedAt,
    DateTime UpdatedAt
);