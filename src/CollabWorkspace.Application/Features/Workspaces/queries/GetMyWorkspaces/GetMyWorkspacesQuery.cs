using MediatR;
using CollabWorkspace.Application.DTOs.Workspace;

namespace CollabWorkspace.Application.Features.Workspaces.Queries.GetMyWorkspaces
{
    public record GetMyWorkspacesQuery(Guid UserId) : IRequest<List<WorkspaceWithRosterDto>>;
}