using FluentValidation;
namespace CollabWorkspace.Application.Features.Workspaces.Queries.GetMyWorkspaces
{
    public class GetMyWorkspacesQueryValidator : AbstractValidator<GetMyWorkspacesQuery>
    {
        public GetMyWorkspacesQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId cannot be empty.");
        }
    }
}