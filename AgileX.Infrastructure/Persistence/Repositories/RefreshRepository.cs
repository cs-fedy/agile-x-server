using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class RefreshRepository : IRefreshRepository
{
    private readonly CustomDbContext _dbContext;

    public RefreshRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void SaveRefresh(Refresh refresh)
    {
        _dbContext.Refreshes.Add(refresh);
        _dbContext.SaveChanges();
    }

    public void DeleteRefresh(Guid refreshId)
    {
        var existingRefresh = GetRefresh(refreshId);
        if (existingRefresh != null)
            _dbContext.Remove(existingRefresh);
    }

    public Refresh? GetRefresh(Guid token) =>
        _dbContext.Refreshes.SingleOrDefault(refresh => refresh.Token == token);
}
