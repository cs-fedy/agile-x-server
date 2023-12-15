using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Users.Commands.UpdateFullName;

public abstract record UpdateFullNameCommand(Guid UserId, string FullName)
    : IRequest<Result<SuccessMessage>>;
