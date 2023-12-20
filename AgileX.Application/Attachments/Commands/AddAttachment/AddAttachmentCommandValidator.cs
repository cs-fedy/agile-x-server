using FluentValidation;

namespace AgileX.Application.Attachments.Commands.AddAttachment;

public class AddAttachmentCommandValidator : AbstractValidator<AddAttachmentCommand>
{
    public AddAttachmentCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Url).NotEmpty();
        RuleFor(x => x.Type).NotEmpty();
    }
}
