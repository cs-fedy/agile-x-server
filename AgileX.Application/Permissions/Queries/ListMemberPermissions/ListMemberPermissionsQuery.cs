using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Permissions.Queries.ListMemberPermissions;

public abstract record ListMemberPermissionsQuery(
    Guid ProjectId,
    Guid LoggedUserId,
    Guid TargetUserId
) : IRequest<Result<List<MemberPermission>>>;
