using Polly;
using Polly.Fallback;
using ShoppingCart.Api;
using ShoppingCart.Api.Features.Catalogo;
using ShoppingCart.Api.Shared.Domain.Models;
using ShoppingCart.Api.Shared.Extensions;
using ShoppingCart.Api.Shared.Networking.CatalogoApi;

var builder = WebApplication.CreateBuilder(args);


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


builder.Services.AddOpenApi();

builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices(builder.Configuration);

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
SearchProduct.AddEndpoint(app);
app.ApplyMigrations();

app.UseHttpsRedirection();

app.Run();