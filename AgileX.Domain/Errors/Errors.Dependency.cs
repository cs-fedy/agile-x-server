using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public static class DependencyErrors
{
    public static Error DependencyNotFound = Error.NotFound(
        code: "Dependency.NotFound",
        description: "Requested dependency not found"
    );

    public static Error DependencyAlreadyExist = Error.Conflict(
        code: "Dependency.AlreadyExist",
        description: "Requested dependency already exist"
    );

    public static Error TicketInProgressOrCompleted = Error.Validation(
        code: "Dependency.InProgressOrCompleted",
        description: "Ticket is in progress or completed. Can't add new dependency."
    );
}
