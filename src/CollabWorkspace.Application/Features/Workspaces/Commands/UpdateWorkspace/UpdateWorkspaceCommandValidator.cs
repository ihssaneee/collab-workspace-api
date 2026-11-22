using FluentValidation;
namespace CollabWorkspace.Application.features.Workspaces.Commands.UpdateWorkspace
{
    public class UpdateWorkspaceCommandValidator : AbstractValidator<UpdateworkspaceCommand>
    {
        public UpdateWorkspaceCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Workspace name cannot be empty.")
                .Length(3, 100).WithMessage("Workspace name must be between 3 and 100 characters.");
        }
    }
    
}