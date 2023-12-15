using FluentValidation;

namespace AgileX.Application.Users.Commands.UpdateUsername;

public class UpdateUsernameCommandValidator : AbstractValidator<UpdateUsernameCommand>
{
    public UpdateUsernameCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewUsername).NotEmpty();
    }
}
