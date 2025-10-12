# 🛡️ INSTRUCCIONES DE MEJORA - MI GENTE EN LÍNEA

## 📌 CONTEXTO DEL PROYECTO

### Descripción
Sistema de gestión financiera "Mi Gente en Línea" que requiere remediación completa de vulnerabilidades de seguridad identificadas en auditoría técnica (Sept 2025) y reestructuración arquitectónica.

### Estado Actual
- Base de datos SQL Server (Database-First)
- ASP.NET Core Web API
- Múltiples vulnerabilidades críticas de seguridad
- Arquitectura monolítica sin separación de capas
- Falta de patrones de diseño y mejores prácticas

---

## 🎯 OBJETIVOS PRINCIPALES

### 1. REMEDIACIÓN DE SEGURIDAD (Prioridad Crítica)
- [ ] **SQL Injection**: Eliminar todas las consultas SQL concatenadas
- [ ] **Contraseñas**: Implementar hashing seguro (BCrypt/Argon2)
- [ ] **Autenticación**: Implementar JWT con refresh tokens
- [ ] **Autorización**: Sistema basado en roles y políticas
- [ ] **Validación**: FluentValidation en todos los inputs
- [ ] **Logging**: Serilog con auditoría completa
- [ ] **Rate Limiting**: Protección contra fuerza bruta
- [ ] **CORS**: Configuración restrictiva por ambiente

### 2. MIGRACIÓN ARQUITECTÓNICA
- [ ] Migrar de Database-First a **Code-First con Entity Framework Core**
- [ ] Implementar **Clean Architecture (Onion Architecture)**
- [ ] Separación de capas: Domain, Application, Infrastructure, API
- [ ] Implementar patrones: Repository, Unit of Work, CQRS, Mediator

### 3. CALIDAD Y TESTING
- [ ] Unit Tests con xUnit (mínimo 80% cobertura)
- [ ] Integration Tests para endpoints críticos
- [ ] Tests de seguridad automatizados

---

## 🏗️ ARQUITECTURA OBJETIVO

### Estructura de Capas (Onion Architecture)

```
MiGenteEnLinea/
├── src/
│   ├── Core/
│   │   ├── MiGenteEnLinea.Domain/              # Entidades, Value Objects, Interfaces
│   │   │   ├── Entities/
│   │   │   │   ├── Usuario.cs
│   │   │   │   ├── Cliente.cs
│   │   │   │   ├── Prestamo.cs
│   │   │   │   ├── Pago.cs
│   │   │   │   └── Transaccion.cs
│   │   │   ├── ValueObjects/
│   │   │   ├── Enums/
│   │   │   └── Interfaces/
│   │   │       ├── Repositories/
│   │   │       └── Services/
│   │   │
│   │   └── MiGenteEnLinea.Application/         # Casos de uso, DTOs, Validators
│   │       ├── Common/
│   │       │   ├── Interfaces/
│   │       │   ├── Behaviors/
│   │       │   └── Exceptions/
│   │       ├── Features/
│   │       │   ├── Usuarios/
│   │       │   │   ├── Commands/
│   │       │   │   ├── Queries/
│   │       │   │   ├── DTOs/
│   │       │   │   └── Validators/
│   │       │   ├── Clientes/
│   │       │   ├── Prestamos/
│   │       │   └── Pagos/
│   │       └── DependencyInjection.cs
│   │
│   ├── Infrastructure/
│   │   ├── MiGenteEnLinea.Infrastructure/      # Persistencia, Identity, Logging
│   │   │   ├── Persistence/
│   │   │   │   ├── Contexts/
│   │   │   │   ├── Configurations/
│   │   │   │   ├── Repositories/
│   │   │   │   └── Migrations/
│   │   │   ├── Identity/
│   │   │   │   ├── Services/
│   │   │   │   └── Models/
│   │   │   ├── Services/
│   │   │   └── DependencyInjection.cs
│   │   │
│   │   └── MiGenteEnLinea.Shared/              # Utilidades compartidas
│   │       ├── Extensions/
│   │       ├── Helpers/
│   │       └── Constants/
│   │
│   └── Presentation/
│       └── MiGenteEnLinea.API/                 # Controllers, Middleware, Filters
│           ├── Controllers/
│           ├── Middleware/
│           ├── Filters/
│           ├── Extensions/
│           └── Program.cs
│
├── tests/
│   ├── MiGenteEnLinea.Domain.Tests/
│   ├── MiGenteEnLinea.Application.Tests/
│   ├── MiGenteEnLinea.Infrastructure.Tests/
│   └── MiGenteEnLinea.API.Tests/
│
└── docs/
    ├── SECURITY.md
    ├── ARCHITECTURE.md
    └── API_DOCUMENTATION.md
```

---

## 🔒 ESTÁNDARES DE SEGURIDAD OBLIGATORIOS

