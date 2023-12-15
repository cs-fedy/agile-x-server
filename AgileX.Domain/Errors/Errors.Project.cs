using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public class ProjectErrors
{
    public static Error ProjectNotFound = Error.NotFound(
        code: "Project.NotFound",
        description: "Requested project not found"
    );
}
