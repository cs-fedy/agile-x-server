using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;

namespace AgileX.Application.Common.Interfaces.Authentication;

public interface IRefreshGenerator
{
    RefreshToken Generate();
}
