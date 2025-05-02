using Authentication.Application.Abstracts;
using Authentication.Domain.Entities;
using Authentication.Domain.Exceptions;
using Authentication.Domain.Requests;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Authentication.Application.Services;

public class AccountService(IAuthTokenProcessor tokenProcessor, UserManager<User> userManager, IUserRepository userRepository) : IAccountService
{
    public async Task RegisterAsync(RegisterRequest registerRequest)
    {
        var userExists = await userManager.FindByEmailAsync(registerRequest.Email) != null;
        if (userExists) throw new UserAlReadyExistsException(email: registerRequest.Email);

        var user = User.Create(registerRequest.Email, registerRequest.FirstName, registerRequest.LastName);
        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, registerRequest.Password);

        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded) throw new RegistrationFailedException(result.Errors.Select(x => x.Description));
    }

    public async Task LoginAsync(LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginRequest.Password))
            throw new LoginFailedException(loginRequest.Email);

        var (jwtToken, expirationDateInUtc) = tokenProcessor.GenerateJwtToken(user);
        var refreshTokenValue = tokenProcessor.GenerateRefreshToken();

        var refreshtokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiresAtUtc = refreshtokenExpirationDateInUtc;

        await userManager.UpdateAsync(user);

        tokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
        tokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshtokenExpirationDateInUtc);
    }

    public async Task RefreshTokenAsync(string? refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken)) throw new RefreshTokenException("Refresh token is required");

        var user = await userRepository.GetUserByRefreshTokenAsync(refreshToken);

        if (user == null) throw new RefreshTokenException("Unable to retrieve user for refresh token");

        if (user.RefreshTokenExpiresAtUtc < DateTime.UtcNow) throw new RefreshTokenException("Refresh token has expired");

        var (jwtToken, expirationDateInUtc) = tokenProcessor.GenerateJwtToken(user);
        var refreshTokenValue = tokenProcessor.GenerateRefreshToken();

        var refreshtokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiresAtUtc = refreshtokenExpirationDateInUtc;

        await userManager.UpdateAsync(user);

        tokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
        tokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshtokenExpirationDateInUtc);
    }

    public async Task LoginWithGoogleAsync(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal == null)
            throw new ExternalLoginProviderException("Google", "ClaimsPrincipal is null");

        var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);
        if (email == null)
            throw new ExternalLoginProviderException("Google", "Email claim is null");

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                Email = email,
                UserName = email,
                FirstName = claimsPrincipal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty,
                LastName = claimsPrincipal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty
            };
            var result = await userManager.CreateAsync(user);

            if (!result.Succeeded)
                throw new RegistrationFailedException(result.Errors.Select(x => x.Description));
        }

        var info = new UserLoginInfo("Google",
            claimsPrincipal.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            "Google");

        var loginResult = await userManager.AddLoginAsync(user, info);
        if (!loginResult.Succeeded)
            throw new ExternalLoginProviderException("Google"
                , $"Unable to add login info to user {loginResult.Errors.Select(x => x.Description)}");


        var (jwtToken, expirationDateInUtc) = tokenProcessor.GenerateJwtToken(user);
        var refreshTokenValue = tokenProcessor.GenerateRefreshToken();

        var refreshtokenExpirationDateInUtc = DateTime.UtcNow.AddDays(7);

        user.RefreshToken = refreshTokenValue;
        user.RefreshTokenExpiresAtUtc = refreshtokenExpirationDateInUtc;

        await userManager.UpdateAsync(user);

        tokenProcessor.WriteAuthTokenAsHttpOnlyCookie("ACCESS_TOKEN", jwtToken, expirationDateInUtc);
        tokenProcessor.WriteAuthTokenAsHttpOnlyCookie("REFRESH_TOKEN", user.RefreshToken, refreshtokenExpirationDateInUtc);
    }
}