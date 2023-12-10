using AgileX.Contracts.Authentication.Common;

namespace AgileX.Contracts.Authentication.Login;

public record LoginResponse(AccessToken AccessToken, RefreshToken RefreshToken);
