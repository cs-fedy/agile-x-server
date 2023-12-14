namespace AgileX.Application.Common.Types;

public record RefreshToken(Guid Token, DateTime ExpiresIn);
