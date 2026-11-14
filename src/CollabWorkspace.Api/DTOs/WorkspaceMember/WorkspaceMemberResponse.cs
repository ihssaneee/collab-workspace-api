namespace CollabWorkspace.Api.DTOs.WorkspaceMember
{
    public record WorkspaceMemberResponse(
        Guid Id,
        Guid WorkspaceId,
        Guid UserId,
        string Role,
        DateTime JoinedAt
    );
}