using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class DependencyRepository : IDependencyRepository
{
    public void Save(Dependency dependency)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid ticketId, Guid dependencyTicketId)
    {
        throw new NotImplementedException();
    }

    public Dependency? Get(Guid ticketId, Guid dependencyTicketId)
    {
        throw new NotImplementedException();
    }

    public List<Dependency> ListByTicketId(Guid ticketId)
    {
        throw new NotImplementedException();
    }

    public List<Dependency> ListByDependencyTicketId(Guid dependencyTicketId)
    {
        throw new NotImplementedException();
    }
}
