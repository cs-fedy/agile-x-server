using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Attachments.Commands.DeleteAttachment;

public class DeleteAttachmentCommandHandler
    : IRequestHandler<DeleteAttachmentCommand, Result<SuccessMessage>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IAttachmentRepository _attachmentRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteAttachmentCommandHandler(
        IProjectRepository projectRepository,
        IMemberRepository memberRepository,
        ITicketRepository ticketRepository,
        IAttachmentRepository attachmentRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _projectRepository = projectRepository;
        _memberRepository = memberRepository;
        _ticketRepository = ticketRepository;
        _attachmentRepository = attachmentRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        DeleteAttachmentCommand request,
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

        if (existingMember.UserId != existingTicket.AssignedUserId)
            return TicketErrors.UnauthorizedMember;

        var existingAttachment = _attachmentRepository.GetById(request.AttachmentId);
        if (existingAttachment is null || existingAttachment.IsDeleted)
            return AttachmentErrors.AttachmentNotFound;

        if (existingAttachment.TicketId != request.TicketId)
            return AttachmentErrors.InvalidAccessAttachment;

        _attachmentRepository.Delete(existingAttachment.AttachmentId);

        return new SuccessMessage("Attachment added successfully");
    }
}
