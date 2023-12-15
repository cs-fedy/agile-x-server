using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IRefreshRepository
{
    void Save(Refresh refresh);
    void Delete(Guid refreshId);
    Refresh? Get(Guid token);
}
