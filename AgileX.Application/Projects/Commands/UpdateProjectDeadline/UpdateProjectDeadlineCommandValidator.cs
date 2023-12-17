using FluentValidation;

namespace AgileX.Application.Projects.Commands.UpdateProjectDeadline;

public class UpdateProjectDeadlineCommandValidator : AbstractValidator<UpdateProjectDeadlineCommand>
{
    public UpdateProjectDeadlineCommandValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewDeadline).NotEmpty();
    }
}
