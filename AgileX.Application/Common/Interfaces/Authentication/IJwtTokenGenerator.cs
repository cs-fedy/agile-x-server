using AgileX.Application.Common.Types;
using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;

namespace AgileX.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    AccessToken GenerateToken(User user);
}
