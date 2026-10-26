using System.ComponentModel.DataAnnotations;
namespace CollabWorkspace.Api.DTOs.Workspace
{
    public sealed record CreateWorkspaceRequest(
        [property: Required(AllowEmptyStrings = false, ErrorMessage = "Workspace name cannot be empty.")]
        [property: StringLength(100, MinimumLength = 3)] string Name
    );
    
    
}