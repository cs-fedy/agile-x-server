using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface ILabelRepository
{
    void Save(Label label);
    void Delete(Guid labelId);
    Label? GetById(Guid labelId);
    List<Label> ListUnique();
    List<Label> ListByTicketId(Guid ticketId);
}
