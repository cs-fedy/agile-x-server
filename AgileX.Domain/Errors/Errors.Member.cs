using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public static class MemberErrors
{
    public static Error UnauthorizedMember = Error.Unauthorized(
        code: "Member.Unauthorized",
        description: "Unauthorized member"
    );

    public static Error AlreadyExist = Error.Conflict(
        code: "Member.AlreadyExist",
        description: "User already a member"
    );
}
