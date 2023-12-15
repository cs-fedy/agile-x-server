using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Users.Commands.UpdateFullName;

public class UpdateFullNameCommandHandler
    : IRequestHandler<UpdateFullNameCommand, Result<SuccessMessage>>
{
    private readonly IUserRepository _userRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateFullNameCommandHandler(
        IUserRepository userRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<SuccessMessage>> Handle(
        UpdateFullNameCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingUser = _userRepository.GetById(request.UserId);
        if (existingUser is null || existingUser.IsDeleted)
            return UserErrors.UserNotFound;

        _userRepository.Save(
            existingUser with
            {
                FullName = request.FullName,
                UpdatedAt = _dateTimeProvider.UtcNow
            }
        );

        return new SuccessMessage("User profile update successfully");
    }
}
