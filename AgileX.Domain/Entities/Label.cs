namespace AgileX.Domain.Entities;

public record Label(
    Guid LabelId,
    Guid TicketId,
    Guid ProjectId,
    string Content,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt
);
