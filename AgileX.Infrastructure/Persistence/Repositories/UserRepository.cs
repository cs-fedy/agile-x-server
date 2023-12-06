using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly CustomDbContext _dbContext;

    public UserRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public User? GetUserByEmail(string email) =>
        _dbContext.Users.Where(user => user.Email == email && !user.IsDeleted).SingleOrDefault();

    public User? GetUserById(Guid userId) =>
        _dbContext.Users.Where(user => user.UserId == userId && !user.IsDeleted).SingleOrDefault();

    public void SaveUser(User user)
    {
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }
}
