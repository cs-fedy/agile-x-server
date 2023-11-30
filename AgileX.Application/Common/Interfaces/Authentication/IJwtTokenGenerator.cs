using AgileX.Domain.Entities;

namespace AgileX.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
