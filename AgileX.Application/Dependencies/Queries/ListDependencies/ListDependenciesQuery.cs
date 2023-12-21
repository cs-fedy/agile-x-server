using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Dependencies.Queries.ListDependencies;

public record ListDependenciesQuery(Guid TicketId, Guid UserId)
    : IRequest<Result<List<Dependency>>>;
