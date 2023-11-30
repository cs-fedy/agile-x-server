namespace AgileX.Domain.Entities;

public record User(
    Guid UserId,
    string Email,
    string Passsword,
    string FullName,
    string Username,
    string Role,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsConfirmed = false,
    bool IsDeleted = false,
    DateTime? DletedAt = null
);
