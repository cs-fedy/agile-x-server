using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public class TicketErrors
{
    public static Error TicketNotFound = Error.NotFound(
        code: "Ticket.NotFound",
        description: "Requested ticket not found"
    );
}
