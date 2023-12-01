using FluentValidation;

namespace AgileX.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(X => X.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Username).NotEmpty().MinimumLength(8);
        RuleFor(x => x.FullName).NotEmpty().MinimumLength(12);
    }
}
