namespace AgileX.Domain.Entities;

public record Comment(
    Guid CommentId,
    Guid TicketId,
    Guid? ParentCommentId,
    Guid CommentedBy,
    string Text,
    string? AttachedCode,
    int SubCommentsCount,
    bool IsDelete,
    DateTime? DeletedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
