// File: Application/Mediator/Behaviors/LoggingBehavior.cs

using webapi.Core.Application.Mediator;

namespace webapi.Core.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            logger.LogInformation("Handling {RequestName} - {@Request}", typeof(TRequest).Name, request);
            var response = await next();
            logger.LogInformation("Handled {RequestName} - {@Response}", typeof(TRequest).Name, response);
            return response;
        }
    }
}