### 1. Manejo de Contraseñas
```csharp
// NUNCA HACER (Actual)
password = "plaintext"

// SIEMPRE HACER (Objetivo)
using BCrypt.Net;
string hashedPassword = BCrypt.HashPassword(password, workFactor: 12);
bool isValid = BCrypt.Verify(password, hashedPassword);
```

### 2. Prevención de SQL Injection
```csharp
// ❌ NUNCA (Vulnerable)
var query = $"SELECT * FROM Usuarios WHERE Username = '{username}'";

// ✅ SIEMPRE (Seguro)
var usuario = await _context.Usuarios
    .Where(u => u.Username == username)
    .FirstOrDefaultAsync();
```

### 3. Autenticación JWT
```csharp
// Configuración segura
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
            ClockSkew = TimeSpan.Zero
        };
    });
```

### 4. Validación de Entrada
```csharp
// FluentValidation obligatorio
public class CrearUsuarioCommandValidator : AbstractValidator<CrearUsuarioCommand>
{
    public CrearUsuarioCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(3, 50)
            .Matches("^[a-zA-Z0-9_]+$");
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
            
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]");
    }
}
```

### 5. Logging y Auditoría
```csharp
// Serilog configurado para auditoría
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "MiGenteEnLinea")
    .WriteTo.Console()
    .WriteTo.File("logs/app-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        connectionString,
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = true })
    .CreateLogger();
```

---

## 📝 HALLAZGOS ESPECÍFICOS A REMEDIAR

### 🔴 CRÍTICOS (Atender Inmediatamente)

#### 1. SQL Injection en LoginController
**Ubicación**: `Controllers/LoginController.cs:42`
```csharp
// VULNERABLE
string query = $"SELECT * FROM Usuarios WHERE username = '{model.Username}' AND password = '{model.Password}'";

// REMEDIACIÓN
var usuario = await _context.Usuarios
    .Include(u => u.Rol)
    .Where(u => u.Username == model.Username && u.IsActive)
    .FirstOrDefaultAsync();

if (usuario == null || !BCrypt.Verify(model.Password, usuario.PasswordHash))
{
    return Unauthorized(new { message = "Credenciales inválidas" });
}
```

#### 2. Contraseñas en Texto Plano
**Ubicación**: Múltiples controllers de registro/edición
```csharp
// IMPLEMENTAR en Application Layer
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

public class BCryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password) 
        => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
    
    public bool VerifyPassword(string password, string hashedPassword) 
        => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
}
```

#### 3. Falta de Autenticación en Endpoints Críticos
**Ubicación**: Todos los controllers
```csharp
// APLICAR A TODOS LOS ENDPOINTS
[Authorize(Roles = "Admin,Gerente")]
[HttpGet]
public async Task<IActionResult> GetAll()
{
    // implementación
}

[Authorize]
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    // Verificar que el usuario solo acceda a sus propios datos
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    // validación...
}
```

#### 4. Información Sensible en Respuestas de Error
```csharp
// MIDDLEWARE GLOBAL DE MANEJO DE EXCEPCIONES
public class GlobalExceptionHandlerMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                message = "Ha ocurrido un error procesando su solicitud",
                requestId = Activity.Current?.Id ?? context.TraceIdentifier
            });
        }
    }
}
```

### 🟡 ALTOS (Atender esta Sprint)

#### 5. CORS Permisivo
```csharp
// appsettings.json
"AllowedOrigins": ["https://app.migenteenlinea.com"]

// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .WithHeaders("Authorization", "Content-Type")
              .AllowCredentials();
    });
});
```

#### 6. Falta de Rate Limiting
```csharp
// Implementar AspNetCoreRateLimit
services.AddMemoryCache();
services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
services.AddInMemoryRateLimiting();

// appsettings.json
"IpRateLimiting": {
  "EnableEndpointRateLimiting": true,
  "StackBlockedRequests": false,
  "GeneralRules": [
    {
      "Endpoint": "POST:/api/login",
      "Period": "1m",
      "Limit": 5
    }
  ]
}
}
```

---

## 🚀 PLAN DE IMPLEMENTACIÓN

### FASE 1: Preparación (Semana 1)
- [ ] Crear nueva solución con estructura Clean Architecture
- [ ] Configurar Entity Framework Code-First
- [ ] Migrar modelos de dominio
- [ ] Configurar fluent configurations

### FASE 2: Seguridad Crítica (Semana 2)
- [ ] Implementar sistema de hashing de contraseñas
- [ ] Eliminar todas las consultas SQL concatenadas
- [ ] Implementar autenticación JWT
- [ ] Configurar autorización basada en roles

### FASE 3: Application Layer (Semana 3)
- [ ] Implementar CQRS con MediatR
- [ ] Crear Commands y Queries
- [ ] Implementar FluentValidation
- [ ] Crear DTOs y Mappers (AutoMapper)

