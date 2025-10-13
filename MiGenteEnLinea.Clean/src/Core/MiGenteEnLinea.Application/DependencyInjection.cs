using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace MiGenteEnLinea.Application;

/// <summary>
/// Extensión para registrar todos los servicios de Application en el contenedor DI
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // ========================================
        // MEDIATR (CQRS Pattern)
        // ========================================
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            // TODO: Agregar behaviors cuando se implementen
            // config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            // config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            // config.AddOpenBehavior(typeof(PerformanceBehavior<,>));
        });

        // ========================================
        // FLUENT VALIDATION
        // ========================================
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // ========================================
        // AUTOMAPPER (Object Mapping)
        // ========================================
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
