using FluentValidation;

namespace AgileX.Application.Permissions.Commands.GrantPermission;

public class GrantPermissionCommandValidator : AbstractValidator<GrantPermissionCommand>
{
    public GrantPermissionCommandValidator()
    {
        RuleFor(x => x.LoggedUserId).NotEmpty();
        RuleFor(x => x.TargetUserId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Entity).NotEmpty().IsInEnum();
        RuleFor(x => x.Permission).NotEmpty().IsInEnum();
    }
}