### FASE 4: Infrastructure (Semana 4)
- [ ] Implementar Repository Pattern
- [ ] Configurar Unit of Work
- [ ] Implementar Logging con Serilog
- [ ] Configurar inyección de dependencias

### FASE 5: API Layer (Semana 5)
- [ ] Crear controllers delgados
- [ ] Implementar middleware de excepciones
- [ ] Configurar Rate Limiting
- [ ] Configurar CORS restrictivo
- [ ] Implementar Swagger con autenticación

### FASE 6: Testing (Semana 6)
- [ ] Unit tests (Domain + Application)
- [ ] Integration tests (API)
- [ ] Tests de seguridad
- [ ] Tests de carga

---

## ✅ CHECKLIST DE VALIDACIÓN DE SEGURIDAD

Antes de considerar completa cualquier funcionalidad:

### Input Validation
- [ ] Todos los inputs validados con FluentValidation
- [ ] Longitud máxima definida para strings
- [ ] Tipos de datos correctos
- [ ] Listas blancas para valores enumerados

### Authentication & Authorization
- [ ] JWT implementado correctamente
- [ ] Refresh tokens configurados
- [ ] Todos los endpoints protegidos
- [ ] Roles y políticas aplicadas

### Data Protection
- [ ] Contraseñas hasheadas con BCrypt/Argon2
- [ ] Datos sensibles encriptados en BD
- [ ] Secrets en Azure Key Vault / User Secrets
- [ ] Connection strings protegidas

### SQL Injection Prevention
- [ ] Cero consultas SQL concatenadas
- [ ] Entity Framework usado correctamente
- [ ] Parametrized queries si hay SQL raw

### Error Handling
- [ ] Middleware global implementado
- [ ] Mensajes de error genéricos al cliente
- [ ] Logging detallado server-side
- [ ] Stack traces nunca expuestos

### Logging & Auditing
- [ ] Serilog configurado
- [ ] Logs en base de datos
- [ ] Eventos de seguridad auditados
- [ ] PII no loggeado

### API Security
- [ ] HTTPS obligatorio
- [ ] CORS restrictivo
- [ ] Rate limiting activo
- [ ] Request size limits
- [ ] API versioning

---

## 📚 RECURSOS Y REFERENCIAS

### Documentación Técnica
- [Clean Architecture - Jason Taylor](https://github.com/jasontaylordev/CleanArchitecture)
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [Microsoft Security Best Practices](https://docs.microsoft.com/en-us/aspnet/core/security/)

### NuGet Packages Requeridos
```xml
<!-- Domain Layer -->
<PackageReference Include="FluentValidation" Version="11.9.0" />

<!-- Application Layer -->
<PackageReference Include="MediatR" Version="12.2.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.9.0" />

<!-- Infrastructure Layer -->
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
<PackageReference Include="Serilog.Sinks.MSSqlServer" Version="6.5.0" />

<!-- API Layer -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="AspNetCoreRateLimit" Version="5.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />

<!-- Testing -->
<PackageReference Include="xUnit" Version="2.6.5" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.Testing" Version="8.0.0" />
```

---

## 🎯 CRITERIOS DE ACEPTACIÓN

### Funcionalidad
- ✅ Todas las funcionalidades actuales mantienen su comportamiento
- ✅ Todas las pruebas pasan (mínimo 80% cobertura)
- ✅ Performance igual o mejor que versión actual

### Seguridad
- ✅ CERO vulnerabilidades críticas en scan de seguridad
- ✅ CERO contraseñas en texto plano
- ✅ 100% de endpoints con autenticación/autorización
- ✅ Rate limiting funcionando en endpoints críticos

### Código
- ✅ Arquitectura Clean implementada completamente
- ✅ Code-First migrations funcionando
- ✅ Inyección de dependencias configurada
- ✅ Logging y auditoría completos

### Documentación
- ✅ README actualizado
- ✅ Swagger documentado
- ✅ Diagramas de arquitectura
- ✅ Guía de deployment

---

## 🚨 REGLAS DE ORO

1. **NUNCA** concatenar SQL queries
2. **SIEMPRE** hashear contraseñas
3. **SIEMPRE** validar inputs
4. **NUNCA** exponer stack traces
5. **SIEMPRE** usar HTTPS
6. **SIEMPRE** loggear eventos de seguridad
7. **NUNCA** hardcodear secrets
8. **SIEMPRE** aplicar principio de menor privilegio

---

## 📞 CONTACTO Y SOPORTE

Para dudas sobre implementación:
- Revisar este documento primero
- Consultar documentación de Clean Architecture
- Verificar OWASP guidelines
- Pedir code review antes de merge a main

**Prioridad #1: Seguridad**  
**Prioridad #2: Calidad de código**  
**Prioridad #3: Performance**

---

_Documento actualizado: Diciembre 2024_  
_Basado en Auditoría Técnica: Septiembre 2025_