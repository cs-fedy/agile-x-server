namespace AgileX.Contracts.Authentication.Register;

public record RegisterRequest(
    string Email,
    string Password,
    string Username,
    string FullName
);
