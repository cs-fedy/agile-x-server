using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Register;

public abstract record RegisterCommand(
    string Email,
    string Password,
    string Username,
    string FullName
) : IRequest<Result<SuccessMessage>>;
