using AgileX.Application.Labels.Queries.ListLabels;
using FluentValidation;

namespace AgileX.Application.Labels.Queries.ListUniqueLabels;

public class ListUniqueLabelsQueryValidator : AbstractValidator<ListUniqueLabelsQuery>
{
    public ListUniqueLabelsQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
