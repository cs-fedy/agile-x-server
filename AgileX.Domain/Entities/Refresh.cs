namespace AgileX.Domain.Entities;

public record Refresh(Guid Token, Guid OwnerId, DateTime ExpiresIn);
