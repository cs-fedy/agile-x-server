using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Result;
using AgileX.Domain.Common.Errors;
using MediatR;

namespace AgileX.Application.Authentication.Queries.Login;

public class LoginHandlerQuery : IRequestHandler<LoginQuery, Result<string>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginHandlerQuery(
        IJwtTokenGenerator jwtTokenGenerator,
        IUserRepository userRepository
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<Result<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var existingUser = _userRepository.GetUserByEmail(request.Email);
        if (existingUser == null)
            return Result<string>.From(Errors.User.UserAlreadyExist);

        var token = _jwtTokenGenerator.GenerateToken(existingUser);
        return Result<string>.From(token);
    }
}
