using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Permissions.Commands.GrantPermission;

public abstract record GrantPermissionCommand(
    Guid LoggedUserId,
    Guid TargetUserId,
    Guid ProjectId,
    string Name,
    string Description,
    Entity Entity,
    Permission Permission
) : IRequest<Result<SuccessMessage>>;
