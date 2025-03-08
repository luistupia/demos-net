using Polly;
using Polly.Fallback;
using ShoppingCart.Api.Features.Catalogo;
using ShoppingCart.Api.Shared.Domain.Models;
using ShoppingCart.Api.Shared.Networking.CatalogoApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICatalogoApiCliente, CatalogoApiCliente>();
builder.Services.AddHttpClient<CatalogoApiService>((serviceProvider, httpClient) =>
{
    httpClient.BaseAddress = new Uri("https://localhost:5001");
}).ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
{
    PooledConnectionLifetime = TimeSpan.FromMinutes(5),
}).SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddResiliencePipeline<string,IEnumerable<Catalogo>>("catalogo-products",
    pipelineBuilder =>
    {
        pipelineBuilder.AddFallback(new FallbackStrategyOptions<IEnumerable<Catalogo>>
        {
            FallbackAction = _ => Outcome.FromResultAsValueTask<IEnumerable<Catalogo>>(Enumerable.Empty<Catalogo>())
        });
    });


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // app.UseSwaggerUi(options =>
    // {
    //     options.Path = "/openapi";
    //     options.DocumentTitle = "/openapi/v1.json";
    // });
    app.UseReDoc(options =>
    { 
        options.Path = "/openapi";
        options.DocumentPath = "/openapi/v1.json";
    });
}



GetCatalogo.AddEndpoints(app);

app.UseHttpsRedirection();

app.Run();