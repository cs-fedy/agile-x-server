using FluentValidation;

namespace AgileX.Application.Sprints.Commands.ChangeSprintDates;

public class ChangeSprintDatesCommandValidator : AbstractValidator<ChangeSprintDatesCommand>
{
    public ChangeSprintDatesCommandValidator()
    {
        RuleFor(x => x.SprintId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
