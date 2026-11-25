using MediatR;
using CollabWorkspace.Application.DTOs.Workspace;
using CollabWorkspace.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using CollabWorkspace.Application.DTOs.WorkspaceMember;

namespace CollabWorkspace.Application.Features.Workspaces.Queries.GetMyWorkspaces
{
    public class GetMyWorkspacesQueryHandler : IRequestHandler<GetMyWorkspacesQuery, List<WorkspaceWithRosterDto>>
    {
        private readonly ICollabWorkspaceDbContext _dbContext;
        private readonly IUserService _userService;

        public GetMyWorkspacesQueryHandler(ICollabWorkspaceDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        public async Task<List<WorkspaceWithRosterDto>> Handle(GetMyWorkspacesQuery request, CancellationToken cancellationToken)
        {
            var workspaces = await _dbContext.Workspaces
                .AsNoTracking()
                .Include(w => w.Members)
                .Where(w => w.Members.Any(m => m.UserId == request.UserId) && w.DeletedAt == null)
                .ToListAsync(cancellationToken);

            var memberIds = workspaces.SelectMany(w => w.Members.Select(m => m.UserId)).Distinct();

            var userNames = await _userService.GetUserNamesAsync(memberIds, cancellationToken);

            var workspaceDtos = workspaces.Select(w => {
                var workspaceMembers = w.Members.Select(m=> new MemberNicknameDto(
                    m.UserId,
                    userNames.GetValueOrDefault(m.UserId,"Unknown") ?? "Unknown"
                )).ToList();

                return new WorkspaceWithRosterDto(
                    w.Id,
                    w.Name,
                    w.OwnerId,
                    workspaceMembers,
                    w.CreatedAt,
                    w.UpdatedAt

                );
         } ).ToList();

         return workspaceDtos;
         
        }
        
    }
}