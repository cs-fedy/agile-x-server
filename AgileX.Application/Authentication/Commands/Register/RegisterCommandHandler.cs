using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Events;
using AgileX.Domain.ObjectValues;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<SuccessMessage>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordProvider _passwordProvider;
    private readonly IEventProvider _eventProvider;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordProvider passwordProvider,
        IEventProvider eventProvider
    )
    {
        _userRepository = userRepository;
        _passwordProvider = passwordProvider;
        _eventProvider = eventProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingUser = _userRepository.GetUserByEmail(request.Email);
        if (existingUser != null)
            return UserErrors.UserAlreadyExist;

        var hashedPassword = _passwordProvider.hash(request.Password);
        var createdAt = DateTime.UtcNow;

        User createdUser =
            new(
                Guid.NewGuid(),
                request.Email,
                hashedPassword,
                request.FullName,
                request.Username,
                Role.USER,
                createdAt,
                createdAt
            );

        _userRepository.SaveUser(createdUser);

        await _eventProvider.Publish(new UserCreated(createdUser.UserId));

        return new SuccessMessage("user created successfully");
    }
}
