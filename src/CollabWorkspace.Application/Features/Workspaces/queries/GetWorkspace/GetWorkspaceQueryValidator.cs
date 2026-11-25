using CollabWorkspace.Application.features.Workspaces.Queries.GetWorkspace;
using CollabWorkspace.Application.Features.Workspaces.Queries.GetMyWorkspaces;
using FluentValidation;

namespace CollabWorkspace.Application.Features.Workspaces.Queries.GetWorkspace
{
    public class GetWorkspaceQueryValidator : AbstractValidator<GetWorkspaceQuery>
    {
        public GetWorkspaceQueryValidator()
        {
            RuleFor(x => x.WorkspaceId)
                .NotEmpty().WithMessage("Workspace ID cannot be empty.");
        }
    }
}