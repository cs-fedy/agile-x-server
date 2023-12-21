using FluentValidation;

namespace AgileX.Application.Dependencies.Commands.DeleteDependency;

public class DeleteDependencyCommandValidator : AbstractValidator<DeleteDependencyCommand>
{
    public DeleteDependencyCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.DependencyTicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
