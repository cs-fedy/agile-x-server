using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class DependencyRepository : IDependencyRepository
{
    private readonly CustomDbContext _dbContext;

    public DependencyRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void Save(Dependency dependency)
    {
        var existingDependency = Get(dependency.TicketId, dependency.DependencyTicketId);
        if (existingDependency == null)
            _dbContext.Dependencies.Add(dependency);
        else
            existingDependency = dependency with { };
        _dbContext.SaveChanges();
    }

    public void Delete(Guid ticketId, Guid dependencyTicketId)
    {
        var existingDependency = Get(ticketId, dependencyTicketId);
        if (existingDependency == null)
            return;

        existingDependency = existingDependency with
        {
            IsDeleted = true,
            DeletedAt = DateTime.Now,
        };

        _dbContext.SaveChanges();
    }

    public Dependency? Get(Guid ticketId, Guid dependencyTicketId) =>
        _dbContext
            .Dependencies
            .SingleOrDefault(
                dependency =>
                    dependency.TicketId == ticketId
                    && dependency.DependencyTicketId == dependencyTicketId
            );

    public List<Dependency> ListByTicketId(Guid ticketId) =>
        _dbContext.Dependencies.Where(x => x.TicketId == ticketId).ToList();

    public List<Dependency> ListByDependencyTicketId(Guid dependencyTicketId) =>
        _dbContext.Dependencies.Where(x => x.DependencyTicketId == dependencyTicketId).ToList();
}
