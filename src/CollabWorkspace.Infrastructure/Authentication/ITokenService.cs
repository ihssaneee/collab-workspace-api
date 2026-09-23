using CollabWorkspace.Infrastructure.Identity;
namespace CollabWorkspace.Infrastructure.Authentication
{
    public interface ITokenService
    {
        string GenerateToken(ApplicationUser user);
    }
}