using System.Reflection;
using FluentValidation;
using webapi.Core.Application.Behaviors;
using webapi.Core.Application.Events;
using webapi.Core.Application.Mediator;


namespace webapi.Core.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var asm = Assembly.GetExecutingAssembly();
        
        services.AddScoped<IMediator, Mediator.Mediator>();
        services.AddScoped<IEventNotifier, EventNotifier>();

        // Registra con Scrutor todos los IHandler<,> del ensamblado
        services.Scan(scan => scan
            .FromAssemblyOf<IMediator>()
            .AddClasses(classes => classes.AssignableTo(typeof(IHandler<,>)))
            .AsImplementedInterfaces()
            .WithTransientLifetime());
        
        //Registra con Scrutor todos los IEventHandler<,> del ensamblado
        services.Scan(scan => scan
            .FromAssemblyOf<IMediator>()
            .AddClasses(classes => classes.AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));
        
        services.AddValidatorsFromAssembly(asm);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}