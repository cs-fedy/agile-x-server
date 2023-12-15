using FluentValidation;

namespace AgileX.Application.Authentication.Commands.Refresh;

public class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator() => RuleFor(x => x.Token).NotEmpty().NotEmpty();
}
