using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Labels.Queries.ListLabels;

public abstract record ListLabelsQuery(Guid TicketId, Guid UserId) : IRequest<Result<List<Label>>>;
