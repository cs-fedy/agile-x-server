using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly CustomDbContext _dbContext;

    public TicketRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void Save(Ticket ticket)
    {
        var existingTicket = GetById(ticket.ProjectId);
        if (existingTicket == null)
            _dbContext.Tickets.Add(ticket);
        else
            existingTicket = ticket with { };
        _dbContext.SaveChanges();
    }

    public void Delete(Guid ticketId)
    {
        var existingTicket = GetById(ticketId);
        if (existingTicket == null)
            return;

        var updatedAtTime = DateTime.Now;
        existingTicket = existingTicket with
        {
            IsDeleted = true,
            DeletedAt = updatedAtTime,
            UpdatedAt = updatedAtTime
        };

        _dbContext.SaveChanges();
    }

    public Ticket? GetById(Guid ticketId) =>
        _dbContext.Tickets.SingleOrDefault(ticket => ticket.TicketId == ticketId);

    public List<Ticket> ListBySprintId(Guid sprintId) =>
        _dbContext.Tickets.Where(x => x.SprintId == sprintId).ToList();

    public IEnumerable<Ticket> ListByProjectId(Guid projectId) =>
        _dbContext.Tickets.Where(x => x.ProjectId == projectId).ToList();

    public IEnumerable<Ticket> ListByAssignedUserId(Guid assignedUserId) =>
        _dbContext.Tickets.Where(x => x.AssignedUserId == assignedUserId);

    public IEnumerable<Ticket> ListByParentTicketId(Guid parentTicketId) =>
        _dbContext.Tickets.Where(x => x.ParentTicketId == parentTicketId);
}
