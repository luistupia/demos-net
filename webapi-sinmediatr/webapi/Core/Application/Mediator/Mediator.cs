namespace webapi.Core.Application.Mediator;

public class Mediator(IServiceProvider provider) : IMediator
{
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
    {
        // Resolver handler
        var handlerType = typeof(IHandler<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = provider.GetRequiredService(handlerType);

        // Construir pipeline
        RequestHandlerDelegate<TResponse> handlerDelegate =
            () => handler.Handle((dynamic)request);

        var pipelineType = typeof(IPipelineBehavior<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));

        var behaviors = (IEnumerable<dynamic>)provider.GetServices(pipelineType);
        foreach (var behavior in behaviors.Reverse())
        {
            var next = handlerDelegate;
            handlerDelegate = () => behavior.Handle((dynamic)request, next);
        }

        return await handlerDelegate();
    }
}