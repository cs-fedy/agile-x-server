using AgileX.Domain.Entities;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Attachments.Queries.GetAttachment;

public abstract record GetAttachmentQuery(Guid AttachmentId, Guid UserId) : IRequest<Result<Attachment>>;
