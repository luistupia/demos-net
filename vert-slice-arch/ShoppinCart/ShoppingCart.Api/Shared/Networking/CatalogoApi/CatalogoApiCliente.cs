using System;
using Polly;
using Polly.Registry;
using ShoppingCart.Api.Shared.Domain.Models;

namespace ShoppingCart.Api.Shared.Networking.CatalogoApi;

public sealed class CatalogoApiCliente(CatalogoApiService catalogoApiService,
    ILoggerFactory loggerFactory,
    ResiliencePipelineProvider<string> pipelineProvider) : ICatalogoApiCliente
{
    public Task<Catalogo> GetProductByCodeAsync(string code, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Catalogo>> GetProductsAsync(CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger("RetryLog");
        // var policy = Policy.Handle<ApplicationException>()
        //     .WaitAndRetryAsync(3, retryAttempt =>
        //     {
        //         logger.LogError($"Retried attempt {retryAttempt}");
        //         return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
        //     });

        //var products = await policy.ExecuteAsync(() => catalogoApiService.GetAllProductsAsync(cancellationToken));

        var pipeline = pipelineProvider.GetPipeline<IEnumerable<Catalogo>>("catalogo-products");
        var products = await pipeline.ExecuteAsync(async token => await catalogoApiService.GetAllProductsAsync(token),cancellationToken);
        
        return  products;
    }
}
