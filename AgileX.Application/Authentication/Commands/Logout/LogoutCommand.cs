using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Logout;

public record LogoutCommand(Guid RefreshToken, string AccessToken)
    : IRequest<Result<SuccessMessage>>;
