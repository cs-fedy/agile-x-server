using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    void Save(User user);
    User? GetById(Guid userId);
    User? GetByEmail(string email);
    User? GetByUsername(string username);
    void Delete(Guid userId);
}
