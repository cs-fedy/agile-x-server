using FluentValidation;

namespace AgileX.Application.Tickets.Queries.ListMemberTickets;

public class ListMemberTicketsQueryValidator : AbstractValidator<ListMemberTicketsQuery>
{
    public ListMemberTicketsQueryValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.LoggedUserId).NotEmpty();
        RuleFor(x => x.TargetUserId).NotEmpty();
    }
}
