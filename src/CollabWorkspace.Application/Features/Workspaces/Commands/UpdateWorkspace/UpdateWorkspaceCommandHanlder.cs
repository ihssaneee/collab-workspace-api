using CollabWorkspace.Application.DTOs.Workspace;
using CollabWorkspace.Application.DTOs.WorkspaceMember;
using CollabWorkspace.Application.Interfaces;
using CollabWorkspace.Domain.Entities;
using CollabWorkspace.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace CollabWorkspace.Application.features.Workspaces.Commands.UpdateWorkspace
{
    public class UpdateWorkspaceCommandHandler : IRequestHandler<UpdateworkspaceCommand, WorkspaceWithRosterDto>
    {
        private readonly ICollabWorkspaceDbContext _dbContext;
        private readonly IUserService _userService;

        public UpdateWorkspaceCommandHandler(ICollabWorkspaceDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;

        }

        public async Task<WorkspaceWithRosterDto> Handle(
     UpdateworkspaceCommand request,
     CancellationToken cancellationToken)
        {
            var workspace = await _dbContext.Workspaces
                .Include(w => w.Members)
                .FirstOrDefaultAsync(
                    w => w.Id == request.WorkspaceId && w.OwnerId == request.OwnerId,
                    cancellationToken)
                ?? throw new KeyNotFoundException("Workspace not found.");

            workspace.Name = request.Name;
            workspace.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);

            var memberIds = workspace.Members.Select(m => m.UserId);

            var userNames = await _userService.GetUserNamesAsync(
                memberIds,
                cancellationToken);

            var memberDtos = workspace.Members
                .Select(m => new MemberNicknameDto(
                    m.UserId,
                    userNames.GetValueOrDefault(m.UserId, "Unknown") ?? "Unknown"))
                .ToList();

            return new WorkspaceWithRosterDto(
                workspace.Id,
                workspace.Name,
                workspace.OwnerId,
                memberDtos,
                workspace.CreatedAt,
                workspace.UpdatedAt);
        }
    }
}