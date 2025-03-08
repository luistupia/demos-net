using Catalogo.Api.Extensions;
using Catalogo.Application;
using Catalogo.Infraestructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalogo.Api", Version = "v1" });
});


builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplication();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrations();
await app.SeedCatalogoProduct();
app.UseHttpsRedirection();

app.MapControllers();
app.UseDefaultFiles();
app.UseStaticFiles();
app.Run();
