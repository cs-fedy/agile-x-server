using AgileX.Application.Dependencies.Queries.ListDependencies;
using FluentValidation;

namespace AgileX.Application.Labels.Queries.ListLabels;

public class ListLabelsQueryValidator : AbstractValidator<ListLabelsQuery>
{
    public ListLabelsQueryValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
