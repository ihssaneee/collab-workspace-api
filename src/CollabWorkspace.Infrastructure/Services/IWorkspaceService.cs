

using CollabWorkspace.Domain.Entities;

namespace CollabWorkspace.Infrastructure.Services
{
    public interface IWorkspaceService
    {
        Task<Workspace> CreateWorkspaceAsync( Guid ownerId, string name, CancellationToken cancellationToken);

        Task<IReadOnlyList<Workspace>> GetMyWorkspacesAsync(Guid userId, CancellationToken cancellationToken);
        Task<Workspace?> GetWorkspaceAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken);
        Task<Workspace?> UpdateWorkspaceAsync(Guid workspaceId, Guid ownerId, string name, CancellationToken cancellationToken);
        Task<bool> DeleteWorkspaceAsync(Guid workspaceId, Guid ownerId, CancellationToken cancellationToken);
    }
}