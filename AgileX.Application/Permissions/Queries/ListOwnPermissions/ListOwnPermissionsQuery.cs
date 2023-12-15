using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Permissions.Queries.ListOwnPermissions;

public record ListOwnPermissionsQuery(Guid ProjectId, Guid UserId)
    : IRequest<Result<List<MemberPermission>>>;
