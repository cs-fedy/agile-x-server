using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Users.Commands.UpdateUsername;

public class UpdateUsernameCommandHandler
    : IRequestHandler<UpdateUsernameCommand, Result<SuccessMessage>>
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateUsernameCommandHandler(
        IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        UpdateUsernameCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingUser = _userRepository.GetById(request.UserId);
        if (existingUser is null)
            return UserErrors.UserNotFound;

        var existingUserWithNewUsername = _userRepository.GetByUsername(request.NewUsername);
        if (existingUserWithNewUsername != null)
            return UserErrors.UsernameTaken;

        _userRepository.Save(
            existingUser with
            {
                Username = request.NewUsername,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        return new SuccessMessage("Username updated successfully");
    }
}
