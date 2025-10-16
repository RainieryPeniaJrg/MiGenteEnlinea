using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Interfaces;
using MiGenteEnLinea.Infrastructure.Identity.Services;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using MiGenteEnLinea.Infrastructure.Persistence.Interceptors;
using MiGenteEnLinea.Infrastructure.Services;
using Polly;
using Polly.Extensions.Http;

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
        
        // HttpClient para Padrón Nacional con retry policy
        services.AddHttpClient("PadronAPI", (serviceProvider, client) =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var baseUrl = config["PadronAPI:BaseUrl"];
            
            if (!string.IsNullOrEmpty(baseUrl))
            {
                client.BaseAddress = new Uri(baseUrl);
            }
            
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddPolicyHandler(GetRetryPolicy());

        // Memory Cache para Padrón (tokens + consultas)
        services.AddMemoryCache();

        // Padrón Service
        services.Configure<PadronSettings>(configuration.GetSection("PadronAPI"));
        services.AddScoped<IPadronService, PadronService>();

        // Nómina Calculator Service (Nota: Ya está registrado en Application layer DI)
        // services.AddScoped<INominaCalculatorService, NominaCalculatorService>();

        // ========================================
        // PAYMENT GATEWAY (CARDNET)
        // ========================================
        
        // Configuración de Cardnet
        services.Configure<CardnetSettings>(configuration.GetSection("Cardnet"));

        // HttpClient para Cardnet con retry policy y circuit breaker
        services.AddHttpClient("CardnetAPI", (serviceProvider, client) =>
        {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            var baseUrl = config["Cardnet:BaseUrl"];
            
            if (!string.IsNullOrEmpty(baseUrl))
            {
                client.BaseAddress = new Uri(baseUrl);
            }
            
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Content-Type", "application/json");
        })
        .AddPolicyHandler(GetRetryPolicy()) // Retry 3 veces
        .AddPolicyHandler(GetCircuitBreakerPolicy()); // Circuit breaker después de 5 fallos

        // Payment Service
        // TODO: Descomentar cuando CardnetPaymentService esté implementado (Fase 5)
        // services.AddScoped<IPaymentService, CardnetPaymentService>();

        // TODO: Agregar cuando se migren del legacy
        // services.AddScoped<IEmailService, EmailService>();
        // services.AddScoped<IPdfGenerationService, PdfGenerationService>();
        // services.AddScoped<IFileStorageService, FileStorageService>();

        return services;
    }

    /// <summary>
    /// Política de reintentos con backoff exponencial para llamadas HTTP.
    /// 3 intentos: 0s → 2s → 4s → 8s (máximo 14s total).
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // 5xx, 408, network failures
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    // Log retry attempts (opcional)
                    Console.WriteLine($"[Retry {retryAttempt}] Reintenando después de {timespan.TotalSeconds}s...");
                });
    }

    /// <summary>
    /// Circuit Breaker policy para evitar saturar servicios externos con errores.
    /// Abre el circuito después de 5 fallos consecutivos y lo mantiene abierto por 30 segundos.
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, duration) =>
                {
                    Console.WriteLine($"[Circuit Breaker] Circuito abierto por {duration.TotalSeconds}s debido a múltiples fallos.");
                },
                onReset: () =>
                {
                    Console.WriteLine("[Circuit Breaker] Circuito cerrado, reanudando llamadas.");
                });
    }
}
