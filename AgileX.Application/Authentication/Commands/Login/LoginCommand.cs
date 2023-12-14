using AgileX.Application.Authentication.Common;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthenticationResult>>;
