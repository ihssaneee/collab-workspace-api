using System.Security.Claims;
namespace CollabWorkspace.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            var userId = user.FindFirstValue("sub");
            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }
    }
}