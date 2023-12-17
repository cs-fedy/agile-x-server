using FluentValidation;

namespace AgileX.Application.Members.Queries.ListMembers;

public class ListMembersQueryValidator : AbstractValidator<ListMembersQuery>
{
    public ListMembersQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.LoggedUserId).NotEmpty();
    }
}
