using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<SuccessMessage>>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository) =>
        _userRepository = userRepository;

    public async Task<Result<SuccessMessage>> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingUser = _userRepository.GetById(request.UserId);
        if (existingUser is null || existingUser.IsDeleted)
            return UserErrors.UserNotFound;

        _userRepository.Delete(existingUser.UserId);
        return new SuccessMessage("User deleted successfully");
    }
}
