using FluentValidation;

namespace AgileX.Application.Sprints.Commands.DeleteSprint;

public class DeleteSprintCommandValidator : AbstractValidator<DeleteSprintCommand>
{
    public DeleteSprintCommandValidator()
    {
        RuleFor(x => x.SprintId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
