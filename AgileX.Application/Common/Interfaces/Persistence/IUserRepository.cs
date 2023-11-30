using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    void SaveUser(User user);
    User? GetUserById(Guid userId);
    User? GetUserByEmail(string email);
}
