using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface ISprintRepository
{
    void Save(Sprint sprint);
    void Delete(Guid sprintId);
    Sprint? GetById(Guid sprintId);
    List<Sprint> ListByProjectId(Guid projectId);
}
