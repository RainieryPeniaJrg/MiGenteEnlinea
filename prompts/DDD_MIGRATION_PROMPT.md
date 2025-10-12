# 🎯 PROMPT: Migración de Entidades con Enfoque DDD y Auditoría

---

## 📋 CONTEXTO DEL PROYECTO

Estoy trabajando en la **migración de MiGente En Línea** desde ASP.NET Web Forms (Database-First) hacia Clean Architecture con ASP.NET Core 8 (Code-First).

### Estado Actual
- ✅ **36 entidades scaffolded** desde la base de datos `db_a9f8ff_migente`
- ✅ **Clean Architecture estructura creada** con 4 proyectos (Domain, Application, Infrastructure, API)
- ✅ **DbContext generado**: `MiGenteDbContext.cs` con todas las entidades
- ✅ **Entidades generadas**: En `Infrastructure/Persistence/Entities/Generated/`

### Ubicación de Archivos
```
ProyectoMigente/ (WORKSPACE ROOT)
├── .git/                                         # Git repository
├── .github/                                      # GitHub configuration
├── MiGenteEnLinea-Workspace.code-workspace       # VS Code workspace config
│
├── Codigo Fuente Mi Gente/                       # 🔷 LEGACY PROJECT
│   ├── MiGente_Front/
│   │   └── Data/                                 # Entidades EF6 (Database-First)
│   │       ├── Credenciales.cs                   # ⚠️ Passwords en texto plano
│   │       ├── Ofertantes.cs                     # Empleadores (legacy name)
│   │       └── Contratistas.cs
│   └── ...
│
└── MiGenteEnLinea.Clean/                         # 🚀 CLEAN ARCHITECTURE PROJECT
    ├── MiGenteEnLinea.Clean.sln
    ├── src/
    │   ├── Core/
    │   │   ├── MiGenteEnLinea.Domain/            # ✅ Aquí van las entidades refactorizadas
    │   │   │   ├── Entities/                     # Entidades DDD refactorizadas
    │   │   │   │   ├── Authentication/
    │   │   │   │   │   └── Credencial.cs         # ✅ A crear (refactorizada)
    │   │   │   │   ├── Empleadores/
    │   │   │   │   │   └── Empleador.cs          # ✅ A crear (refactorizada)
    │   │   │   │   └── Contratistas/
    │   │   │   │       └── Contratista.cs        # ✅ A crear (refactorizada)
    │   │   │   ├── ValueObjects/                 # Value Objects inmutables
    │   │   │   ├── Common/                       # Clases base (AuditableEntity, etc.)
    │   │   │   ├── Events/                       # Domain Events
    │   │   │   └── Interfaces/                   # Interfaces del dominio
    │   │   │
    │   │   └── MiGenteEnLinea.Application/       # Use Cases, DTOs, Validators
    │   │       ├── Features/
    │   │       │   ├── Authentication/
    │   │       │   ├── Empleadores/
    │   │       │   └── Contratistas/
    │   │       └── Common/
    │   │
    │   ├── Infrastructure/
    │   │   └── MiGenteEnLinea.Infrastructure/
    │   │       ├── Persistence/
    │   │       │   ├── Contexts/
    │   │       │   │   └── MiGenteDbContext.cs   # DbContext generado
    │   │       │   ├── Entities/Generated/       # 36 entidades scaffolded
    │   │       │   │   ├── Credenciale.cs        # ⚠️ Scaffolded (a refactorizar)
    │   │       │   │   ├── Ofertante.cs          # ⚠️ Scaffolded (a refactorizar)
    │   │       │   │   └── Contratista.cs        # ⚠️ Scaffolded (a refactorizar)
    │   │       │   ├── Configurations/           # ✅ Aquí van las Fluent API configs
    │   │       │   │   └── CredencialConfiguration.cs  # ✅ A crear
    │   │       │   ├── Interceptors/             # Interceptors de EF Core
    │   │       │   │   └── AuditableEntityInterceptor.cs  # ✅ A crear
    │   │       │   └── Repositories/             # Implementación de repositorios
    │   │       │
    │   │       ├── Identity/                     # Servicios de identidad
    │   │       │   └── Services/
    │   │       │       └── BCryptPasswordHasher.cs  # ✅ A crear
    │   │       │
    │   │       └── Services/                     # Servicios externos
    │   │
    │   └── Presentation/
    │       └── MiGenteEnLinea.API/               # API REST
    │           └── Controllers/
    │
    └── tests/                                     # Tests (futura implementación)
```

