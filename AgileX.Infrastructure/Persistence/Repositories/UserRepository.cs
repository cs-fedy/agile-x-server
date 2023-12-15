using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CustomDbContext _dbContext;

    public UserRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public User? GetByEmail(string email) =>
        _dbContext.Users.SingleOrDefault(user => user.Email == email);

    public void Delete(Guid userId)
    {
        var existingUser = GetById(userId);
        if (existingUser == null)
            return;

        existingUser = existingUser with
        {
            IsDeleted = true,
            DeletedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _dbContext.SaveChanges();
    }

    public User? GetById(Guid userId) =>
        _dbContext.Users.SingleOrDefault(user => user.UserId == userId);

    public User? GetByUsername(string username) =>
        _dbContext.Users.SingleOrDefault(user => user.Username == username);

    public void Save(User user)
    {
        var existingUser = GetById(user.UserId);
        if (existingUser == null)
            _dbContext.Users.Add(user);
        else
            existingUser = user with { };
        _dbContext.SaveChanges();
    }
}
