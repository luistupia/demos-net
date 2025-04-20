namespace webapi.Core.Application.Mediator;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
    
public interface IRequest<TResponse> { }

public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request);
}

public interface IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next);
}

public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request);
}