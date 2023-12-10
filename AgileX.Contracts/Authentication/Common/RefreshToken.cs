namespace AgileX.Contracts.Authentication.Common;

public record RefreshToken(Guid token, DateTime expiresIn);
