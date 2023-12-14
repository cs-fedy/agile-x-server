using AgileX.Application.Authentication.Common;
using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Errors;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Refresh;

public class RefreshCommandHandler : IRequestHandler<RefreshCommand, Result<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshGenerator _refreshGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshRepository _refreshRepository;
    private readonly ICacheRepository _cacheRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RefreshCommandHandler(
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshGenerator refreshGenerator,
        IUserRepository userRepository,
        IRefreshRepository refreshRepository,
        ICacheRepository cacheRepository,
        IDateTimeProvider dateTimeProvider
    )
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshGenerator = refreshGenerator;
        _userRepository = userRepository;
        _refreshRepository = refreshRepository;
        _cacheRepository = cacheRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<AuthenticationResult>> Handle(
        RefreshCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingRefresh = _refreshRepository.GetRefresh(request.Token);
        if (existingRefresh == null)
            return RefreshErrors.InvalidRefresh;

        if (existingRefresh.ExpiresIn > _dateTimeProvider.UtcNow)
            return RefreshErrors.InvalidRefresh;

        var existingUser = _userRepository.GetUserById(existingRefresh.OwnerId);
        if (existingUser == null)
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

        _refreshRepository.SaveRefresh(refresh);

        return new AuthenticationResult(AccessToken: token, RefreshToken: generatedRefreshToken);
    }
}
