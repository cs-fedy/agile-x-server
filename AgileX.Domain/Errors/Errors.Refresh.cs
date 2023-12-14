using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public static class RefreshErrors
{
    public static Error InvalidRefresh = Error.NotFound(
        code: "Refresh.Invalid",
        description: "Not found / invalid refresh token"
    );
}
