using System.Numerics;

namespace AgileX.Contracts.Authentication.Register;

public record RegiseterResponse(
    Guid UserId,
    string Username,
    string FullName,
    string Email, 
    string Role,
    bool IsConfirmed,
    bool IsDeleted,
    BigInteger DeletedAt,
    BigInteger CreatedAt,
    BigInteger UpdatedAt
);
