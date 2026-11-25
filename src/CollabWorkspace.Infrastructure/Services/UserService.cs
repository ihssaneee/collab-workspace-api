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

        //get a list of names of users given a list of user ids
        public async Task<IReadOnlyDictionary<Guid, string?>> GetUserNamesAsync(
             IEnumerable<Guid> userIds,
            CancellationToken cancellationToken = default)
        {
            var distinctIds = userIds.Distinct().ToList();
            if (distinctIds.Count == 0)
            {
                return new Dictionary<Guid, string?>();
            }

            return await _dbContext.Users
                .AsNoTracking()
                .Where(u => distinctIds.Contains(u.Id))
                .Select(u => new { u.Id, u.UserName })
                .ToDictionaryAsync(u => u.Id, u => u.UserName, cancellationToken);
        }

    }
}