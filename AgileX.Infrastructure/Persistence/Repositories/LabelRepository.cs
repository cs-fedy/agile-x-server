using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class LabelRepository : ILabelRepository
{
    public void Save(Label label)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid labelId)
    {
        throw new NotImplementedException();
    }

    public Label? GetById(Guid labelId)
    {
        throw new NotImplementedException();
    }

    public List<Label> ListUnique(Guid projectId)
    {
        throw new NotImplementedException();
    }

    public List<Label> ListByTicketId(Guid ticketId)
    {
        throw new NotImplementedException();
    }

    public List<Label> List()
    {
        throw new NotImplementedException();
    }
}
