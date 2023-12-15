using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Permissions.Commands.RevokePermission;

public abstract record RevokePermissionCommand(
    Guid LoggedUserId,
    Guid TargetUserId,
    Guid ProjectId,
    Permission Permission
) : IRequest<Result<SuccessMessage>>;
