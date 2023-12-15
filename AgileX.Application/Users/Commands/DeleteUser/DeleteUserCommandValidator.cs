using FluentValidation;

namespace AgileX.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator() => RuleFor(x => x.UserId).NotEmpty();
}
