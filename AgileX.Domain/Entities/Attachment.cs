namespace AgileX.Domain.Entities;

public record Attachment(
    Guid AttachmentId,
    Guid TicketId,
    string Url,
    string Type,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt
);
