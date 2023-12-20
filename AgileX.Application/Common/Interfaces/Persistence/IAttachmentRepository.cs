using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface IAttachmentRepository
{
    void Save(Attachment attachment);
    void Delete(Guid attachmentId);
    Attachment? GetById(Guid attachmentId);
    List<Attachment> ListByTicketId(Guid ticketId);
}
