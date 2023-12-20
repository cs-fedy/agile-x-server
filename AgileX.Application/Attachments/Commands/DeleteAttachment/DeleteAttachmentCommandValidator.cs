using FluentValidation;

namespace AgileX.Application.Attachments.Commands.DeleteAttachment;

public class DeleteAttachmentCommandValidator : AbstractValidator<DeleteAttachmentCommand>
{
    public DeleteAttachmentCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.AttachmentId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
