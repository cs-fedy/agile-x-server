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
        var existingUser = _userRepository.GetByEmail(request.Email);
        if (existingUser != null)
            return UserErrors.UserAlreadyExist;

        var userId = Guid.NewGuid();
        var hashedPassword = _passwordProvider.hash(request.Password);
        var createdAt = DateTime.UtcNow;

        _userRepository.Save(
            new User(
                UserId: userId,
                Email: request.Email,
                Password: hashedPassword,
                FullName: request.FullName,
                Username: request.Username,
                Role: Role.USER,
                CreatedAt: createdAt,
                UpdatedAt: createdAt,
                IsDeleted: false,
                DeletedAt: null,
                IsConfirmed: false
            )
        );

        await _eventProvider.Publish(new UserCreated(userId));

        return new SuccessMessage("user created successfully");
    }
}
