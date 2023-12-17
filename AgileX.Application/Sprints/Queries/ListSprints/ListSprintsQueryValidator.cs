using FluentValidation;

namespace AgileX.Application.Sprints.Queries.ListSprints;

public class ListSprintsQueryValidator : AbstractValidator<ListSprintsQuery>
{
    public ListSprintsQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
