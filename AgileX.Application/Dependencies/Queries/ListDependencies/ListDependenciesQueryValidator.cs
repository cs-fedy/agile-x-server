using FluentValidation;

namespace AgileX.Application.Dependencies.Queries.ListDependencies;

public class ListDependenciesQueryValidator : AbstractValidator<ListDependenciesQuery>
{
    public ListDependenciesQueryValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