---

## 🎯 OBJETIVO DE ESTA SESIÓN

Quiero que me ayudes a **refactorizar las entidades scaffolded** siguiendo principios de **Domain-Driven Design (DDD)** y **mejores prácticas de Clean Architecture**.

### Prioridad de Entidades a Migrar

#### 🔥 PRIORIDAD 1 - CRÍTICA (Esta sesión)
1. **Credencial** (`Credenciale.cs`) - Autenticación con passwords en texto plano ⚠️
2. **Usuario** (si existe separado de Credencial)
3. **Empleador** (`Ofertante.cs`) - Core business entity
4. **Contratista** (`Contratista.cs`) - Core business entity

#### ⚠️ PRIORIDAD 2 - ALTA (Próxima sesión)
5. **Empleado** (`Empleado.cs`) - Gestión de empleados
6. **Suscripcion** (`Suscripcione.cs`) - Revenue critical
7. **Plan** (`PlanesEmpleadore.cs`, `PlanesContratista.cs`) - Subscription plans

#### 📊 PRIORIDAD 3 - MEDIA (Sprints futuros)
8. Resto de entidades transaccionales (Nómina, Contratos, Calificaciones, etc.)
9. Entidades de catálogo (Servicios, Deducciones, etc.)
10. Views (entidades de solo lectura con prefijo `V`)

---

## 📐 PATRONES Y PRINCIPIOS A APLICAR

### 1. 🏛️ Domain-Driven Design (DDD)

#### A. Rich Domain Model (NO Anemic Model)
```csharp
// ❌ EVITAR: Anemic Model (solo getters/setters)
public class Credencial
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool Activo { get; set; }
}

// ✅ APLICAR: Rich Domain Model (lógica de negocio en la entidad)
public class Credencial : AuditableEntity
{
    // Private constructor para EF Core
    private Credencial() { }

    // Static factory method
    public static Credencial Create(string userId, string email, string passwordHash)
    {
        var credencial = new Credencial();
        credencial.UserId = userId;
        credencial.SetEmail(email);
        credencial.PasswordHash = passwordHash;
        credencial.Activo = false; // Requiere activación
        return credencial;
    }

    // Propiedades con setters privados (encapsulación)
    public int Id { get; private set; }
    public string UserId { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool Activo { get; private set; }
    public DateTime? FechaActivacion { get; private set; }
    public DateTime? UltimoAcceso { get; private set; }

    // Domain methods (comportamiento)
    public void Activar()
    {
        if (Activo)
            throw new InvalidOperationException("La credencial ya está activa");
            
        Activo = true;
        FechaActivacion = DateTime.UtcNow;
    }

    public void Desactivar()
    {
        if (!Activo)
            throw new InvalidOperationException("La credencial ya está inactiva");
            
        Activo = false;
    }

    public void ActualizarPasswordHash(string nuevoPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(nuevoPasswordHash))
            throw new ArgumentException("El hash de password no puede estar vacío");
            
        PasswordHash = nuevoPasswordHash;
    }

    public void RegistrarAcceso()
    {
        if (!Activo)
            throw new InvalidOperationException("No se puede registrar acceso en credencial inactiva");
            
        UltimoAcceso = DateTime.UtcNow;
    }

    private void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío");
            
        if (!email.Contains("@"))
            throw new ArgumentException("El email no es válido");
            
        Email = email.ToLowerInvariant();
    }
}
```

#### B. Value Objects (para conceptos sin identidad)
```csharp
// Ejemplo: Email como Value Object
public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El email no puede estar vacío");
            
        if (!value.Contains("@") || !value.Contains("."))
            throw new ArgumentException("El email no es válido");
            
        return new Email(value.ToLowerInvariant().Trim());
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

// Base class para Value Objects
public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != GetType())
            return false;

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
}
```

#### C. Domain Events (para comunicación entre agregados)
```csharp
// Base class para eventos de dominio
public abstract class DomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

// Ejemplo: Evento cuando se activa una credencial
public sealed class CredencialActivadaEvent : DomainEvent
{
    public int CredencialId { get; }
    public string Email { get; }

    public CredencialActivadaEvent(int credencialId, string email)
    {
        CredencialId = credencialId;
        Email = email;
    }
}

// Entidad con soporte para eventos
public abstract class AggregateRoot : AuditableEntity
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
```

