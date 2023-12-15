using FluentValidation;

namespace AgileX.Application.Users.Commands.UpdateFullName;

public class UpdateFullNameCommandValidator : AbstractValidator<UpdateFullNameCommand>
{
    public UpdateFullNameCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty();
    }
}
