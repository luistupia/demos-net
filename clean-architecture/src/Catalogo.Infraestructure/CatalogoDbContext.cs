using System;
using Catalogo.Domain.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Catalogo.Infraestructure;

public sealed class CatalogoDbContext(DbContextOptions options, IPublisher publisher) : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var results = await base.SaveChangesAsync(cancellationToken);
        await PublishNotifications();
        return results;
    }

    private async Task PublishNotifications()
    {
        var domainEventNotifications = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity)
            .SelectMany(e =>
            {
                var eventNotificactions = e.GetDomainEvents.ToList();
                e.ClearDomainEvents();
                return eventNotificactions;
            }).ToList();

        foreach (var eventNotification in domainEventNotifications)
        {
            await publisher.Publish(eventNotification);
        }
    }
}
