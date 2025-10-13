using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiGenteEnLinea.Application.Common.Interfaces;
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
        // ========================================
        // DATABASE CONTEXT
        // ========================================
        services.AddDbContext<MiGenteDbContext>((serviceProvider, options) =>
        {
            var auditInterceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();

            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    // Retry policy para conexiones intermitentes
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);

                    // Timeout de comandos
                    sqlOptions.CommandTimeout(60);

                    // Assembly de migrations (para separar migrations en Infrastructure)
                    sqlOptions.MigrationsAssembly(typeof(MiGenteDbContext).Assembly.FullName);
                })
                .AddInterceptors(auditInterceptor)
                .EnableSensitiveDataLogging(false) // Solo en desarrollo
                .EnableDetailedErrors(false); // Solo en desarrollo
        });

        // Registrar interfaz para Application Layer
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<MiGenteDbContext>());

        // ========================================
        // IDENTITY SERVICES
        // ========================================
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<Application.Common.Interfaces.IPasswordHasher, BCryptPasswordHasher>();

        // TODO: Agregar JWT Token Service cuando se implemente
        // services.AddScoped<IJwtTokenService, JwtTokenService>();

        // ========================================
        // INTERCEPTORS
        // ========================================
        services.AddScoped<AuditableEntityInterceptor>();

        // ========================================
        // REPOSITORIES (Generic Repository Pattern)
        // ========================================
        // TODO: Descomentar cuando se implementen los repositorios
        // services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        // services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositorios específicos
        // services.AddScoped<ICredencialRepository, CredencialRepository>();
        // services.AddScoped<IEmpleadorRepository, EmpleadorRepository>();
        // services.AddScoped<IContratistaRepository, ContratistaRepository>();
        // services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
        // services.AddScoped<ISuscripcionRepository, SuscripcionRepository>();

        // ========================================
        // EXTERNAL SERVICES
        // ========================================
        // TODO: Agregar cuando se migren del legacy
        // services.AddScoped<IEmailService, EmailService>();
        // services.AddScoped<ICardnetPaymentService, CardnetPaymentService>();
        // services.AddScoped<IPdfGenerationService, PdfGenerationService>();
        // services.AddScoped<IFileStorageService, FileStorageService>();

        return services;
    }
}
