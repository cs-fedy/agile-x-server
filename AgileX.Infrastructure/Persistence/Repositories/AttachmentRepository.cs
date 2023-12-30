using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class AttachmentRepository : IAttachmentRepository
{
    private readonly CustomDbContext _dbContext;

    public AttachmentRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void Save(Attachment attachment)
    {
        var existingAttachment = GetById(attachment.AttachmentId);
        if (existingAttachment == null)
            _dbContext.Attachments.Add(attachment);
        else
            existingAttachment = attachment with { };
        _dbContext.SaveChanges();
    }

    public void Delete(Guid attachmentId)
    {
        var existingAttachment = GetById(attachmentId);
        if (existingAttachment == null)
            return;

        existingAttachment = existingAttachment with
        {
            IsDeleted = true,
            DeletedAt = DateTime.Now,
        };

        _dbContext.SaveChanges();
    }

    public Attachment? GetById(Guid attachmentId) =>
        _dbContext
            .Attachments
            .SingleOrDefault(attachment => attachment.AttachmentId == attachmentId);

    public List<Attachment> ListByTicketId(Guid ticketId) =>
        _dbContext.Attachments.Where(x => x.TicketId == ticketId).ToList();
}
