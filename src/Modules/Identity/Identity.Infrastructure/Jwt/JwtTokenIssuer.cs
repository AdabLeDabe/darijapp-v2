using System.Security.Claims;
using System.Text;
using Identity.Application;
using Identity.Application.Login;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Jwt;

public sealed class JwtTokenIssuer(IOptions<JwtOptions> options, TimeProvider clock)
    : ITokenIssuer
{
    public AccessToken Issue(AppUser user, IReadOnlyCollection<string> roles)
    {
        var o = options.Value;
        var now = clock.GetUtcNow();
        var expiresAt = now.AddMinutes(o.AccessTokenMinutes);

        var claims = new List<Claim>()
        {
            new("sub", user.Id.ToString()),
            new("email", user.Email!)
        };
        claims.AddRange(roles.Select(r => new Claim("role", r)));

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = o.Issuer,
            Audience = o.Audience,
            IssuedAt = now.UtcDateTime,
            NotBefore = now.UtcDateTime,
            Expires = expiresAt.UtcDateTime,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(o.Key)),
                SecurityAlgorithms.HmacSha256)
        };

        var token = new JsonWebTokenHandler().CreateToken(descriptor);
        return new AccessToken(token, expiresAt);
    }
}