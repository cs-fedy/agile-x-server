using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly CustomDbContext _dbContext;

    public ProjectRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void Save(Project project)
    {
        var existingProject = GetById(project.ProjectId);
        if (existingProject == null)
            _dbContext.Projects.Add(project);
        else
            existingProject = project with { };
        _dbContext.SaveChanges();
    }

    public Project? GetById(Guid projectId) =>
        _dbContext.Projects.SingleOrDefault(project => project.ProjectId == projectId);

    public void Delete(Guid projectId)
    {
        var existingProject = GetById(projectId);
        if (existingProject == null)
            return;

        var updatedAtTime = DateTime.Now;
        existingProject = existingProject with
        {
            IsDeleted = true,
            DeletedAt = updatedAtTime,
            UpdatedAt = updatedAtTime
        };

        _dbContext.SaveChanges();
    }
}
