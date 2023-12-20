using FluentValidation;

namespace AgileX.Application.Attachments.Queries.GetAttachment;

public class GetAttachmentQueryValidator : AbstractValidator<GetAttachmentQuery>
{
    public GetAttachmentQueryValidator()
    {
        RuleFor(x => x.AttachmentId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}
