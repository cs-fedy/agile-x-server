using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    public void Save(Comment comment)
    {
        throw new NotImplementedException();
    }

    public void Delete(Guid commentId)
    {
        throw new NotImplementedException();
    }

    public Comment? GetById(Guid commentId)
    {
        throw new NotImplementedException();
    }

    public List<Comment> ListByTicketId(Guid ticketId)
    {
        throw new NotImplementedException();
    }

    public List<Comment> ListByParentCommentId(Guid parentCommentId)
    {
        throw new NotImplementedException();
    }
}
