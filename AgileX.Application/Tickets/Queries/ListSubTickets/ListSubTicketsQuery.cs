using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Queries.ListSubTickets;

public abstract record ListSubTicketsQuery(Guid TicketId, Guid UserId)
    : IRequest<Result<List<Ticket>>>;
