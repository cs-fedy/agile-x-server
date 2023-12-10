using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Queries.Login;

public class LoginHandlerQuery : IRequestHandler<LoginQuery, Result<LoginResul>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshGenerator _refreshGenerator;
    private readonly IUserRepository _userRepository;
    private readonly ICacheRepository _cacheRepository;

    public LoginHandlerQuery(
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshGenerator refreshGenerator,
        IUserRepository userRepository,
        ICacheRepository cacheRepository
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshGenerator = refreshGenerator;
        _userRepository = userRepository;
        _cacheRepository = cacheRepository;
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
        await _cacheRepository.Cache($"token-list_{token.token}", true, token.expiresIn);
        var refresh = _refreshGenerator.Generate();

        return new LoginResul(AccessToken: token, RefreshToken: refresh);
    }
}
