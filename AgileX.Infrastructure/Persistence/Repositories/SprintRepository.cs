using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class SprintRepository : ISprintRepository
{
    public void Save(Sprint sprint)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid sprintId)
    {
        throw new NotImplementedException();
    }

    public Sprint? GetById(Guid sprintId)
    {
        throw new NotImplementedException();
    }

    public List<Sprint> ListByProjectId(Guid projectId)
    {
        throw new NotImplementedException();
    }
}
