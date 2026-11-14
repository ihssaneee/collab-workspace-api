using CollabWorkspace.Application.DTOs.Workspace;
using CollabWorkspace.Application.Interfaces;
using CollabWorkspace.Domain.Entities;
using CollabWorkspace.Domain.Enums;
using MediatR;
namespace CollabWorkspace.Application.features.Workspaces.Commands.CreateWorkspace
{
    public class CreateWorkspaceCommandHandler : IRequestHandler<CreateWorkspaceCommand, WorkspaceWithRosterDto>
    {
        private readonly ICollabWorkspaceDbContext _dbContext;
        private readonly IUserService _userService;
       
        public CreateWorkspaceCommandHandler(ICollabWorkspaceDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
            
        }

        public async Task<WorkspaceWithRosterDto> Handle(CreateWorkspaceCommand request, CancellationToken cancellationToken)
        {
            var userName = await _userService.GetUserNameAsync(request.OwnerId, cancellationToken);
            if (userName is null)
            {
                throw new InvalidOperationException("User not found");
            }

            var now = DateTime.UtcNow;
            var workspace = new Workspace
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                OwnerId = request.OwnerId,
                CreatedAt = now,
                UpdatedAt = now
            };

            workspace.Members.Add(new WorkspaceMember
            {
                Id = Guid.NewGuid(),
                UserId = request.OwnerId,
                Role = WorkspaceRole.Owner,
                JoinedAt = now
            });
           
            _dbContext.Workspaces.Add(workspace);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new WorkspaceWithRosterDto
            (
                workspace.Id,
                workspace.Name,
                workspace.OwnerId,
                [new(request.OwnerId, userName)],
                workspace.CreatedAt,
                workspace.UpdatedAt



            );
        }
    }
}