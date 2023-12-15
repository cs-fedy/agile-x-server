using AgileX.Domain.ObjectValues;

namespace AgileX.Domain.Entities;

public record MemberPermission(
    Guid UserId,
    Guid ProjectId,
    string Name,
    string Description,
    Entity Entity,
    Permission Permission,
    bool IsDeleted,
    DateTime? DeletedAt,
    DateTime CreatedAt
);
