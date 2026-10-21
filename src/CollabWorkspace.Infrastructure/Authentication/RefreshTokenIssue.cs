
namespace CollabWorkspace.Infrastructure.Authentication;

public sealed record RefreshTokenIssue(
    string RawToken,
    Guid FamilyId,
    DateTime ExpiresAtUtc);
