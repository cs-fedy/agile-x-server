using FluentValidation;

namespace AgileX.Application.Members.Queries.GetMember;

public class GetMemberQueryValidator : AbstractValidator<GetMemberQuery>
{
    public GetMemberQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.LoggedUserId).NotEmpty();
        RuleFor(x => x.TargetUserId).NotEmpty();
    }
}
