using AgileX.Domain.ObjectValues;

namespace AgileX.Domain.Entities;

public record Project(
    Guid ProjectId,
    string Name,
    string Description,
    CompletionStatus CompletionStatus,
    decimal Progress,
    int Priority,
    DateTime Deadline,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
