using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Members.Commands.RemoveMember;

public record RemoveMemberCommand(Guid LoggedUserId, Guid ProjectId, Guid TargetUserId)
    : IRequest<Result<SuccessMessage>>;
