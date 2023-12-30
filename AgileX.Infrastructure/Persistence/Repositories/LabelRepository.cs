using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class LabelRepository : ILabelRepository
{
    private readonly CustomDbContext _dbContext;

    public LabelRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void Save(Label label)
    {
        var existingLabel = GetById(label.LabelId);
        if (existingLabel == null)
            _dbContext.Labels.Add(label);
        else
            existingLabel = label with { };
        _dbContext.SaveChanges();
    }

    public void Delete(Guid labelId)
    {
        var existingLabel = GetById(labelId);
        if (existingLabel == null)
            return;

        existingLabel = existingLabel with { IsDeleted = true, DeletedAt = DateTime.Now };
        _dbContext.SaveChanges();
    }

    public Label? GetById(Guid labelId) =>
        _dbContext.Labels.SingleOrDefault(label => label.LabelId == labelId);

    public List<Label> ListUnique(Guid projectId) =>
        _dbContext.Labels.Where(label => label.ProjectId == projectId).Distinct().ToList();

    public List<Label> ListByTicketId(Guid ticketId) =>
        _dbContext.Labels.Where(label => label.TicketId == ticketId).ToList();

    public List<Label> List() => _dbContext.Labels.Distinct().ToList();
}
