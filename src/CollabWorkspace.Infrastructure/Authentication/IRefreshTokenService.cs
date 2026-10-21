namespace CollabWorkspace.Infrastructure.Authentication
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenIssue> CreateAsync(
        Guid userId,
        Guid? familyId = null,
        CancellationToken cancellationToken = default);
        Task<RefreshTokenRotation?> RotateAsync(
        string rawToken,
        CancellationToken cancellationToken = default);
    }
}