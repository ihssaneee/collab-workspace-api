using CollabWorkspace.Application.DTOs.Workspace;
using MediatR;

namespace CollabWorkspace.Application.features.Workspaces.Queries.GetWorkspace
{
    public record GetWorkspaceQuery(
        Guid WorkspaceId,
        Guid UserId
    ) : IRequest<WorkspaceWithRosterDto>;
}