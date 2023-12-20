using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Dependencies.Commands.AddDependency;

public abstract record AddDependencyCommand(Guid TicketId, Guid DependencyTicketId, Guid UserId)
    : IRequest<Result<SuccessMessage>>;
