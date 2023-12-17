using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public class SprintErrors
{
    public static Error SprintNotFound = Error.NotFound(
        code: "Sprint.NotFound",
        description: "Requested sprint not found"
    );
}
