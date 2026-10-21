
using System.Security.Cryptography;
using System.Text;
using CollabWorkspace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CollabWorkspace.Infrastructure.Authentication;

public sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly CollabWorkspaceDbContext _dbContext;

    private static readonly TimeSpan TokenLifetime =
        TimeSpan.FromDays(7);

    private static readonly TimeSpan SessionLifetime =
        TimeSpan.FromDays(30);

    public RefreshTokenService(CollabWorkspaceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshTokenIssue> CreateAsync(
        Guid userId,
        Guid? familyId = null,
        CancellationToken cancellationToken = default)
    {
        var userExists = await _dbContext.Users
            .AnyAsync(u => u.Id == userId, cancellationToken);

        if (!userExists)
        {
            throw new InvalidOperationException(
                "Cannot create a refresh token for a nonexistent user.");
        }

        var now = DateTime.UtcNow;
        var tokenFamilyId = familyId ?? Guid.NewGuid();

        DateTime absoluteExpiresAt;

        if (familyId is null)
        {
            absoluteExpiresAt = now.Add(SessionLifetime);
        }
        else
        {
            var existingDeadline = await _dbContext.RefreshTokens
                .Where(t => t.FamilyId == tokenFamilyId)
                .Select(t => (DateTime?)t.AbsoluteExpiresAtUtc)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingDeadline is null ||
                existingDeadline.Value <= now)
            {
                throw new InvalidOperationException(
                    "The refresh-token family does not exist or has expired.");
            }

            absoluteExpiresAt = existingDeadline.Value;
        }

        var expiresAt = Min(
            now.Add(TokenLifetime),
            absoluteExpiresAt);

        var rawToken = GenerateRawToken();
        var tokenHash = HashToken(rawToken);

        var refreshToken = new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            CreatedAtUtc = now,
            ExpiresAtUtc = expiresAt,
            AbsoluteExpiresAtUtc = absoluteExpiresAt,
            FamilyId = tokenFamilyId
        };

        _dbContext.RefreshTokens.Add(refreshToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RefreshTokenIssue(
            rawToken,
            tokenFamilyId,
            expiresAt);
    }

    public async Task<RefreshTokenRotation?> RotateAsync(
        string rawToken,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(rawToken))
        {
            return null;
        }

        var tokenHash = HashToken(rawToken);

  
        var familyId = await _dbContext.RefreshTokens
            .AsNoTracking()
            .Where(t => t.TokenHash == tokenHash)
            .Select(t => (Guid?)t.FamilyId)
            .FirstOrDefaultAsync(cancellationToken);

        if (familyId is null)
        {
            return null;
        }

        await using var transaction =
            await _dbContext.Database.BeginTransactionAsync(
                cancellationToken);

        try
        {

            var connection = _dbContext.Database.GetDbConnection();

            await using (var command = connection.CreateCommand())
            {
                command.Transaction =
                    transaction.GetDbTransaction();

                command.CommandText =
                    """
                    SELECT pg_advisory_xact_lock(
                        hashtextextended(@familyId, 0)
                    );
                    """;

                var parameter = command.CreateParameter();
                parameter.ParameterName = "familyId";
                parameter.Value = familyId.Value.ToString();
                command.Parameters.Add(parameter);

                await command.ExecuteScalarAsync(cancellationToken);
            }

          
            var storedToken = await _dbContext.RefreshTokens
                .FromSqlInterpolated($"""
                    SELECT *
                    FROM "RefreshTokens"
                    WHERE "TokenHash" = {tokenHash}
                    FOR UPDATE
                    """)
                .SingleOrDefaultAsync(cancellationToken);

            if (storedToken is null)
            {
                await transaction.RollbackAsync(cancellationToken);
                return null;
            }

            var now = DateTime.UtcNow;

           
            if (storedToken.RevokedAtUtc is not null)
            {
                if (storedToken.ReplacedByTokenHash is not null)
                {
                    await RevokeFamilyAsync(
                        storedToken.FamilyId,
                        now,
                        cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                else
                {
                    await transaction.RollbackAsync(cancellationToken);
                }

                return null;
            }

         
            if (storedToken.ExpiresAtUtc <= now ||
                storedToken.AbsoluteExpiresAtUtc <= now)
            {
                storedToken.RevokedAtUtc = now;

                await _dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return null;
            }

       
            var replacementRawToken = GenerateRawToken();
            var replacementHash = HashToken(replacementRawToken);

            var replacementExpiresAt = Min(
                now.Add(TokenLifetime),
                storedToken.AbsoluteExpiresAtUtc);

            var replacement = new RefreshToken
            {
                UserId = storedToken.UserId,
                TokenHash = replacementHash,
                CreatedAtUtc = now,
                ExpiresAtUtc = replacementExpiresAt,
                AbsoluteExpiresAtUtc =
                    storedToken.AbsoluteExpiresAtUtc,
                FamilyId = storedToken.FamilyId
            };

            storedToken.RevokedAtUtc = now;
            storedToken.ReplacedByTokenHash = replacementHash;

            _dbContext.RefreshTokens.Add(replacement);

            await _dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new RefreshTokenRotation(
                replacement.UserId,
                replacementRawToken,
                replacement.ExpiresAtUtc);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task RevokeFamilyAsync(
        Guid familyId,
        DateTime now,
        CancellationToken cancellationToken)
    {
        var activeTokens = await _dbContext.RefreshTokens
            .Where(t =>
                t.FamilyId == familyId &&
                t.RevokedAtUtc == null)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.RevokedAtUtc = now;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static string GenerateRawToken()
    {
        return Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(64))
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }

    private static string HashToken(string rawToken)
    {
        return Convert.ToHexString(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(rawToken)));
    }

    private static DateTime Min(DateTime first, DateTime second)
    {
        return first <= second ? first : second;
    }
}
