using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Queries.GetTicket;

public abstract record GetTicketQuery(Guid TicketId, Guid UserId) : IRequest<Result<Ticket>>;
