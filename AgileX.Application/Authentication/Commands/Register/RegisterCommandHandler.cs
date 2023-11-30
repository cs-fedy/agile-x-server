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
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<Result<SuccessMessage>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingUser = _userRepository.GetUserByEmail(request.Email);
        if (existingUser != null)
            return Result<SuccessMessage>.From(Errors.User.UserAlreadyExist);

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

        var successMessage = new SuccessMessage("user created successfully");
        return Result<SuccessMessage>.From(successMessage);
    }
}
