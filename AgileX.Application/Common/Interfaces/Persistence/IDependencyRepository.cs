using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IDependencyRepository
{
    void Save(Dependency dependency);
    void Delete(Guid ticketId, Guid dependencyTicketId);
    Dependency? Get(Guid ticketId, Guid dependencyTicketId);
    List<Dependency> ListByTicketId(Guid ticketId);
    List<Dependency> ListByDependencyTicketId(Guid dependencyTicketId);
}
