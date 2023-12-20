using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class TicketRepository : ITicketRepository
{
    public void Save(Ticket ticket)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid ticketId)
    {
        throw new NotImplementedException();
    }

    public Ticket? GetById(Guid ticketId)
    {
        throw new NotImplementedException();
    }

    public List<Ticket> ListBySprintId(Guid sprintId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Ticket> ListByProjectId(Guid projectId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Ticket> ListByAssignedUserId(Guid assignedUserId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Ticket> ListByParentTicketId(Guid parentTicketId)
    {
        throw new NotImplementedException();
    }
}
