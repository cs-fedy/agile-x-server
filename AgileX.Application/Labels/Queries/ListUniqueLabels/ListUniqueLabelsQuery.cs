using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Labels.Queries.ListUniqueLabels;

public abstract record ListUniqueLabelsQuery(Guid ProjectId, Guid UserId)
    : IRequest<Result<List<Label>>>;
