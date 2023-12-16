using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Projects.Commands.DeleteProject;

public abstract record DeleteProjectCommand(Guid ProjectId, Guid UserId)
    : IRequest<Result<SuccessMessage>>;
