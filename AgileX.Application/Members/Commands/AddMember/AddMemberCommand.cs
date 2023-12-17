using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Members.Commands.AddMember;

public abstract record AddMemberCommand(Guid LoggedUserId, Guid ProjectId, Guid TargetUserId)
    : IRequest<Result<SuccessMessage>>;
