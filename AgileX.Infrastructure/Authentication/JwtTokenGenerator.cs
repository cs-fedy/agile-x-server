using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using AgileX.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
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

    public string GenerateToken(User user)
    {
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var claims = new[]
        {
            new Claim("userId", user.UserId.ToString()),
            new Claim("isConfirmed", user.IsConfirmed.ToString()),
            new Claim("role", user.Role == Role.USER ? "user" : "admin"),
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            claims: claims,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
