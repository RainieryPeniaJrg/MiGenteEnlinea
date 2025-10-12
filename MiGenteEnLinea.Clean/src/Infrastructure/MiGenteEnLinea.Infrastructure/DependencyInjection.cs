using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiGenteEnLinea.Domain.Interfaces;
using MiGenteEnLinea.Infrastructure.Identity.Services;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using MiGenteEnLinea.Infrastructure.Persistence.Interceptors;

namespace MiGenteEnLinea.Infrastructure;

/// <summary>
/// Extensión para registrar todos los servicios de Infrastructure en el contenedor DI
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext con interceptor de auditoría
        services.AddDbContext<MiGenteDbContext>((serviceProvider, options) =>
        {
            var auditInterceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();

            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(60);
                })
                .AddInterceptors(auditInterceptor);
        });

        // Identity Services
        // services.AddHttpContextAccessor(); // TODO: This will be registered in API layer
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        // Interceptors
        services.AddScoped<AuditableEntityInterceptor>();

        return services;
    }
}
