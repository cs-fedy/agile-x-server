using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Commands.ChangeTicketStatus;

public record ChangeTicketStatusCommand(
    Guid TicketId,
    Guid UserId,
    CompletionStatus CompletionStatus
) : IRequest<Result<SuccessMessage>>;
