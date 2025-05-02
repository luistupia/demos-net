using System.Net;
using Authentication.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Authentication.Api.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, message) = HandleHttpException(exception);
        
        logger.LogError(exception, exception.Message);
        
        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(message,cancellationToken);
        
        return true;
    }

    private (HttpStatusCode statusCode, string reasonPhrase) HandleHttpException(Exception exception)
    {
        return exception switch
        {
            LoginFailedException => (HttpStatusCode.Unauthorized, exception.Message),
            UserAlReadyExistsException => (HttpStatusCode.Conflict, exception.Message),
            RegistrationFailedException => (HttpStatusCode.BadRequest, exception.Message),
            RefreshTokenException => (HttpStatusCode.Unauthorized, exception.Message),
            _ => (HttpStatusCode.InternalServerError, $"An unexpected error ocurred: {exception.Message}")
        };
    }
}