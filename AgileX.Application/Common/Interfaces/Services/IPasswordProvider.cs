namespace AgileX.Application.Common.Interfaces.Services;

public interface IPasswordProvider
{
    string hash(string password);
    bool compare(string hashedPassword, string plainPassword);
}
