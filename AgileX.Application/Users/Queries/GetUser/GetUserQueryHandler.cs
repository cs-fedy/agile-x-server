using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Entities;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<User>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<Result<User>> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingUser = _userRepository.GetById(request.UserId);
        if (existingUser is null || existingUser.IsDeleted)
            return UserErrors.UserNotFound;

        return existingUser;
    }
}
