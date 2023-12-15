using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public class PermissionErrors
{
    public static Error UnauthorizedAction = Error.Unauthorized(
        code: "Permission.UnauthorizedAction",
        description: "Unauthorized action"
    );

    public static Error PermissionAlreadyGranted = Error.Conflict(
        code: "Permission.AlreadyGranted",
        description: "Permission already granted"
    );
}
