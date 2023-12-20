using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Commands.ChangeTicketSprint;

public abstract record ChangeTicketSprintCommand(Guid TicketId, Guid NewSprintId, Guid UserId)
    : IRequest<Result<SuccessMessage>>;
