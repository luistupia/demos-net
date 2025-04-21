using Application;
using Application.Handlers;
using BackgroundWorkers;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=orders.db"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHostedService<OutboxPublisher>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() {
        Title = "Order API",
        Version = "v1",
        Description = "API para gestionar órdenes con patrón Outbox"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
    c.RoutePrefix = ""; // descomenta si quieres que esté en "/"
});

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}


app.MapPost("/orders", async (CreateOrderCommand cmd, IUnitOfWork uow) =>
{
    var handler = new CreateOrderHandler(uow);
    var orderId = await handler.HandleAsync(cmd);
    return Results.Created($"/orders/{orderId}", new { orderId });
});

app.Run();