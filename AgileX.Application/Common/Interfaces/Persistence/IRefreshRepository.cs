using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IRefreshRepository
{
    void SaveRefresh(Refresh refresh);
    void DeleteRefresh(Guid refreshId);
    Refresh? GetRefresh(Guid token);
}
