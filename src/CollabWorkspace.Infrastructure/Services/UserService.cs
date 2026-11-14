using CollabWorkspace.Application.Interfaces;
using CollabWorkspace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CollabWorkspace.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly CollabWorkspaceDbContext _dbContext;


        public UserService(CollabWorkspaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string?> GetUserNameAsync(Guid userId, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Where(u => u.Id == userId)
                .Select(u => u.UserName)
                .FirstOrDefaultAsync(cancellationToken);

            return user;
        }
    }
}