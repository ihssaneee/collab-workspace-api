namespace CollabWorkspace.Application.DTOs.WorkspaceMember
{
    public record WorkspaceMemberDto(
        Guid Id,
        Guid WorkspaceId,
        Guid UserId,
        string Role,
        DateTime JoinedAt
    );
}