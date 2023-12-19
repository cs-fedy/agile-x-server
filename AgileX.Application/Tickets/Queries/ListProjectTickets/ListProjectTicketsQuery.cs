using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Queries.ListProjectTickets;

public record ListProjectTicketsQuery(Guid ProjectId, Guid UserId) : IRequest<Result<List<Ticket>>>;
