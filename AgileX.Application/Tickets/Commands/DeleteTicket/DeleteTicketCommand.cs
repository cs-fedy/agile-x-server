using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Commands.DeleteTicket;

public abstract record DeleteTicketCommand(Guid TicketId, Guid UserId)
    : IRequest<Result<SuccessMessage>>;
