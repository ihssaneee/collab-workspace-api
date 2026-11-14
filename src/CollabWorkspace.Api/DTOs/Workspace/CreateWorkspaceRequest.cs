using System.ComponentModel.DataAnnotations;
namespace CollabWorkspace.Api.DTOs.Workspace
{
    public sealed record CreateWorkspaceRequest(
        [param: Required(AllowEmptyStrings = false, ErrorMessage = "Workspace name cannot be empty.")]
        [param: StringLength(100, MinimumLength = 3)] string Name
    );
    
    
}