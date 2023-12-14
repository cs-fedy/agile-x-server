using AgileX.Application.Common.Types;

namespace AgileX.Application.Authentication.Common;

public record AuthenticationResult(AccessToken AccessToken, RefreshToken RefreshToken);
