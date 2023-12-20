using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class AttachmentRepository : IAttachmentRepository
{
    public void Save(Attachment attachment)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid attachmentId)
    {
        throw new NotImplementedException();
    }

    public Attachment? GetById(Guid attachmentId)
    {
        throw new NotImplementedException();
    }

    public List<Attachment> ListByTicketId(Guid ticketId)
    {
        throw new NotImplementedException();
    }
}
