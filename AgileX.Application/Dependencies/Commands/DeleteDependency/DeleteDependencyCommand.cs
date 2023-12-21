using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Dependencies.Commands.DeleteDependency;

public abstract record DeleteDependencyCommand(Guid TicketId, Guid DependencyTicketId, Guid UserId)
    : IRequest<Result<SuccessMessage>>;
