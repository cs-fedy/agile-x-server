using AgileX.Domain.Common.Result;

namespace AgileX.Domain.Common.Errors;

public static class Errors
{
    public static class User
    {
        public static Error UserAlreadyExist = Error.Conflict(
            code: "User.AlreadyExist",
            description: "User already exist - duplicate email"
        );

        public static Error UserNotFound = Error.NotFound(
            code: "User.NotFound",
            description: "Requested user not found"
        );
    }
}
