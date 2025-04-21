using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundWorkers;

public class OutboxPublisher(IServiceScopeFactory scopeFactory, ILogger<OutboxPublisher> logger)
    : BackgroundService
{
    private readonly ILogger<OutboxPublisher> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var messages = await context.OutboxMessages
                .Where(x => !x.Processed)
                .OrderBy(x => x.OccurredOn)    // <-- garantiza un orden estable
                .Take(10)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                Console.WriteLine($"[EVENT PUBLISHED] {msg.Type}: {msg.Payload}");
                msg.Processed = true;
            }

            await context.SaveChangesAsync(stoppingToken);
            await Task.Delay(2000, stoppingToken);
        }
    }
}