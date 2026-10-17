namespace CollabWorkspace.Infrastructure.Authentication;

public sealed record RefreshTokenRotation(
    Guid UserId,
    string RawToken,
    DateTime ExpiresAtUtc);