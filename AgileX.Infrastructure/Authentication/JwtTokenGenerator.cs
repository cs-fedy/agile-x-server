using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgileX.Application.Common.Interfaces.Authentication;
using AgileX.Application.Common.Interfaces.Services;
using AgileX.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgileX.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(
        IDateTimeProvider dateTimeProvider,
        IOptions<JwtSettings> jwtOptions
    )
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(s: _jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256
        );
#pragma warning restore CS8604 // Possible null reference argument.

        var claims = new[]
        {
            new Claim("userId", user.UserId.ToString()),
            new Claim("role", user.Role)
        };

        var accessToken = new JwtSecurityToken(
            claims: claims,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: signingCredentials,
            issuer: _jwtSettings.Issuer
        );

        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }
}
