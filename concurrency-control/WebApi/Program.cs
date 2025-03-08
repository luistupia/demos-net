using Domain.Abstracts.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Scalar.AspNetCore;
using WebApi.Modules;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Concurrency Control API");
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        options.WithDefaultHttpClient(ScalarTarget.Php, ScalarClient.HttpClient);
    });
}

app.AddBankAccountEndpoints();
app.UseHttpsRedirection();

app.Run();