using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Result;
using AgileX.Domain.Common.Errors;
using AgileX.Domain.Common.Result;
using AgileX.Domain.Entities;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<SuccessMessage>>
{
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository) =>
        _userRepository = userRepository;

    public async Task<Result<SuccessMessage>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingUser = _userRepository.GetUserByEmail(request.Email);
        if (existingUser != null)
            return Errors.User.UserAlreadyExist;

        DateTime createdAt = DateTime.UtcNow;
        User createdUser =
            new(
                Guid.NewGuid(),
                request.Email,
                request.Password,
                request.FullName,
                request.Username,
                "user",
                createdAt,
                createdAt
            );

        _userRepository.SaveUser(createdUser);

        return new SuccessMessage("user created successfully");
    }
}
