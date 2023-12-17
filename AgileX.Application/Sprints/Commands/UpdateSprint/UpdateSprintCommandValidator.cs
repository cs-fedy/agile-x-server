using FluentValidation;

namespace AgileX.Application.Sprints.Commands.UpdateSprint;

public class UpdateSprintCommandValidator : AbstractValidator<UpdateSprintCommand>
{
    public UpdateSprintCommandValidator()
    {
        RuleFor(x => x.SprintId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
