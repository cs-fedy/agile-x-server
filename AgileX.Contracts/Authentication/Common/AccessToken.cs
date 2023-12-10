namespace AgileX.Contracts.Authentication.Common;

public record AccessToken(string token, DateTime expiresIn);
