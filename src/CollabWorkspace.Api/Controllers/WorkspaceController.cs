using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CollabWorkspace.Api.DTOs.Workspace;
using CollabWorkspace.Infrastructure.Services;
using CollabWorkspace.Api.Extensions;
using CollabWorkspace.Api.DTOs.WorkspaceMember;
using MediatR;
using CollabWorkspace.Application.features.Workspaces.Commands.CreateWorkspace;
using CollabWorkspace.Application.features.Workspaces.Commands.UpdateWorkspace;
using CollabWorkspace.Application.features.Workspaces.Commands.DeleteWorkspace;
using CollabWorkspace.Application.Features.Workspaces.Queries.GetMyWorkspaces;
using CollabWorkspace.Application.features.Workspaces.Queries.GetWorkspace;


namespace CollabWorkspace.Api.Controllers
{
    [ApiController]
    [Route("api/workspaces")]

    public class WorkspaceController : ControllerBase
    {
        private readonly ISender _sender;


        public WorkspaceController( ISender sender)
        {
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

            var response = await _sender.Send(new GetWorkspaceQuery(workspaceId, userId.Value), cancellationToken);
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

            await _sender.Send(new DeleteWorkspaceCommand(workspaceId, userId.Value), cancellationToken);

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

            var response = await _sender.Send(new GetMyWorkspacesQuery(userId.Value), cancellationToken);

            return Ok(response);
        }
    }
}
