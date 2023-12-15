using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    public void Save(Project project)
    {
        throw new NotImplementedException();
    }

    public Project? GetById(Guid projectId)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid projectId)
    {
        throw new NotImplementedException();
    }
}
