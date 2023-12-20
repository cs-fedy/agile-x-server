using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Attachments.Commands.AddAttachment;

public abstract record AddAttachmentCommand(Guid TicketId, Guid UserId, string Url, string Type)
    : IRequest<Result<SuccessMessage>>;
