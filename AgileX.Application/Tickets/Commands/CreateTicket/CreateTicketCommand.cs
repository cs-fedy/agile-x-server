using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Tickets.Commands.CreateTicket;

public abstract record CreateTicketCommand(
    Guid ProjectId,
    Guid UserId,
    Guid? AssignedUserId,
    Guid? SprintId,
    Guid? ParentTicketId,
    string Name,
    string Description,
    DateTime Deadline,
    int Priority,
    int Reminder
) : IRequest<Result<SuccessMessage>>;
