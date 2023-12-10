using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;

namespace AgileX.Application.Authentication.Queries.Login;

public record LoginResul(AccessToken AccessToken, RefreshToken RefreshToken);
