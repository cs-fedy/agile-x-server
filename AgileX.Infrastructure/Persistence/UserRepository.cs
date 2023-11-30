using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private static readonly List<User> _users = new();

    public User? GetUserByEmail(string email)
    {
        return _users.Find(user => user.Email == email);
    }

    public User? GetUserById(Guid userId)
    {
        return _users.Find(user => user.UserId == userId);
    }

    public void SaveUser(User user)
    {
        var exisitngUserIndex = _users.FindIndex(x => x.UserId == user.UserId);

        if (exisitngUserIndex > -1)
            _users[exisitngUserIndex] = user;
        else _users.Add(user);
    }
}
