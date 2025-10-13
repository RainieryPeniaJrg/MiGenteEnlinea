using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MiGenteEnLinea.Infrastructure.Persistence.Contexts;

/// <summary>
/// Design-time factory para crear el DbContext durante las migraciones de EF Core.
/// Esto permite que 'dotnet ef migrations add' funcione sin ejecutar la aplicación completa.
/// </summary>
public class MiGenteDbContextFactory : IDesignTimeDbContextFactory<MiGenteDbContext>
{
    public MiGenteDbContext CreateDbContext(string[] args)
    {
        // Construir la configuración desde appsettings.json
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/MiGenteEnLinea.API"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

        var configuration = configurationBuilder.Build();

        // Obtener el connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'DefaultConnection' not found in appsettings.json. " +
                "Please ensure appsettings.json exists in the API project and contains a valid connection string.");
        }

        // Configurar el DbContext con el connection string
        var optionsBuilder = new DbContextOptionsBuilder<MiGenteDbContext>();
        
        optionsBuilder.UseSqlServer(
            connectionString,
            sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly("MiGenteEnLinea.Infrastructure");
                sqlServerOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlServerOptions.CommandTimeout(60);
            });

        // Habilitar logging detallado en modo desarrollo
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();

        return new MiGenteDbContext(optionsBuilder.Options);
    }
}
