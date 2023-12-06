using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Queries.Login;

public class LoginHandlerQuery : IRequestHandler<LoginQuery, Result<LoginResul>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginHandlerQuery(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<Result<LoginResul>> Handle(
        LoginQuery request,
        CancellationToken cancellationToken
    )
    {
        await Task.CompletedTask;
        var existingUser = _userRepository.GetUserByEmail(request.Email);
        if (existingUser == null)
            return Errors.User.UserNotFound;

        var token = _jwtTokenGenerator.GenerateToken(existingUser);
        return new LoginResul(token);
    }
}
