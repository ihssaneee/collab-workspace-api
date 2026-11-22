using CollabWorkspace.Application.DTOs.Workspace;
using MediatR;

namespace CollabWorkspace.Application.features.Workspaces.Commands.UpdateWorkspace
{
    public record UpdateworkspaceCommand(
        Guid WorkspaceId,
        Guid OwnerId,
        string Name
        
    ) : IRequest<WorkspaceWithRosterDto>;
}