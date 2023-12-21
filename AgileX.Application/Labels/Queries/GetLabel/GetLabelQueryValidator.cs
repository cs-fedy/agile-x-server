using FluentValidation;
using MediatR;

namespace AgileX.Application.Labels.Queries.GetLabel;

public class GetLabelQueryValidator : AbstractValidator<GetLabelQuery>
{
    public GetLabelQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.LabelId).NotEmpty();
    }
}
