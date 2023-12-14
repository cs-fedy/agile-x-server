using AgileX.Application.Authentication.Common;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Refresh;

public record RefreshCommand(Guid Token) : IRequest<Result<AuthenticationResult>>;
