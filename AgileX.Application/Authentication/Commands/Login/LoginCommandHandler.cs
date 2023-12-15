using AgileX.Application.Authentication.Common;
using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshGenerator _refreshGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshRepository _refreshRepository;
    private readonly ICacheRepository _cacheRepository;

    public LoginCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshGenerator refreshGenerator,
        IUserRepository userRepository,
        IRefreshRepository refreshRepository,
        ICacheRepository cacheRepository
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshGenerator = refreshGenerator;
        _userRepository = userRepository;
        _refreshRepository = refreshRepository;
        _cacheRepository = cacheRepository;
    }

    public async Task<Result<AuthenticationResult>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingUser = _userRepository.GetByEmail(request.Email);
        if (existingUser is null || existingUser.IsDeleted)
            return UserErrors.UserNotFound;

        var token = _jwtTokenGenerator.GenerateToken(existingUser);
        await _cacheRepository.Cache($"token-list_{token.Token}", true, token.ExpiresIn);

        var generatedRefreshToken = _refreshGenerator.Generate();

        Domain.Entities.Refresh refresh =
            new(
                Token: generatedRefreshToken.Token,
                OwnerId: existingUser.UserId,
                ExpiresIn: generatedRefreshToken.ExpiresIn
            );

        _refreshRepository.Save(refresh);

        return new AuthenticationResult(AccessToken: token, RefreshToken: generatedRefreshToken);
    }
}
