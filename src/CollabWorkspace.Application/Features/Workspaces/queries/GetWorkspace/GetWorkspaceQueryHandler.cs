using MediatR;
using CollabWorkspace.Application.DTOs.Workspace;
using CollabWorkspace.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using CollabWorkspace.Application.DTOs.WorkspaceMember;
using CollabWorkspace.Application.features.Workspaces.Queries.GetWorkspace;
using CollabWorkspace.Domain.Entities;

namespace CollabWorkspace.Application.Features.Workspaces.Queries.GetWorkspace
{
    public class GetWorkspaceQueryHandler : IRequestHandler<GetWorkspaceQuery, WorkspaceWithRosterDto>
    {
        private readonly ICollabWorkspaceDbContext _context;
         private readonly IUserService _userService;

        public GetWorkspaceQueryHandler(ICollabWorkspaceDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<WorkspaceWithRosterDto> Handle(GetWorkspaceQuery request, CancellationToken cancellationToken)
        {
            var workspace = await _context.Workspaces
                        .AsNoTracking()
                        .Include(w=>w.Members)
                        .FirstOrDefaultAsync(w =>w.Id== request.WorkspaceId && w.Members.Any(m=>m.UserId== request.UserId) && w.DeletedAt == null, cancellationToken)
                        ?? throw new KeyNotFoundException("Workspace not found.");
                        

            var memberIds= workspace.Members.Select(m => m.UserId);

            var memberNames = await _userService.GetUserNamesAsync(memberIds, cancellationToken);

            var workspaceMembers = workspace.Members.Select(m => new MemberNicknameDto(
                m.UserId,
                memberNames.GetValueOrDefault(m.UserId, "Unknown") ?? "Unknown"


            )).ToList();
            return new WorkspaceWithRosterDto(
                workspace.Id,
                workspace.Name,
                workspace.OwnerId,
                workspaceMembers,
                workspace.CreatedAt,
                workspace.UpdatedAt
            );
        }
    }
}