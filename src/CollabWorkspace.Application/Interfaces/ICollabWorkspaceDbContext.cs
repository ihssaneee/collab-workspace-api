using CollabWorkspace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace CollabWorkspace.Application.Interfaces
{
    public interface ICollabWorkspaceDbContext
    {
        DbSet<Workspace> Workspaces { get; }
        DbSet<WorkspaceMember> WorkspaceMembers { get; }
        DbSet<Document> Documents { get; }
        DbSet<DocumentBlock> DocumentBlocks { get; }

    
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}