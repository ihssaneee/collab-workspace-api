using CollabWorkspace.Domain.Entities;
using CollabWorkspace.Infrastructure.Data;
using  CollabWorkspace.Domain.Enums;
using Microsoft.EntityFrameworkCore;
namespace CollabWorkspace.Infrastructure.Services
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly CollabWorkspaceDbContext _dbContext;
        public WorkspaceService(CollabWorkspaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async  Task<Workspace> CreateWorkspaceAsync( Guid ownerId, string name, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var workspace = new Workspace
            {
                Id = Guid.NewGuid(),
                Name = name,
                OwnerId = ownerId,
                CreatedAt = now,
                UpdatedAt = now
            };

            workspace.Members.Add(new WorkspaceMember
            {
                Id = Guid.NewGuid(),
                UserId = ownerId,
                Role = WorkspaceRole.Owner,
                JoinedAt = now
            });

         _dbContext.Workspaces.Add(workspace);

            await _dbContext.SaveChangesAsync(cancellationToken);

           

          

            return workspace;
        }

        public async Task<Workspace?> GetWorkspaceAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Workspaces
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == workspaceId && w.Members.Any(m => m.UserId == userId), cancellationToken);
            

        }

        public async Task<Workspace?> UpdateWorkspaceAsync(Guid workspaceId, Guid ownerId, string name, CancellationToken cancellationToken)
        {
            var workspace = await _dbContext.Workspaces
                .FirstOrDefaultAsync(w => w.Id == workspaceId && w.OwnerId == ownerId, cancellationToken);

            if (workspace == null)
            {
                return null;
            }

            workspace.Name = name;
            workspace.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return workspace;
            
        }

      public async Task<bool> DeleteWorkspaceAsync(Guid workspaceId, Guid ownerId, CancellationToken cancellationToken)
{
    int deletedRows = await _dbContext.Workspaces
        .Where(w => w.Id == workspaceId && w.OwnerId == ownerId)
        .ExecuteDeleteAsync(cancellationToken);

    return deletedRows > 0;
}


        public async Task<IReadOnlyList<Workspace>> GetMyWorkspacesAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _dbContext.WorkspaceMembers
                .AsNoTracking()
                .Where(member => member.UserId == userId)
                .Select(member => member.Workspace)
                .ToListAsync(cancellationToken);
             
            
            
        }

    }
}