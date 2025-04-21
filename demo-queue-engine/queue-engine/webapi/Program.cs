using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using webapi.Context;
using webapi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddConsole();

builder.Logging.AddFilter(
    category: "Microsoft.EntityFrameworkCore",
    level: LogLevel.Warning);

builder.Services.AddDbContext<AppDbContext>(opt => {
    opt.UseSqlite("Data Source=queue.db");
    opt.LogTo(_ => { }, LogLevel.None);
});
    

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

var server = app.Services.GetRequiredService<IServer>();
var addresses = server.Features
    .Get<IServerAddressesFeature>()!
    .Addresses;
Console.WriteLine("QueueEngine escuchando en:");
foreach (var addr in addresses)
    Console.WriteLine($"  → {addr}");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

QueueEndpoints.AddEndpoints(app);

app.UseHttpsRedirection();

app.Run();
