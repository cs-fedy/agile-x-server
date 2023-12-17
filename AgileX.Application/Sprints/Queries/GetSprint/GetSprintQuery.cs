using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Queries.GetSprint;

public record GetSprintQuery(Guid SprintId, Guid UserId) : IRequest<Result<Sprint>>;
