using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Members.Queries.GetMember;

public record GetMemberQuery(Guid ProjectId, Guid LoggedUserId, Guid TargetUserId)
    : IRequest<Result<GetMemberResult>>;
