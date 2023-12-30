using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class SprintRepository : ISprintRepository
{
    private readonly CustomDbContext _dbContext;

    public SprintRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void Save(Sprint sprint)
    {
        var existingSprint = GetById(sprint.SprintId);
        if (existingSprint == null)
            _dbContext.Sprints.Add(sprint);
        else
            existingSprint = sprint with { };
        _dbContext.SaveChanges();
    }

    public void Delete(Guid sprintId)
    {
        var existingSprint = GetById(sprintId);
        if (existingSprint == null)
            return;

        var updatedAtTime = DateTime.Now;
        existingSprint = existingSprint with
        {
            IsDeleted = true,
            DeletedAt = updatedAtTime,
            UpdatedAt = updatedAtTime,
        };
        _dbContext.SaveChanges();
    }

    public Sprint? GetById(Guid sprintId) =>
        _dbContext.Sprints.SingleOrDefault(sprint => sprint.SprintId == sprintId);

    public List<Sprint> ListByProjectId(Guid projectId) =>
        _dbContext.Sprints.Where(x => x.ProjectId == projectId).ToList();
}
