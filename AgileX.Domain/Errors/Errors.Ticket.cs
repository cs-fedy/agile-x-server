using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public static class TicketErrors
{
    public static Error TicketNotFound = Error.NotFound(
        code: "Ticket.NotFound",
        description: "Requested ticket not found"
    );

    public static Error UnauthorizedMember = Error.Unauthorized(
        code: "Ticket.UnauthorizedMember",
        description: "Member not authorized to perform actions on this ticket"
    );
}
