using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface ITicketRepository
{
    void Save(Ticket ticket);
    void Delete(Guid ticketId);
    Ticket? GetById(Guid ticketId);
    List<Ticket> ListBySprintId(Guid sprintId);
    IEnumerable<Ticket> ListByProjectId(Guid projectId);
    IEnumerable<Ticket> ListByAssignedUserId(Guid assignedUserId);
    IEnumerable<Ticket> ListByParentTicketId(Guid parentTicketId);
}
