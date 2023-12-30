using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Comments.Commands.CreateComment;

public class CreateCommentCommandHandler
    : IRequestHandler<CreateCommentCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEventProvider _eventProvider;

    public CreateCommentCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ITicketRepository ticketRepository,
        ICommentRepository commentRepository,
        IDateTimeProvider dateTimeProvider,
        IEventProvider eventProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _ticketRepository = ticketRepository;
        _commentRepository = commentRepository;
        _dateTimeProvider = dateTimeProvider;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        CreateCommentCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingTicket = _ticketRepository.GetById(request.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return TicketErrors.TicketNotFound;

        var existingProject = _projectRepository.GetById(existingTicket.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(existingProject.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        if (request.ParentCommentId is not null)
        {
            var existingComment = _commentRepository.GetById(request.ParentCommentId.Value);
            if (existingComment is null || existingComment.IsDeleted)
                return CommentErrors.CommentNotFound;
        }

        var commentId = Guid.NewGuid();
        var creationDate = _dateTimeProvider.UtcNow;

        _commentRepository.Save(
            new Comment(
                CommentId: commentId,
                TicketId: request.TicketId,
                ParentCommentId: request.ParentCommentId,
                CommentedBy: request.UserId,
                Text: request.Text,
                AttachedCode: request.AttachedCode,
                SubCommentsCount: 0,
                IsDeleted: false,
                DeletedAt: null,
                CreatedAt: creationDate,
                UpdatedAt: creationDate
            )
        );

        await _eventProvider.Publish(new CommentCreated(commentId));
        return new SuccessMessage("Comment created successfully");
    }
}
