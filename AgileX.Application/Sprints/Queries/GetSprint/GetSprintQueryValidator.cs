using FluentValidation;

namespace AgileX.Application.Sprints.Queries.GetSprint;

public class GetSprintQueryValidator : AbstractValidator<GetSprintQuery>
{
    public GetSprintQueryValidator()
    {
        RuleFor(x => x.SprintId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
