using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using CollabWorkspace.Api.DTOs.Workspace;
using CollabWorkspace.Infrastructure.Services;
using CollabWorkspace.Api.Extensions;
using CollabWorkspace.Api.DTOs.WorkspaceMember;
using MediatR;
using CollabWorkspace.Application.features.Workspaces.Commands.CreateWorkspace;
using CollabWorkspace.Application.features.Workspaces.Commands.UpdateWorkspace;


namespace CollabWorkspace.Api.Controllers
{
    [ApiController]
    [Route("api/workspaces")]

    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;
        private readonly ISender _sender;


        public WorkspaceController(IWorkspaceService workspaceService, ISender sender)
        {
            _workspaceService = workspaceService;
            _sender = sender;
        }

        [HttpGet("{workspaceId}")]
        [ActionName(nameof(GetWorkspaceAsync))]
        [Authorize]
        public async Task<IActionResult> GetWorkspaceAsync(Guid workspaceId, CancellationToken cancellationToken)
        {
            Guid? userId = User.GetUserId();
           
            if (userId == null)
            {
                return Unauthorized();
            }

            var workspace = await _workspaceService.GetWorkspaceAsync(workspaceId, userId.Value, cancellationToken);

            if (workspace == null)
            {
                return NotFound();
            }

            var response = new WorkspaceResponse(
                workspace.Id,
                workspace.Name,
                workspace.OwnerId,
                workspace.Members.Select(m => new WorkspaceMemberResponse(
                    m.Id,
                    m.WorkspaceId,
                    m.UserId,
                    m.Role.ToString(),
                    m.JoinedAt
                )).ToList(),
                workspace.CreatedAt,
                workspace.UpdatedAt
            );

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWorkspaceAsync([FromBody] CreateWorkspaceRequest request, CancellationToken cancellationToken)
        {
            Guid? userId = User.GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var workspace = await _sender.Send(new CreateWorkspaceCommand(userId.Value,request.Name), cancellationToken);

           
            return CreatedAtAction(nameof(GetWorkspaceAsync), new { workspaceId = workspace.Id }, workspace);



            
        }
    [HttpPut("{workspaceId}")]
    [Authorize]
        public async Task<IActionResult> UpdateWorkspaceAsync( Guid workspaceId,[FromBody] UpdateWorkspaceRequest request, CancellationToken cancellationToken)
        {
            Guid? userId = User.GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }
            var updatedWorkspace = await _sender.Send(new UpdateworkspaceCommand(workspaceId, userId.Value, request.Name), cancellationToken);

            return Ok(updatedWorkspace);

        }

        [HttpDelete("{workspaceId}")]
        [Authorize]
        public async Task<IActionResult> DeleteWorkspaceAsync(Guid workspaceId, CancellationToken cancellationToken)
        {
            Guid? userId = User.GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var isDeleted = await _workspaceService.DeleteWorkspaceAsync(workspaceId, userId.Value, cancellationToken);

            if (!isDeleted)
            {
               return NotFound(new { message = "Workspace not found or you are not the owner." });
            }

            

            return NoContent();
        }

        [HttpGet("my-workspaces")]
        [Authorize]
        public async Task<IActionResult> GetMyWorkspacesAsync(CancellationToken cancellationToken)
        {
            Guid? userId = User.GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var workspaces = await _workspaceService.GetMyWorkspacesAsync(userId.Value, cancellationToken);

         List<WorkspaceResponse> response = [..workspaces.Select(w => new WorkspaceResponse(
                w.Id,
                w.Name,
                w.OwnerId,
                w.Members.Select(m => new WorkspaceMemberResponse(
                    m.Id,
                    m.WorkspaceId,
                    m.UserId,
                    m.Role.ToString(),
                    m.JoinedAt
                )).ToList(),
                w.CreatedAt,
                w.UpdatedAt
                
                
            ))];
            return Ok(response);
        }
    }
}
