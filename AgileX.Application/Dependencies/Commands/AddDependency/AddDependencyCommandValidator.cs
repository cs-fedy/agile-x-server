using FluentValidation;

namespace AgileX.Application.Dependencies.Commands.AddDependency;

public class AddDependencyCommandValidator : AbstractValidator<AddDependencyCommand>
{
    public AddDependencyCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.DependencyTicketId).NotEmpty();
    }
}
