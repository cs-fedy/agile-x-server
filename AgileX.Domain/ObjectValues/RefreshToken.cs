namespace AgileX.Domain.ObjectValues;

public record RefreshToken(Guid token, DateTime expiresIn);
