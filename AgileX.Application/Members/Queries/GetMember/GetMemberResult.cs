using AgileX.Domain.Entities;

namespace AgileX.Application.Members.Queries.GetMember;

public record GetMemberResult(Member Member, User User);
