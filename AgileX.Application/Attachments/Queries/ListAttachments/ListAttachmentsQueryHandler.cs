using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Attachments.Queries.ListAttachments;

public class ListAttachmentsQueryHandler
    : IRequestHandler<ListAttachmentsQuery, Result<List<Attachment>>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IAttachmentRepository _attachmentRepository;

    public ListAttachmentsQueryHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ITicketRepository ticketRepository,
        IAttachmentRepository attachmentRepository
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _ticketRepository = ticketRepository;
        _attachmentRepository = attachmentRepository;
    }

    public async Task<Result<List<Attachment>>> Handle(
        ListAttachmentsQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingTicket = _ticketRepository.GetById(request.TicketId);
        if (existingTicket is null || existingTicket.IsDeleted)
            return TicketErrors.TicketNotFound;

        var existingProject = _projectRepository.GetById(existingTicket.ProjectId);
        if (existingProject is null || existingProject.IsDeleted)
            return ProjectErrors.ProjectNotFound;

        var existingMember = _memberRepository.Get(existingProject.ProjectId, request.UserId);
        if (existingMember is null || existingMember.IsDeleted)
            return MemberErrors.UnauthorizedMember;

        return _attachmentRepository
            .ListByTicketId(request.TicketId)
            .Where(x => !x.IsDeleted)
            .ToList();
    }
}