---

### 2. 📝 Auditable Entity Pattern

#### A. Base Class para Auditoría
```csharp
namespace MiGenteEnLinea.Domain.Common;

/// <summary>
/// Clase base para todas las entidades que requieren auditoría.
/// Rastrea quién y cuándo creó/modificó cada registro.
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// Fecha y hora UTC de creación del registro
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// ID del usuario que creó el registro
    /// </summary>
    public string CreatedBy { get; set; }

    /// <summary>
    /// Fecha y hora UTC de última modificación del registro
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// ID del usuario que modificó el registro por última vez
    /// </summary>
    public string? UpdatedBy { get; set; }
}
```

#### B. Soft Delete Support
```csharp
namespace MiGenteEnLinea.Domain.Common;

/// <summary>
/// Clase base para entidades que soportan eliminación lógica (soft delete)
/// </summary>
public abstract class SoftDeletableEntity : AuditableEntity
{
    /// <summary>
    /// Indica si el registro está eliminado lógicamente
    /// </summary>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// Fecha y hora UTC de eliminación lógica
    /// </summary>
    public DateTime? DeletedAt { get; private set; }

    /// <summary>
    /// ID del usuario que eliminó el registro
    /// </summary>
    public string? DeletedBy { get; private set; }

    /// <summary>
    /// Marca el registro como eliminado lógicamente
    /// </summary>
    public void SoftDelete(string deletedBy)
    {
        if (IsDeleted)
            throw new InvalidOperationException("El registro ya está eliminado");

        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = deletedBy;
    }

    /// <summary>
    /// Restaura un registro eliminado lógicamente
    /// </summary>
    public void Restore()
    {
        if (!IsDeleted)
            throw new InvalidOperationException("El registro no está eliminado");

        IsDeleted = false;
        DeletedAt = null;
        DeletedBy = null;
    }
}
```

#### C. Interceptor para Auditoría Automática
```csharp
namespace MiGenteEnLinea.Infrastructure.Persistence.Interceptors;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MiGenteEnLinea.Domain.Common;

/// <summary>
/// Interceptor que actualiza automáticamente los campos de auditoría
/// </summary>
public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;

    public AuditableEntityInterceptor(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context == null) return;

        var userId = _currentUserService.UserId ?? "System";
        var utcNow = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = userId;
                entry.Entity.CreatedAt = utcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedBy = userId;
                entry.Entity.UpdatedAt = utcNow;
            }
        }

        // Soft deletes
        foreach (var entry in context.ChangeTracker.Entries<SoftDeletableEntity>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.SoftDelete(userId);
            }
        }
    }
}

/// <summary>
/// Servicio para obtener el usuario actual
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}
```

---

### 3. 🛠️ Fluent API Configuration

#### A. Configuración Base
```csharp
namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Common;

/// <summary>
/// Configuración base para entidades auditables
/// </summary>
public abstract class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : AuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        // Configuración de auditoría
        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(450)
            .HasColumnName("created_by");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.UpdatedBy)
            .HasMaxLength(450)
            .HasColumnName("updated_by");

        // Índices para consultas de auditoría
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.CreatedBy);
    }
}
```

#### B. Configuración de Entidad Específica
```csharp
namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Authentication;

/// <summary>
/// Configuración de Fluent API para la entidad Credencial
/// </summary>
public sealed class CredencialConfiguration : AuditableEntityConfiguration<Credencial>
{
    public override void Configure(EntityTypeBuilder<Credencial> builder)
    {
        // Configuración base de auditoría
        base.Configure(builder);

        // Tabla
        builder.ToTable("Credenciales");

        // Primary Key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        // UserId
        builder.Property(c => c.UserId)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("userID")
            .IsUnicode(false);

        builder.HasIndex(c => c.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Credenciales_UserId");

        // Email
        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("email")
            .IsUnicode(false);

        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Credenciales_Email");

        // PasswordHash (antes era 'password' en texto plano)
        builder.Property(c => c.PasswordHash)
            .IsRequired()
            .HasColumnName("password") // Mismo nombre de columna en DB
            .IsUnicode(false);

        // Activo
        builder.Property(c => c.Activo)
            .IsRequired()
            .HasColumnName("activo")
            .HasDefaultValue(false);

        // FechaActivacion
        builder.Property(c => c.FechaActivacion)
            .HasColumnName("fecha_activacion");

        // UltimoAcceso
        builder.Property(c => c.UltimoAcceso)
            .HasColumnName("ultimo_acceso");

        // Índice compuesto para búsquedas
        builder.HasIndex(c => new { c.Email, c.Activo })
            .HasDatabaseName("IX_Credenciales_Email_Activo");

        // Query filter para soft deletes (si se implementa)
        // builder.HasQueryFilter(c => !c.IsDeleted);
    }
}
```

