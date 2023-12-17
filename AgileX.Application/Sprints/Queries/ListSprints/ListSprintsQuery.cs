using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Sprints.Queries.ListSprints;

public abstract record ListSprintsQuery(Guid ProjectId, Guid UserId)
    : IRequest<Result<List<Sprint>>>;
