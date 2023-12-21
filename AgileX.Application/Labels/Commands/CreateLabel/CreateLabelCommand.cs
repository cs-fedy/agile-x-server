using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Labels.Commands.CreateLabel;

public abstract record CreateLabelCommand(Guid TicketId, Guid UserId, string Content)
    : IRequest<Result<SuccessMessage>>;
