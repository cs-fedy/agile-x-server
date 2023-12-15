using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Users.Commands.UpdateUsername;

public abstract record UpdateUsernameCommand(Guid UserId, string NewUsername)
    : IRequest<Result<SuccessMessage>>;
