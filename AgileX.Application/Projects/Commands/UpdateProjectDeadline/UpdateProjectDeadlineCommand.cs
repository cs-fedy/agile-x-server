using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Commands.UpdateProjectDeadline;

public record UpdateProjectDeadlineCommand(Guid ProjectId, Guid UserId, DateTime NewDeadline)
    : IRequest<Result<SuccessMessage>>;
