using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;

namespace AgileX.Infrastructure.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly CustomDbContext _dbContext;

    public CommentRepository(CustomDbContext dbContext) => _dbContext = dbContext;

    public void Save(Comment comment)
    {
        var existingComment = GetById(comment.CommentId);
        if (existingComment == null)
            _dbContext.Comments.Add(comment);
        else
            existingComment = comment with { };
        _dbContext.SaveChanges();
    }

    public void Delete(Guid commentId)
    {
        var existingComment = GetById(commentId);
        if (existingComment == null)
            return;

        existingComment = existingComment with
        {
            IsDeleted = true,
            DeletedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        _dbContext.SaveChanges();
    }

    public Comment? GetById(Guid commentId) =>
        _dbContext.Comments.SingleOrDefault(comment => comment.CommentId == commentId);

    public List<Comment> ListByTicketId(Guid ticketId) =>
        _dbContext.Comments.Where(x => x.TicketId == ticketId).ToList();

    public List<Comment> ListByParentCommentId(Guid parentCommentId) =>
        _dbContext.Comments.Where(x => x.ParentCommentId == parentCommentId).ToList();
}
