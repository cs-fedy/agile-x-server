using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public class MemberErrors
{
    public static Error UnauthorizedMember = Error.Unauthorized(
        code: "Member.Unauthorized",
        description: "Unauthorized member"
    );
}
