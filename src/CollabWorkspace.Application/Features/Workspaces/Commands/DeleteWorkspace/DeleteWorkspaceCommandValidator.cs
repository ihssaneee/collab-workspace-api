using FluentValidation;
namespace CollabWorkspace.Application.features.Workspaces.Commands.DeleteWorkspace
{
    public class DeleteWorkspaceCommandValidator : AbstractValidator<DeleteWorkspaceCommand>
    {
        public DeleteWorkspaceCommandValidator()
        {
            RuleFor(x => x.WorkspaceId)
                .NotEmpty().WithMessage("Workspace ID cannot be empty.");

            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("Owner ID cannot be empty.");
        }
    }
}