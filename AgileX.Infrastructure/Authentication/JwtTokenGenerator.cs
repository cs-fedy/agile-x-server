using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Domain.ObjectValues;
using AgileX.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgileX.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> jwtOptions)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtOptions.Value;
    }

    public AccessToken GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Authentication, user.IsConfirmed ? "CONFIRMED" : "NOT_CONFIRMED"),
            new Claim(ClaimTypes.Role, user.Role == Role.USER ? "user" : "admin"),
        };

        var expiresIn = _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            claims: claims,
            audience: _jwtSettings.Audience,
            expires: expiresIn,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        );

        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return new AccessToken(token, expiresIn);
    }
}
