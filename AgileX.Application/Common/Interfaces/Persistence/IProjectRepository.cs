using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IProjectRepository
{
    void Save(Project project);
    Project? GetById(Guid projectId);
    void Delete(Guid projectId);
}
