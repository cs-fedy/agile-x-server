namespace AgileX.Domain.Events;

public record CommentCreated(Guid CommentId) : IEvent;

public record CommentDeleted(Guid CommentId) : IEvent;
