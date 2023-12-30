using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class RefreshRepository : IRefreshRepository
{
    private readonly CustomDbContext _dbContext;

    public RefreshRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public Refresh? GetRefresh(Guid token) =>
        _dbContext.Refreshes.SingleOrDefault(refresh => refresh.Token == token);

    public void Save(Refresh refresh)
    {
        _dbContext.Refreshes.Add(refresh);
        _dbContext.SaveChanges();
    }

    public void Delete(Guid refreshId)
    {
        var existingRefresh = GetRefresh(refreshId);
        if (existingRefresh != null)
            _dbContext.Remove(existingRefresh);
    }

    public Refresh? Get(Guid token) =>
        _dbContext.Refreshes.SingleOrDefault(refresh => refresh.Token == token);
}
