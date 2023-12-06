using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Application.Common.Result;
using AgileX.Domain.Common.Errors;
using AgileX.Domain.Common.Result;
using AgileX.Domain.Entities;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<SuccessMessage>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordProvider _passwordProvider;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordProvider passwordProvider)
    {
        _userRepository = userRepository;
        _passwordProvider = passwordProvider;
    }
       

    public async Task<Result<SuccessMessage>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingUser = _userRepository.GetUserByEmail(request.Email);
        if (existingUser != null)
            return Errors.User.UserAlreadyExist;

        string hashedPassword = _passwordProvider.hash(request.Password);
        DateTime createdAt = DateTime.UtcNow;

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

        // TODO: emit an event using mediatR telling that the user has been created

        return new SuccessMessage("user created successfully");
    }
}
