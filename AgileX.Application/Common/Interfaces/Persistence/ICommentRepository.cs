using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Persistence;

public interface ICommentRepository
{
    void Save(Comment comment);
    void Delete(Guid commentId);
    Comment? GetById(Guid commentId);
    List<Comment> ListByTicketId(Guid ticketId);
    List<Comment> ListByParentCommentId(Guid parentCommentId);
}