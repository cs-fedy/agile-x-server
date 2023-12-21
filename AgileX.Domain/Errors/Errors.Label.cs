using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public static class LabelErrors
{
    public static Error LabelNotFound = Error.NotFound(
        code: "Label.NotFound",
        description: "Requested label not found"
    );
}
