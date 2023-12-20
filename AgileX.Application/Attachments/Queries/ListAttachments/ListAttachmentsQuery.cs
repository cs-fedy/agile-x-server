using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Attachments.Queries.ListAttachments;

public abstract record ListAttachmentsQuery(Guid TicketId, Guid UserId)
    : IRequest<Result<List<Attachment>>>;
