using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using CollabWorkspace.Api.DTOs.Workspace;
using CollabWorkspace.Infrastructure.Services;
using CollabWorkspace.Api.Extensions;


namespace CollabWorkspace.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class WorkspaceController : ControllerBase
    {
        private readonly IWorkspaceService _workspaceService;


        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
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

            var workspace = await _workspaceService.CreateWorkspaceAsync(userId.Value, request.Name, cancellationToken);

            var response = new WorkspaceResponse(
                workspace.Id,
                workspace.Name,
                workspace.OwnerId,
                workspace.CreatedAt,
                workspace.UpdatedAt
            );
            return CreatedAtAction(nameof(GetWorkspaceAsync), new { workspaceId = workspace.Id }, response);



            
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
            var workspace = await _workspaceService.UpdateWorkspaceAsync(workspaceId, userId.Value, request.Name, cancellationToken);
            if (workspace == null)
            {
                return NotFound();
            }
            var response = new WorkspaceResponse(
                workspace.Id,
                workspace.Name,
                workspace.OwnerId,
                workspace.CreatedAt,
                workspace.UpdatedAt
            );
            return Ok(response);
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

            if (isDeleted == false)
            {
                return NotFound();
            }

            

            return Ok(isDeleted);
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
                w.CreatedAt,
                w.UpdatedAt
            ))];
            return Ok(response);
        }
    }
}
