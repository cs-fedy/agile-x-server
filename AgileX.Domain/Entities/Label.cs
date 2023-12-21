namespace AgileX.Domain.Entities;

public record Label(
    Guid LabelId,
    Guid TicketId,
    string Content,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt
);
