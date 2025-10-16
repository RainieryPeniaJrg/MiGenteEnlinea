using MiGenteEnLinea.Infrastructure;
using MiGenteEnLinea.Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// CONFIGURACIÓN DE LOGGING CON SERILOG
// ========================================
var loggerConfig = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "MiGenteEnLinea.API")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .WriteTo.Console()
    .WriteTo.File("logs/migente-.txt", rollingInterval: RollingInterval.Day);

// Intentar agregar SQL Server sink (opcional si DB no está disponible)
try
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrEmpty(connectionString))
    {
        loggerConfig.WriteTo.MSSqlServer(
            connectionString: connectionString,
            sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true
            });
        Console.WriteLine("✅ Serilog: SQL Server sink configurado");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"⚠️ Serilog: No se pudo conectar a SQL Server para logs. Continuando con Console y File sinks. Error: {ex.Message}");
}

Log.Logger = loggerConfig.CreateLogger();

builder.Host.UseSerilog();

// ========================================
// REGISTRAR CAPAS (Dependency Injection)
// ========================================

// Infrastructure Layer (DbContext, Identity, Services)
builder.Services.AddInfrastructure(builder.Configuration);

// Application Layer (MediatR, Validators, Mappings)
builder.Services.AddApplication();

// ========================================
// ASP.NET CORE SERVICES
// ========================================

// HttpContext para CurrentUserService
builder.Services.AddHttpContextAccessor();

// Controllers con configuración de JSON
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // PascalCase
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// API Explorer para Swagger
builder.Services.AddEndpointsApiExplorer();

// Swagger con autenticación JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MiGente En Línea API",
        Version = "v1",
        Description = "API para gestión de empleadores, contratistas y nómina en República Dominicana",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "MiGente Support",
            Email = "soporte@migenteenlinea.com"
        }
    });

    // TODO: Descomentar cuando se implemente JWT
    /*
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    */
});

// ========================================
// CORS (permitir frontend localhost)
// ========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:4200", "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });

    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins("https://migenteenlinea.com", "https://www.migenteenlinea.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// ========================================
// BUILD APP
// ========================================
var app = builder.Build();

// ========================================
// MIDDLEWARE PIPELINE
// ========================================

// Serilog Request Logging
app.UseSerilogRequestLogging();

// Exception Handling
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // TODO: Implementar GlobalExceptionHandlerMiddleware para producción
    // app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

// Swagger (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MiGente API v1");
        options.RoutePrefix = string.Empty; // Swagger en raíz: https://localhost:5001/
    });
}

// HTTPS Redirection
app.UseHttpsRedirection();

// CORS
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevelopmentPolicy");
}
else
{
    app.UseCors("ProductionPolicy");
}

// Authentication & Authorization (TODO: Habilitar cuando JWT esté implementado)
// app.UseAuthentication();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Health Check Endpoint
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Environment = app.Environment.EnvironmentName
}));

// ========================================
// INICIALIZAR BASE DE DATOS (Opcional)
// ========================================
// TODO: Descomentar si necesitas aplicar migrations automáticamente en desarrollo
/*
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<MiGenteDbContext>();
    
    // Aplicar migrations pendientes
    await dbContext.Database.MigrateAsync();
}
*/

// ========================================
// RUN APP
// ========================================
try
{
    Log.Information("Iniciando MiGente En Línea API...");
    app.Run();
    Log.Information("API detenida correctamente.");
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciar.");
}
finally
{
    Log.CloseAndFlush();
}
