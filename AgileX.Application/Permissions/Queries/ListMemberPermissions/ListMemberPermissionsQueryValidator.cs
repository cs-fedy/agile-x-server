using FluentValidation;

namespace AgileX.Application.Permissions.Queries.ListMemberPermissions;

public class ListMemberPermissionsQueryValidator : AbstractValidator<ListMemberPermissionsQuery>
{
    public ListMemberPermissionsQueryValidator()
    {
        RuleFor(x => x.LoggedUserId).NotEmpty();
        RuleFor(x => x.TargetUserId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
