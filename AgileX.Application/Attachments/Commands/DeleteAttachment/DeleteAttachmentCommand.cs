using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Attachments.Commands.DeleteAttachment;

public abstract record DeleteAttachmentCommand(Guid TicketId, Guid UserId, Guid AttachmentId)
    : IRequest<Result<SuccessMessage>>;
