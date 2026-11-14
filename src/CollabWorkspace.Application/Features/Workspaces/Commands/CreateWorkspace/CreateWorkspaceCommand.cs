using CollabWorkspace.Application.DTOs.Workspace;
using MediatR;
namespace CollabWorkspace.Application.features.Workspaces.Commands.CreateWorkspace
{
    public record CreateWorkspaceCommand(
        Guid OwnerId,
         string Name
         ) : IRequest<WorkspaceWithRosterDto>;
}