using AgileX.Application.Common.Types;

namespace AgileX.Application.Common.Interfaces.Authentication;

public interface IRefreshGenerator
{
    RefreshToken Generate();
}
