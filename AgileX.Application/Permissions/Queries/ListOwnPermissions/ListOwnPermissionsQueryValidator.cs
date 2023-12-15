using AgileX.Application.Permissions.Queries.ListMemberPermissions;
using FluentValidation;

namespace AgileX.Application.Permissions.Queries.ListOwnPermissions;

public class ListOwnPermissionsQueryValidator : AbstractValidator<ListMemberPermissionsQuery>
{
    public ListOwnPermissionsQueryValidator()
    {
        RuleFor(x => x.LoggedUserId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}
