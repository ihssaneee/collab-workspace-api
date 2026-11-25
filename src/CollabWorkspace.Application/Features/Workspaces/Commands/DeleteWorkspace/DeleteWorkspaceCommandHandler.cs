using CollabWorkspace.Application.DTOs.Workspace;
using CollabWorkspace.Application.DTOs.WorkspaceMember;
using CollabWorkspace.Application.Interfaces;
using CollabWorkspace.Domain.Entities;
using CollabWorkspace.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CollabWorkspace.Application.features.Workspaces.Commands.DeleteWorkspace
{
    public class DeleteWorkspaceCommandHandler : IRequestHandler<DeleteWorkspaceCommand,bool>
    {
        private readonly ICollabWorkspaceDbContext _dbContext;

        public DeleteWorkspaceCommandHandler(ICollabWorkspaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(DeleteWorkspaceCommand request, CancellationToken cancellationToken)
        {
            var workspace = await _dbContext.Workspaces
                .FirstOrDefaultAsync(w => w.Id == request.WorkspaceId && w.OwnerId == request.OwnerId, cancellationToken) ?? throw new KeyNotFoundException("Workspace not found.");

            workspace.DeletedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);   

            return true;
        }
        
    }
}