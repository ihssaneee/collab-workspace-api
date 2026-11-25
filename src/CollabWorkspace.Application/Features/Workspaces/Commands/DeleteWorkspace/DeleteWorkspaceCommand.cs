using CollabWorkspace.Application.DTOs.Workspace;
using MediatR;

namespace CollabWorkspace.Application.features.Workspaces.Commands.DeleteWorkspace
{
    public record DeleteWorkspaceCommand(
        Guid WorkspaceId,
        Guid OwnerId
    ) : IRequest<bool>;
}