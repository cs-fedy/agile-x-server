using AgileX.Application.Labels.Queries.GetLabel;
using FluentValidation;

namespace AgileX.Application.Labels.Commands.DeleteLabel;

public class DeleteLabelCommandValidator : AbstractValidator<DeleteLabelCommand>
{
    public DeleteLabelCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.LabelId).NotEmpty();
    }
}
