using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid UserId) : IRequest<Result<SuccessMessage>>;
