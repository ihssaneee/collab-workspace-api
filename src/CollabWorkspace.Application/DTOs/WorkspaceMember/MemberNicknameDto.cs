namespace CollabWorkspace.Application.DTOs.WorkspaceMember
{
    public record MemberNicknameDto(
        Guid UserId,
        string Nickname
    );
}