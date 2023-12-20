using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Commands.ChangeTicketDeadline;

public record ChangeTicketDeadlineCommand(Guid TicketId, Guid UserId, DateTime NewDeadline)
    : IRequest<Result<SuccessMessage>>;
