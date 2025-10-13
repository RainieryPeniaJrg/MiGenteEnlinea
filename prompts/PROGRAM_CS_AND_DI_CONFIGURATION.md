# ⚙️ CONFIGURACIÓN COMPLETA DE PROGRAM.CS Y DEPENDENCY INJECTION

**Fecha de Creación:** 12 de octubre, 2025  
**Estado:** FASE 2 - DESPUÉS DE VALIDAR RELACIONES DB  
**Prioridad:** 🟡 ALTA  
**Prerequisito:** `DATABASE_RELATIONSHIPS_VALIDATION.md` completado  
**Agente Recomendado:** Claude Sonnet 4.5 (Modo Autónomo)

---

## 🎯 OBJETIVO

Configurar **completamente** el `Program.cs` de la API y el `DependencyInjection.cs` de Infrastructure para:

1. ✅ Registrar DbContext con connection string correcta
2. ✅ Aplicar **Assembly Scanning** para configuraciones Fluent API
3. ✅ Registrar servicios de Infrastructure (BCrypt, CurrentUser, Interceptors)
4. ✅ Configurar MediatR para CQRS (Application layer)
5. ✅ Registrar repositorios (cuando se implementen)
6. ✅ Configurar JWT Authentication (cuando se implemente)
7. ✅ Configurar CORS, Logging, y Middleware
8. ✅ Asegurar que NO haya conflictos en migrations

---

## 📊 ESTADO ACTUAL

### ✅ Lo que YA existe:

#### `DependencyInjection.cs` (Infrastructure Layer):
```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ✅ DbContext con Audit Interceptor
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

        // ✅ Identity Services
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        // ✅ Interceptors
        services.AddScoped<AuditableEntityInterceptor>();

        return services;
    }
}
```

**Estado:** ✅ **BUENO** - Pero falta registrar repositorios y otros servicios.

#### `Program.cs` (API Layer):
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
```

**Estado:** ⚠️ **INCOMPLETO** - Falta registrar Infrastructure, Application, Configuración, etc.

---

## 🚀 CONFIGURACIÓN COMPLETA DE PROGRAM.CS

### Archivo: `src/Presentation/MiGenteEnLinea.API/Program.cs`

Reemplazar **completamente** con esta configuración:

```csharp
using MiGenteEnLinea.Infrastructure;
using MiGenteEnLinea.Application;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// CONFIGURACIÓN DE LOGGING CON SERILOG
// ========================================
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "MiGenteEnLinea.API")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .WriteTo.Console()
    .WriteTo.File("logs/migente-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true
        })
    .CreateLogger();

builder.Host.UseSerilog();

// ========================================
// REGISTRAR CAPAS (Dependency Injection)
// ========================================

// Infrastructure Layer (DbContext, Identity, Services)
builder.Services.AddInfrastructure(builder.Configuration);

// Application Layer (MediatR, Validators, Mappings)
// TODO: Descomentar cuando se cree Application/DependencyInjection.cs
// builder.Services.AddApplication();

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
```

---

## 🔧 ACTUALIZAR DEPENDENCYINJECTION.CS (Infrastructure)

### Archivo: `src/Infrastructure/MiGenteEnLinea.Infrastructure/DependencyInjection.cs`

**Reemplazar completamente** con esta versión extendida:

```csharp
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

        // ========================================
        // IDENTITY SERVICES
        // ========================================
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

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
```

---

## 📦 CREAR APPLICATION LAYER DEPENDENCY INJECTION

### Archivo NUEVO: `src/Core/MiGenteEnLinea.Application/DependencyInjection.cs`

```csharp
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
```

---

## 📄 CONFIGURACIÓN DE APPSETTINGS.JSON

### Archivo: `src/Presentation/MiGenteEnLinea.API/appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=db_a9f8ff_migente;User Id=sa;Password=1234;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information",
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    }
  },
  "Jwt": {
    "SecretKey": "TU_SECRET_KEY_DE_AL_MENOS_32_CARACTERES_AQUI",
    "Issuer": "MiGenteEnLinea.API",
    "Audience": "MiGenteEnLinea.Client",
    "ExpirationHours": 8,
    "RefreshTokenExpirationDays": 7
  },
  "Cardnet": {
    "MerchantId": "349000001",
    "ApiKey": "TU_API_KEY_AQUI",
    "ApiUrlSales": "https://ecommerce.cardnet.com.do/api/payment/transactions/sales",
    "ApiUrlIdempotency": "https://ecommerce.cardnet.com.do/api/payment/idenpotency-keys",
    "UseTestMode": true
  },
  "Email": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "tu_email@gmail.com",
    "Password": "tu_password_aqui",
    "FromName": "MiGente En Línea",
    "EnableSsl": true
  },
  "AllowedHosts": "*"
}
```

### Archivo: `src/Presentation/MiGenteEnLinea.API/appsettings.Development.json`

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.EntityFrameworkCore.Database.Command": "Information"
      }
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=db_a9f8ff_migente;User Id=sa;Password=1234;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

---

## 📦 INSTALAR NUGET PACKAGES FALTANTES

Ejecutar estos comandos en la raíz de cada proyecto:

### Application Layer:
```bash
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Core\MiGenteEnLinea.Application"

dotnet add package MediatR --version 12.2.0
dotnet add package FluentValidation --version 11.9.0
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.9.0
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
```

### Infrastructure Layer:
```bash
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Infrastructure\MiGenteEnLinea.Infrastructure"

dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.MSSqlServer --version 6.5.0
dotnet add package Serilog.Sinks.File --version 5.0.0
```

### API Layer:
```bash
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Presentation\MiGenteEnLinea.API"

dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
```

---

## ✅ VALIDACIÓN COMPLETA

### 1. Compilar Solución
```bash
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean"
dotnet build
```

**Resultado esperado:** `Build succeeded. 0 Error(s)`

### 2. Ejecutar API
```bash
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Presentation\MiGenteEnLinea.API"
dotnet run
```

**Resultado esperado:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

### 3. Verificar Swagger
Abrir navegador en: `https://localhost:5001/`

**Resultado esperado:** Interfaz de Swagger UI cargada correctamente.

### 4. Verificar Health Check
Abrir navegador en: `https://localhost:5001/health`

**Resultado esperado:**
```json
{
  "Status": "Healthy",
  "Timestamp": "2025-10-12T15:30:00Z",
  "Environment": "Development"
}
```

### 5. Verificar Logs
Revisar archivo: `src/Presentation/MiGenteEnLinea.API/logs/migente-{fecha}.txt`

**Resultado esperado:** Logs de inicio sin errores.

---

## 🚨 TROUBLESHOOTING

### Error: "No se puede resolver el servicio IHttpContextAccessor"

**Solución:** Asegurar que en `Program.cs` esté registrado:
```csharp
builder.Services.AddHttpContextAccessor();
```

### Error: "Could not load assembly MediatR"

**Solución:** Instalar MediatR en Application layer:
```bash
cd src/Core/MiGenteEnLinea.Application
dotnet add package MediatR --version 12.2.0
```

### Error: "Connection string not found"

**Solución:** Verificar que `appsettings.json` tenga:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=db_a9f8ff_migente;..."
  }
}
```

### Error: "Serilog namespace not found"

**Solución:** Instalar paquetes Serilog en API layer:
```bash
cd src/Presentation/MiGenteEnLinea.API
dotnet add package Serilog.AspNetCore --version 8.0.0
```

---

## 📄 DOCUMENTACIÓN REQUERIDA

Después de completar la configuración, crear archivo:

**`MiGenteEnLinea.Clean/PROGRAM_CS_CONFIGURATION_REPORT.md`**

```markdown
# Reporte de Configuración de Program.cs y DI

**Fecha:** [FECHA]
**Ejecutado por:** [AGENTE/DEVELOPER]

## Resumen

- ✅ Program.cs configurado con Serilog, CORS, Swagger
- ✅ DependencyInjection.cs (Infrastructure) actualizado
- ✅ DependencyInjection.cs (Application) creado
- ✅ appsettings.json con connection string correcto
- ✅ NuGet packages instalados
- ✅ dotnet build: Success (0 errors)
- ✅ dotnet run: API ejecutándose en puerto 5001
- ✅ Swagger UI funcionando
- ✅ Health check endpoint OK
- ✅ Logs generándose correctamente

## Servicios Registrados

### Infrastructure Layer
- [x] DbContext con Audit Interceptor
- [x] ICurrentUserService
- [x] IPasswordHasher (BCrypt)
- [ ] IJwtTokenService (pendiente)
- [ ] Repositories (pendiente)
- [ ] External Services (pendiente)

### Application Layer
- [x] MediatR
- [x] FluentValidation
- [x] AutoMapper
- [ ] Behaviors (pendiente)

### API Layer
- [x] HttpContextAccessor
- [x] Controllers
- [x] Swagger
- [x] CORS
- [x] Serilog
- [ ] JWT Authentication (pendiente)

## Problemas Encontrados

[Lista de cualquier problema encontrado y cómo se resolvió]
```

---

## 🤖 INSTRUCCIONES PARA AGENTE AUTÓNOMO

**CONTEXTO:**
- Prerequisito: `DATABASE_RELATIONSHIPS_VALIDATION.md` completado ✅
- Workspace: `C:\Users\ray\OneDrive\Documents\ProyectoMigente\`

**AUTORIZACIÓN:**
- ✅ MODIFICAR `Program.cs`
- ✅ MODIFICAR `DependencyInjection.cs` (Infrastructure)
- ✅ CREAR `DependencyInjection.cs` (Application)
- ✅ MODIFICAR `appsettings.json` y `appsettings.Development.json`
- ✅ EJECUTAR `dotnet add package` para instalar NuGet
- ✅ EJECUTAR `dotnet build` y `dotnet run` para validar
- ❌ NO EJECUTAR migrations (`dotnet ef database update`)

**WORKFLOW:**

1. **INSTALAR** packages faltantes con `dotnet add package`
2. **CREAR** `Application/DependencyInjection.cs`
3. **REEMPLAZAR** `Program.cs` con la configuración completa
4. **ACTUALIZAR** `Infrastructure/DependencyInjection.cs`
5. **ACTUALIZAR** `appsettings.json` con connection string
6. **VALIDAR** con `dotnet build` (debe ser 0 errors)
7. **EJECUTAR** `dotnet run` y verificar que la API inicie
8. **VERIFICAR** Swagger en `https://localhost:5001/`
9. **VERIFICAR** Health check en `https://localhost:5001/health`
10. **REPORTAR** resultados en `PROGRAM_CS_CONFIGURATION_REPORT.md`

**COMENZAR EJECUCIÓN AUTOMÁTICA AHORA.**

---

_Última actualización: 12 de octubre, 2025_