---

### 4. 🔐 Seguridad y Validación

#### A. BCrypt Password Hasher
```csharp
namespace MiGenteEnLinea.Infrastructure.Services;

using BCrypt.Net;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

public sealed class BCryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty", nameof(password));

        return BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
            return false;

        try
        {
            return BCrypt.Verify(password, hashedPassword);
        }
        catch
        {
            return false;
        }
    }
}
```

#### B. FluentValidation para Commands
```csharp
namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Register;

using FluentValidation;

public sealed class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    public RegistrarUsuarioCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .EmailAddress().WithMessage("El email no es válido")
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$")
            .WithMessage("La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .MaximumLength(50)
            .Matches("^[a-zA-Z0-9_-]+$")
            .WithMessage("El userId solo puede contener letras, números, guiones y guiones bajos");
    }
}
```

---

## 🚀 TAREAS ESPECÍFICAS QUE NECESITO

### ✅ Tarea 1: Refactorizar Entidad Credencial

**NOTA:** Ya existe la solución creada con el script `setup-codefirst-migration.ps1` y las 36 entidades scaffolded en `Infrastructure/Persistence/Entities/Generated/`.

1. **Crear clases base en Domain/Common/**:
   - [ ] `AuditableEntity.cs` - Base para auditoría (CreatedAt, CreatedBy, UpdatedAt, UpdatedBy)
   - [ ] `SoftDeletableEntity.cs` - Base para soft delete (IsDeleted, DeletedAt, DeletedBy)
   - [ ] `AggregateRoot.cs` - Base para agregados (Domain Events)
   - [ ] `ValueObject.cs` - Base para value objects

2. **Implementar Credencial con DDD**:
   - [ ] Copiar `Generated/Credenciale.cs` a `Domain/Entities/Authentication/Credencial.cs`
   - [ ] Heredar de `AuditableEntity`
   - [ ] Encapsular propiedades (setters privados)
   - [ ] Crear constructor privado para EF Core
   - [ ] Crear factory method estático `Create()` para instanciación
   - [ ] Agregar domain methods: `Activar()`, `Desactivar()`, `ActualizarPasswordHash()`, `RegistrarAcceso()`
   - [ ] Renombrar propiedad `Password` a `PasswordHash`
   - [ ] Agregar propiedades: `FechaActivacion`, `UltimoAcceso`
   - [ ] Implementar validaciones de negocio en los métodos

3. **Crear Fluent API Configuration**:
   - [ ] Crear `CredencialConfiguration.cs` en `Infrastructure/Persistence/Configurations/`
   - [ ] Heredar de `IEntityTypeConfiguration<Credencial>`
   - [ ] Mapear a tabla `Credenciales` existente en DB
   - [ ] Configurar columnas con nombres legacy (mantener compatibilidad):
     - `Id` → columna `id`
     - `UserId` → columna `userID`
     - `Email` → columna `email`
     - `PasswordHash` → columna `password` (mismo nombre en DB)
     - `Activo` → columna `activo`
   - [ ] Agregar índices únicos en `UserId` y `Email`
   - [ ] Configurar columnas de auditoría (created_at, created_by, etc.)

4. **Implementar BCrypt Password Hasher**:
   - [ ] Crear `IPasswordHasher` en `Domain/Interfaces/`
   - [ ] Implementar `BCryptPasswordHasher` en `Infrastructure/Services/`
   - [ ] Registrar servicio en `Infrastructure/DependencyInjection.cs`

5. **Crear Interceptor de Auditoría**:
   - [ ] Crear `AuditableEntityInterceptor.cs` en `Infrastructure/Persistence/Interceptors/`
   - [ ] Implementar `ICurrentUserService` en `Infrastructure/Identity/`
   - [ ] Registrar interceptor en `MiGenteDbContext`

6. **Actualizar DbContext**:
   - [ ] Registrar `CredencialConfiguration` en `OnModelCreating()`
   - [ ] Aplicar interceptor de auditoría
   - [ ] Configurar connection string en `appsettings.json`

---

### ✅ Tarea 2: Refactorizar Entidades Core (Empleador, Contratista)

Seguir el mismo patrón para:

1. **Empleador** (mapea a tabla `Ofertantes`):
   - Heredar de `SoftDeletableEntity` (puede ser eliminado lógicamente)
   - Propiedades: RNC, Nombre, Email, Telefono, Direccion, PlanId, etc.
   - Domain methods: `CambiarPlan()`, `SuspenderCuenta()`, `ActivarCuenta()`
   - Relación con `Empleado` (uno a muchos)
   - Relación con `Suscripcion` (uno a muchos)

2. **Contratista** (mapea a tabla `Contratistas`):
   - Heredar de `SoftDeletableEntity`
   - Propiedades: Cedula, Nombre, Email, Telefono, Servicios, PlanId, etc.
   - Domain methods: `AgregarServicio()`, `EliminarServicio()`, `ActualizarPerfil()`
   - Relación con `Calificacion` (uno a muchos)
   - Relación con `Suscripcion` (uno a muchos)

---

### ✅ Tarea 3: Implementar Auditoría Automática

1. **Crear AuditableEntityInterceptor**:
   - [ ] Crear `Interceptors/AuditableEntityInterceptor.cs` en Infrastructure
   - [ ] Inyectar `ICurrentUserService` para obtener usuario actual
   - [ ] Implementar lógica de actualización automática de campos de auditoría
   - [ ] Registrar interceptor en `DbContext`

2. **Implementar ICurrentUserService**:
   - [ ] Crear interface en Application/Common/Interfaces
   - [ ] Implementar en Infrastructure/Identity/ usando `IHttpContextAccessor`
   - [ ] Extraer UserId del JWT token (cuando se implemente auth)
   - [ ] Por ahora, retornar "System" como default
   - [ ] Registrar servicio en DependencyInjection

3. **Configurar DbContext**:
   - [ ] Agregar interceptor en `OnConfiguring()` o en DI
   - [ ] Configurar global query filters para soft deletes

---

### ✅ Tarea 4: Crear Controllers para Authentication

1. **Crear AuthController en API**:
   - [ ] Crear `Controllers/AuthController.cs`
   - [ ] Endpoint `POST /api/auth/register` - Registrar nuevo usuario
   - [ ] Endpoint `POST /api/auth/login` - Iniciar sesión
   - [ ] Endpoint `POST /api/auth/activate` - Activar cuenta
   - [ ] Endpoint `POST /api/auth/change-password` - Cambiar contraseña

2. **Crear Commands en Application**:
   - [ ] `Features/Authentication/Commands/Register/RegistrarUsuarioCommand.cs`
   - [ ] `Features/Authentication/Commands/Login/LoginCommand.cs`
   - [ ] `Features/Authentication/Commands/Activate/ActivarCuentaCommand.cs`
   - [ ] Handlers correspondientes para cada command

3. **Crear DTOs**:
   - [ ] `Features/Authentication/DTOs/UsuarioDto.cs`
   - [ ] `Features/Authentication/DTOs/CredencialDto.cs`
   - [ ] `Features/Authentication/DTOs/LoginResultDto.cs`

4. **FluentValidation**:
   - [ ] Validators para cada Command
   - [ ] Reglas de validación (email, password complexity, etc.)

---

### ✅ Tarea 5: Crear Migraciones

1. **Agregar columnas de auditoría**:
   ```powershell
   dotnet ef migrations add AddAuditableFields `
       --startup-project ../../Presentation/MiGenteEnLinea.API `
       --context MiGenteDbContext `
       --output-dir Persistence/Migrations
   ```

2. **Revisar migración generada**:
   - Verificar que solo agrega columnas nuevas (created_at, created_by, etc.)
   - NO debe crear tablas (ya existen)
   - NO debe modificar password (migración de passwords es aparte)

3. **Aplicar migración**:
   ```powershell
   dotnet ef database update --startup-project ../../Presentation/MiGenteEnLinea.API
   ```

---

## 📝 CHECKLIST DE VALIDACIÓN

Cuando termines cada entidad, verifica:

### ✅ Clean Code
- [ ] Nombres en español (dominio de negocio dominicano)
- [ ] Métodos descriptivos (verbos de acción)
- [ ] Sin magic numbers o strings
- [ ] Sin código comentado
- [ ] XML documentation en clases públicas

### ✅ DDD Principles
- [ ] Entidad es un Aggregate Root o parte de uno
- [ ] Lógica de negocio en la entidad (no anemic model)
- [ ] Validaciones en la entidad, no en el setter
- [ ] Factory methods para creación compleja
- [ ] Domain events para comunicación entre agregados

### ✅ Auditoría
- [ ] Hereda de `AuditableEntity` o `SoftDeletableEntity`
- [ ] Campos de auditoría configurados en Fluent API
- [ ] Interceptor registrado y funcionando

### ✅ Seguridad
- [ ] Passwords hasheados con BCrypt (nunca texto plano)
- [ ] Validación de inputs con FluentValidation
- [ ] Encapsulación correcta (setters privados)
- [ ] Sin SQL injection (usar LINQ/EF Core)

### ✅ Performance
- [ ] Índices definidos en Fluent API
- [ ] Query filters para soft deletes
- [ ] Lazy loading deshabilitado
- [ ] Eager loading solo cuando necesario

---

## 🎓 GUÍAS DE REFERENCIA

### Convenciones de Nombres

#### Español para Dominio
```csharp
// ✅ CORRECTO (dominio de negocio)
public class Empleador
{
    public void CambiarPlan(int nuevoPlanId) { }
    public void SuspenderCuenta() { }
}

// ❌ INCORRECTO (inglés en dominio)
public class Employer
{
    public void ChangePlan(int newPlanId) { }
}
```

#### Inglés para Infraestructura
```csharp
// ✅ CORRECTO
public interface IRepository<T> { }
public class AuditableEntity { }

// Excepción: términos técnicos universales
public class CreatedAt { get; set; }
public string UpdatedBy { get; set; }
```

### Estructura de Archivos por Feature

```
Application/Features/Authentication/
├── Commands/
│   ├── Register/
│   │   ├── RegistrarUsuarioCommand.cs
│   │   ├── RegistrarUsuarioCommandHandler.cs
│   │   └── RegistrarUsuarioCommandValidator.cs
│   └── Login/
│       ├── LoginCommand.cs
│       ├── LoginCommandHandler.cs
│       └── LoginCommandValidator.cs
├── Queries/
│   └── GetUsuario/
│       ├── GetUsuarioQuery.cs
│       └── GetUsuarioQueryHandler.cs
└── DTOs/
    ├── UsuarioDto.cs
    └── CredencialDto.cs
```

---

## 🚧 RESTRICCIONES Y CONSIDERACIONES

### ⚠️ NO Hacer (Breaking Changes)

1. **NO renombrar tablas existentes** (usar Fluent API para mapear):
   ```csharp
   // ✅ CORRECTO
   builder.ToTable("Credenciales"); // Tabla existente en DB
   
   // ❌ INCORRECTO - cambiaría nombre de tabla
   builder.ToTable("Credentials");
   ```

2. **NO eliminar columnas** que usa el legacy (mientras ambos sistemas corran):
   ```csharp
   // Mantener columna 'password' aunque se renombre propiedad a 'PasswordHash'
   builder.Property(c => c.PasswordHash)
       .HasColumnName("password"); // Mismo nombre en DB
   ```

3. **NO cambiar tipos de datos** sin migración de datos:
   ```csharp
   // Si en DB es NVARCHAR(50), mantener
   builder.Property(c => c.UserId)
       .HasMaxLength(50)
       .IsUnicode(false);
   ```

### ✅ SÍ Hacer (Safe Changes)

1. **Agregar columnas nuevas** (nullable o con default):
   ```csharp
   builder.Property(e => e.CreatedAt)
       .IsRequired()
       .HasDefaultValueSql("GETUTCDATE()");
   ```

2. **Agregar índices** para mejorar performance:
   ```csharp
   builder.HasIndex(c => c.Email).IsUnique();
   builder.HasIndex(c => new { c.Email, c.Activo });
   ```

3. **Agregar constraints** que no rompan datos existentes:
   ```csharp
   builder.Property(c => c.Email)
       .IsRequired() // Solo si ya no hay nulls en DB
       .HasMaxLength(100);
   ```

---

## 💡 EJEMPLOS DE CONSULTAS LINQ

### Consultas Comunes

```csharp
// Buscar credencial activa por email
var credencial = await _context.Credenciales
    .Where(c => c.Email == email && c.Activo)
    .FirstOrDefaultAsync();

// Buscar empleadores con plan activo
var empleadores = await _context.Empleadores
    .Include(e => e.Suscripcion)
    .Where(e => !e.IsDeleted && e.Suscripcion.FechaVencimiento > DateTime.UtcNow)
    .OrderByDescending(e => e.CreatedAt)
    .ToListAsync();

// Contar contratistas por servicio
var contratosPorServicio = await _context.Contratistas
    .Where(c => !c.IsDeleted)
    .GroupBy(c => c.ServicioId)
    .Select(g => new { ServicioId = g.Key, Count = g.Count() })
    .ToListAsync();

// Auditoría: últimas modificaciones
var ultimosRegistros = await _context.Credenciales
    .OrderByDescending(c => c.UpdatedAt ?? c.CreatedAt)
    .Take(10)
    .Select(c => new {
        c.Email,
        c.UpdatedBy,
        FechaModificacion = c.UpdatedAt ?? c.CreatedAt
    })
    .ToListAsync();
```

---

## 🎯 RESULTADO ESPERADO

Al final de esta sesión, deberíamos tener:

1. ✅ **Entidad Credencial refactorizada** con DDD y auditoría
2. ✅ **Fluent API configuration** completa para Credencial
3. ✅ **BCrypt password hasher** implementado
4. ✅ **Controllers de Authentication** con endpoints REST
5. ✅ **Commands y Handlers** con CQRS pattern
6. ✅ **FluentValidation** para todos los inputs
7. ✅ **Migración de EF Core** generada y aplicada
8. ✅ **Auditoría automática** funcionando con interceptor

**Bonus:** Si hay tiempo, empezar con Empleador y Contratista siguiendo el mismo patrón.

**NOTA:** Los tests unitarios y de integración se agregarán en una fase posterior. Por ahora nos enfocamos en la migración de entidades, lógica de negocio y controllers.

---

## 📚 RECURSOS ADICIONALES

- **DDD Reference**: <https://www.domainlanguage.com/ddd/reference/>
- **Clean Architecture**: <https://github.com/jasontaylordev/CleanArchitecture>
- **EF Core Best Practices**: <https://docs.microsoft.com/en-us/ef/core/performance/>
- **FluentValidation**: <https://docs.fluentvalidation.net/>
- **CQRS Pattern**: <https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs>

---

## 🤔 PREGUNTAS PARA GUIAR LA IMPLEMENTACIÓN

1. **¿Esta entidad debería heredar de `AuditableEntity` o `SoftDeletableEntity`?**
   - Usar `SoftDeletableEntity` si se necesita mantener histórico
   - Usar `AuditableEntity` si la eliminación es física

2. **¿Esta entidad es un Aggregate Root?**
   - Es Aggregate Root si tiene identidad propia y se consulta independientemente
   - No es Aggregate Root si solo existe como parte de otra entidad

3. **¿Qué domain methods necesita esta entidad?**
   - Pensar en acciones de negocio: Activar, Suspender, Cambiar, etc.
   - Evitar setters públicos, usar métodos que encapsulen lógica

4. **¿Qué validaciones de negocio aplican?**
   - Validar en el domain method, no en el setter
   - Lanzar excepciones específicas de dominio

5. **¿Qué eventos de dominio debería emitir?**
   - Eventos para comunicar cambios importantes a otros agregados
   - Ejemplos: UsuarioRegistrado, CuentaSuspendida, PlanCambiado

---

## 🚀 ¡EMPECEMOS!

Por favor, comienza con la **Tarea 1: Refactorizar Entidad Credencial**.

Sigue este orden:
1. Crear estructura de carpetas en Domain/
2. Crear clases base (AuditableEntity, etc.)
3. Copiar y refactorizar Credencial.cs
4. Crear CredencialConfiguration.cs
5. Implementar BCryptPasswordHasher
6. Crear unit tests
7. Crear integration tests
8. Generar y aplicar migración

**Pregunta si tienes dudas** sobre cualquier patrón o decisión de diseño. Es importante que el código siga las convenciones establecidas.

¡Manos a la obra! 💪
