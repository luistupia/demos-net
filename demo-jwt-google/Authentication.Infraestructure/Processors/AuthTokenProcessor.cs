using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Authentication.Application.Abstracts;
using Authentication.Domain.Entities;
using Authentication.Infraestructure.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Infraestructure.Processors;

public class AuthTokenProcessor(IOptions<JwtOptions> options, IHttpContextAccessor httpContextAccessor) : IAuthTokenProcessor
{
    private readonly JwtOptions _jwtOptions = options.Value;

    public (string jwtToken, DateTime expires) GenerateJwtToken(User user)
    {
        var singninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
        
        var credentials = new SigningCredentials(singninKey,SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.NameIdentifier, user.ToString()),
        };

        var expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationTimeInMinutes);
        var token = new JwtSecurityToken(issuer:_jwtOptions.Issuer,audience:_jwtOptions.Audience,claims: claims,expires:expires,signingCredentials:credentials);
        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        return (jwtToken, expires);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public void WriteAuthTokenAsHttpOnlyCookie(string cookieName,string token,DateTime expiration)
    {
        httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName,token,new CookieOptions
        {
            HttpOnly = true,
            Expires = expiration,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
    }
    
    
}