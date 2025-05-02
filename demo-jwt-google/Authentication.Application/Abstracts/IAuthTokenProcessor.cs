using Authentication.Domain.Entities;

namespace Authentication.Application.Abstracts;

public interface IAuthTokenProcessor
{
    (string jwtToken, DateTime expires) GenerateJwtToken(User user);
    string GenerateRefreshToken();
    void WriteAuthTokenAsHttpOnlyCookie(string cookieName, string token, DateTime expiration);
}