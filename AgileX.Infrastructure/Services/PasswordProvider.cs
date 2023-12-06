using AgileX.Application.Common.Interfaces.Services;

namespace AgileX.Infrastructure.Services;

public class PasswordProvider : IPasswordProvider
{
    public bool compare(string hashedPassword, string plainPassword) =>
        BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);

    public string hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
}
