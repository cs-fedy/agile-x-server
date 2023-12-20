using FluentValidation;

namespace AgileX.Application.Attachments.Queries.ListAttachments;

public class ListAttachmentQueryValidator : AbstractValidator<ListAttachmentsQuery>
{
    public ListAttachmentQueryValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
