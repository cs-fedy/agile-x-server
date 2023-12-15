using AgileX.Application.Common.Interfaces.Persistence;
using AgileX.Domain.Result;
using MediatR;

namespace AgileX.Application.Authentication.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<SuccessMessage>>
{
    private readonly IRefreshRepository _refreshRepository;
    private readonly ICacheRepository _cacheRepository;

    public LogoutCommandHandler(
        IRefreshRepository refreshRepository,
        ICacheRepository cacheRepository
    )
    {
        _refreshRepository = refreshRepository;
        _cacheRepository = cacheRepository;
    }

    public async Task<Result<SuccessMessage>> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken
    )
    {
        _refreshRepository.Delete(request.RefreshToken);
        await _cacheRepository.Remove($"token-list_{request.AccessToken}");
        return new SuccessMessage("Logged out successfully");
    }
}
