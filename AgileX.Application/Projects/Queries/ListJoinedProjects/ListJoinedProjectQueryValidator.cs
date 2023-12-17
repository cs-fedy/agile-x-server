using FluentValidation;

namespace AgileX.Application.Projects.Queries.ListJoinedProjects;

public class ListJoinedProjectQueryValidator : AbstractValidator<ListJoinedProjectsQuery>
{
    public ListJoinedProjectQueryValidator() => RuleFor(x => x.UserId).NotEmpty();
}
