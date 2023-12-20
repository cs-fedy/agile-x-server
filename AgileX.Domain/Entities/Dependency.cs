namespace AgileX.Domain.Entities;

public record Dependency(
    Guid TicketId,
    Guid DependencyTicketId,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt
);
