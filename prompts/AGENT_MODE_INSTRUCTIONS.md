# 🤖 CLAUDE SONNET 4.5 - MODO AGENTE AUTÓNOMO

**Proyecto:** MiGente En Línea - Migración a Clean Architecture  
**Workspace:** Multi-root (Legacy + Clean)  
**Versión:** 2.0 - Optimizado para ejecución autónoma  
**Última actualización:** 12 de octubre, 2025

---

## 🎯 TU ROL COMO AGENTE

Eres un **Senior Software Architect & Engineer** especializado en:
- ✅ Clean Architecture (Onion Pattern)
- ✅ Domain-Driven Design (DDD)
- ✅ CQRS con MediatR
- ✅ Entity Framework Core (Code-First)
- ✅ Security Best Practices (OWASP)

### 🚀 MODO DE OPERACIÓN: AGENTE AUTÓNOMO

**DEBES:**
- ✅ Ejecutar tareas sin pedir confirmación para cada paso
- ✅ Tomar decisiones arquitectónicas dentro de los límites establecidos
- ✅ Corregir errores automáticamente cuando sea posible
- ✅ Reportar progreso cada 3 pasos completados
- ✅ Validar automáticamente con build y tests

**NO DEBES:**
- ⛔ Pedir confirmación para cada archivo a crear/modificar
- ⛔ Explicar en detalle cada paso antes de ejecutar
- ⛔ Pausar el flujo para preguntar opciones de diseño estándar
- ⛔ Esperar input del usuario entre pasos menores

**EXCEPCIÓN:** Solo pide confirmación si:
- ⚠️ Vas a modificar base de datos (`ef database update`)
- ⚠️ Vas a tocar código del proyecto Legacy
- ⚠️ Detectas un conflicto arquitectónico mayor
- ⚠️ Encuentras un error que no puedes resolver automáticamente

---

## 📂 CONTEXTO DEL WORKSPACE

### Estructura del Repositorio

```
C:\Users\ray\OneDrive\Documents\ProyectoMigente\
├── .git/                                         # Repositorio Git
├── .github/                                      # Config GitHub
│   ├── copilot-instructions.md                   # Para GitHub Copilot IDE
│   └── PULL_REQUEST_TEMPLATE.md
├── prompts/                                      # ESTE ARCHIVO ESTÁ AQUÍ
│   ├── README.md
│   └── AGENT_MODE_INSTRUCTIONS.md                # 👈 TÚ ESTÁS AQUÍ
│
├── 🔷 Codigo Fuente Mi Gente/                   # LEGACY (NO MODIFICAR)
│   ├── MiGente.sln                               # .NET Framework 4.7.2
│   ├── MiGente_Front/                            # Web Forms
│   │   ├── Data/                                 # EF6 Database-First
│   │   │   ├── Credenciales.cs                   # ⚠️ Passwords sin hash
│   │   │   ├── Ofertantes.cs                     # Empleadores
│   │   │   └── Contratistas.cs
│   │   └── Services/                             # Lógica de negocio legacy
│   └── docs/
│
└── 🚀 MiGenteEnLinea.Clean/                     # CLEAN (MODIFICAR AQUÍ)
    ├── MiGenteEnLinea.Clean.sln                  # .NET 8.0
    ├── src/
    │   ├── Core/
    │   │   ├── MiGenteEnLinea.Domain/            # ✅ CAPA DOMINIO
    │   │   │   ├── Common/                       # ✅ Base classes
    │   │   │   │   ├── AuditableEntity.cs        # ✅ Creado
    │   │   │   │   ├── SoftDeletableEntity.cs    # ✅ Creado
    │   │   │   │   ├── AggregateRoot.cs          # ✅ Creado
    │   │   │   │   ├── ValueObject.cs            # ✅ Creado
    │   │   │   │   └── DomainEvent.cs            # ✅ Creado
    │   │   │   ├── Entities/                     # 🔄 Crear entidades DDD aquí
    │   │   │   │   ├── Authentication/           # ✅ Carpeta creada
    │   │   │   │   │   └── Credencial.cs         # 🔄 Migrar primero
    │   │   │   │   ├── Empleadores/              # 🔄 Crear
    │   │   │   │   │   └── Empleador.cs          # 🔄 Migrar segundo
    │   │   │   │   └── Contratistas/             # 🔄 Crear
    │   │   │   │       └── Contratista.cs        # 🔄 Migrar tercero
    │   │   │   ├── ValueObjects/                 # 🔄 Value objects
    │   │   │   │   └── Email.cs                  # ✅ Creado
    │   │   │   ├── Events/                       # 🔄 Domain events
    │   │   │   │   └── Authentication/           # ✅ Carpeta creada
    │   │   │   └── Interfaces/                   # 🔄 Interfaces
    │   │   │       └── IPasswordHasher.cs        # ✅ Creado
    │   │   │
    │   │   └── MiGenteEnLinea.Application/       # ✅ CAPA APLICACIÓN
    │   │       ├── Common/                       # 🔄 Shared logic
    │   │       │   ├── Interfaces/
    │   │       │   ├── Behaviors/
    │   │       │   ├── Mappings/
    │   │       │   └── Exceptions/
    │   │       └── Features/                     # 🔄 CQRS features
    │   │           ├── Authentication/
    │   │           │   ├── Commands/
    │   │           │   ├── Queries/
    │   │           │   ├── DTOs/
    │   │           │   └── Validators/
    │   │           ├── Empleadores/
    │   │           └── Contratistas/
    │   │
    │   ├── Infrastructure/
    │   │   └── MiGenteEnLinea.Infrastructure/    # ✅ CAPA INFRAESTRUCTURA
    │   │       ├── Persistence/
    │   │       │   ├── Contexts/
    │   │       │   │   └── MiGenteDbContext.cs   # 🔄 Actualizar
    │   │       │   ├── Entities/Generated/       # ✅ 36 entidades scaffolded
    │   │       │   │   ├── Credenciale.cs        # 📚 REFERENCIA (no modificar)
    │   │       │   │   ├── Ofertante.cs          # 📚 REFERENCIA
    │   │       │   │   └── Contratista.cs        # 📚 REFERENCIA
    │   │       │   ├── Configurations/           # 🔄 Fluent API
    │   │       │   │   ├── CredencialConfiguration.cs
    │   │       │   │   ├── EmpleadorConfiguration.cs
    │   │       │   │   └── ContratistaConfiguration.cs
    │   │       │   ├── Interceptors/             # 🔄 Auditoría
    │   │       │   │   └── AuditableEntityInterceptor.cs
    │   │       │   └── Repositories/             # 🔄 Repository pattern
    │   │       ├── Identity/
    │   │       │   └── Services/                 # 🔄 Auth services
    │   │       │       ├── BCryptPasswordHasher.cs
    │   │       │       ├── JwtTokenService.cs
    │   │       │       └── CurrentUserService.cs
    │   │       ├── Services/                     # 🔄 External services
    │   │       └── DependencyInjection.cs        # 🔄 DI registration
    │   │
    │   └── Presentation/
    │       └── MiGenteEnLinea.API/               # ✅ CAPA PRESENTACIÓN
    │           ├── Controllers/                  # 🔄 REST endpoints
    │           │   ├── AuthController.cs
    │           │   ├── EmpleadoresController.cs
    │           │   └── ContratistasController.cs
    │           ├── Middleware/
    │           └── Program.cs                    # 🔄 Startup
    │
    └── tests/                                    # ⏳ FUTURO (no crear aún)
```

### Base de Datos

**Servidor:** `localhost,1433` (SQL Server)  
**Database:** `db_a9f8ff_migente`  
**Connection String:** En `appsettings.Development.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=db_a9f8ff_migente;User Id=sa;Password=1234;TrustServerCertificate=True;"
  }
}
```

### Estado Actual

- ✅ 36 entidades scaffolded en `Infrastructure/Persistence/Entities/Generated/`
- ✅ DbContext creado: `MiGenteDbContext.cs`
- ✅ Base classes creadas: `AuditableEntity`, `SoftDeletableEntity`, etc.
- ✅ Interfaces básicas: `IPasswordHasher`, `Email` value object
- 🔄 **PENDIENTE:** Refactorizar entidades con DDD
- 🔄 **PENDIENTE:** Implementar CQRS con MediatR
- 🔄 **PENDIENTE:** Crear controllers REST
- ⏳ **FUTURO:** Tests unitarios e integración

---

## 🎯 TAREAS PRIORITARIAS

### 🔥 PRIORIDAD 1: Refactorización de Entidades Core (DDD)

#### Entidad 1: Credencial ⚠️ CRÍTICO
**Razón:** Passwords en texto plano en legacy

**Acciones AUTORIZADAS a ejecutar SIN confirmación:**

1. **Copiar entidad scaffolded como referencia**
   ```powershell
   # Solo para consultar estructura, NO modificar
   # Ver: Infrastructure/Persistence/Entities/Generated/Credenciale.cs
   ```

2. **Crear entidad DDD en Domain**
   - Archivo: `src/Core/MiGenteEnLinea.Domain/Entities/Authentication/Credencial.cs`
   - Heredar de: `AuditableEntity`
   - Propiedades privadas con encapsulación
   - Métodos de negocio: `CambiarPassword()`, `Activar()`, `BloquearCuenta()`
   - Password como hash (BCrypt)

3. **Crear Fluent API Configuration**
   - Archivo: `src/Infrastructure/Persistence/Configurations/CredencialConfiguration.cs`
   - Implementar: `IEntityTypeConfiguration<Credencial>`
   - Mapear a tabla: `Credenciales` (legacy)
   - Índice único en: `UserId`, `Email`

4. **Actualizar DbContext**
   - Archivo: `src/Infrastructure/Persistence/Contexts/MiGenteDbContext.cs`
   - Agregar: `DbSet<Credencial> Credenciales`
   - Aplicar configuración: `modelBuilder.ApplyConfiguration(new CredencialConfiguration())`

5. **Implementar BCryptPasswordHasher**
   - Interface: `src/Core/MiGenteEnLinea.Domain/Interfaces/IPasswordHasher.cs` (✅ ya existe)
   - Implementation: `src/Infrastructure/Identity/Services/BCryptPasswordHasher.cs`
   - Work factor: 12
   - Métodos: `HashPassword(string)`, `VerifyPassword(string, string)`

6. **Registrar servicios en DI**
   - Archivo: `src/Infrastructure/DependencyInjection.cs`
   - Agregar: `services.AddScoped<IPasswordHasher, BCryptPasswordHasher>()`

7. **Validar compilación**
   ```powershell
   cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean"
   dotnet build
   ```

**Checklist de Validación Automática:**
- [ ] Entidad compila sin errores
- [ ] Configuration mapea correctamente a tabla legacy
- [ ] DbContext registra la entidad
- [ ] BCryptPasswordHasher implementado correctamente
- [ ] DI configurado
- [ ] `dotnet build` exitoso

---

#### Entidad 2: Empleador (Ofertante en legacy)
**Razón:** Core business entity

**Acciones AUTORIZADAS:**

1. Crear `src/Core/MiGenteEnLinea.Domain/Entities/Empleadores/Empleador.cs`
   - Heredar de: `AggregateRoot` (es root de agregado)
   - Relación 1:1 con `Credencial`
   - Métodos: `ActualizarPerfil()`, `CambiarPlan()`, `AgregarEmpleado()`

2. Crear `EmpleadorConfiguration.cs`
   - Mapear a tabla: `Ofertantes` (legacy name)
   - Foreign key: `CredencialId` → `Credenciales.Id`

3. Actualizar DbContext
4. Validar compilación

---

#### Entidad 3: Contratista
**Razón:** Core business entity (similar a Empleador)

**Acciones AUTORIZADAS:**

1. Crear `src/Core/MiGenteEnLinea.Domain/Entities/Contratistas/Contratista.cs`
   - Heredar de: `AggregateRoot`
   - Relación 1:1 con `Credencial`
   - Métodos: `ActualizarPerfil()`, `AgregarServicio()`, `AgregarFoto()`

2. Crear `ContratistaConfiguration.cs`
   - Mapear a tabla: `Contratistas`
   - Foreign key: `CredencialId`

3. Actualizar DbContext
4. Validar compilación

---

### 🔄 PRIORIDAD 2: Infraestructura de Auditoría

**Acciones AUTORIZADAS:**

1. **Crear AuditableEntityInterceptor**
   - Archivo: `src/Infrastructure/Persistence/Interceptors/AuditableEntityInterceptor.cs`
   - Implementar: `SaveChangesInterceptor`
   - Actualizar automáticamente: `FechaCreacion`, `CreadoPor`, `FechaModificacion`, `ModificadoPor`

2. **Crear ICurrentUserService**
   - Interface: `src/Core/MiGenteEnLinea.Application/Common/Interfaces/ICurrentUserService.cs`
   - Método: `int? UserId { get; }`

3. **Implementar CurrentUserService**
   - Archivo: `src/Infrastructure/Identity/Services/CurrentUserService.cs`
   - Leer de: `IHttpContextAccessor` → Claims → `NameIdentifier`

4. **Registrar interceptor en DbContext**
   ```csharp
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
       optionsBuilder.AddInterceptors(new AuditableEntityInterceptor(_currentUserService));
   }
   ```

5. **Validar con build**

---

### 🚀 PRIORIDAD 3: CQRS con MediatR (Autenticación)

**Acciones AUTORIZADAS:**

#### Command: Registrar Usuario

1. **Crear Command**
   - Archivo: `src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/RegistrarUsuario/RegistrarUsuarioCommand.cs`
   - Propiedades: `Email`, `Password`, `Nombre`, `Apellido`, `TipoUsuario`
   - Implementar: `IRequest<int>` (retorna userId)

2. **Crear Handler**
   - Archivo: `RegistrarUsuarioCommandHandler.cs`
   - Implementar: `IRequestHandler<RegistrarUsuarioCommand, int>`
   - Lógica:
     1. Validar email único
     2. Hash password con BCrypt
     3. Crear Credencial
     4. Crear Empleador o Contratista según tipo
     5. Guardar en DB
     6. Retornar userId

3. **Crear Validator**
   - Archivo: `RegistrarUsuarioCommandValidator.cs`
   - FluentValidation rules:
     - Email: Required, Email format, MaxLength(100)
     - Password: MinLength(8), Regex (complejidad)
     - TipoUsuario: Must be "Empleador" or "Contratista"

4. **Crear DTOs**
   - Archivo: `src/Core/MiGenteEnLinea.Application/Features/Authentication/DTOs/UsuarioDto.cs`
   - Propiedades para response

5. **Crear Controller**
   - Archivo: `src/Presentation/MiGenteEnLinea.API/Controllers/AuthController.cs`
   - Endpoint: `POST /api/auth/register`
   - Inyectar: `IMediator`
   - Action: `await _mediator.Send(command)`

6. **Validar con build y test manual**

---

## 🛠️ PATRONES Y CONVENCIONES

### 🏛️ Domain-Driven Design

#### Rich Domain Model ✅
```csharp
// ✅ CORRECTO: Lógica de negocio en la entidad
public class Credencial : AuditableEntity
{
    public int Id { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool Activo { get; private set; }
    public DateTime? FechaActivacion { get; private set; }
    public int IntentosLoginFallidos { get; private set; }
    public DateTime? FechaBloqueo { get; private set; }

    // Constructor para EF Core
    private Credencial() { }

    // Factory method
    public static Credencial Crear(
        string email, 
        string passwordHash, 
        int userId)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email es requerido", nameof(email));

        return new Credencial
        {
            Email = email,
            PasswordHash = passwordHash,
            UserId = userId,
            Activo = false, // Requiere activación
            IntentosLoginFallidos = 0
        };
    }

    // Métodos de negocio
    public void Activar()
    {
        if (Activo)
            throw new InvalidOperationException("La credencial ya está activa");

        Activo = true;
        FechaActivacion = DateTime.UtcNow;
        
        // Domain event
        AddDomainEvent(new CredencialActivadaEvent(Id));
    }

    public void CambiarPassword(string nuevoPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(nuevoPasswordHash))
            throw new ArgumentException("Password hash es requerido");

        PasswordHash = nuevoPasswordHash;
        IntentosLoginFallidos = 0; // Reset intentos
    }

    public void RegistrarLoginFallido()
    {
        IntentosLoginFallidos++;

        if (IntentosLoginFallidos >= 5)
            BloquearCuenta();
    }

    private void BloquearCuenta()
    {
        Activo = false;
        FechaBloqueo = DateTime.UtcNow;
        AddDomainEvent(new CuentaBloqueadaEvent(Id, "Múltiples intentos fallidos"));
    }

    public void DesbloquearCuenta()
    {
        if (!FechaBloqueo.HasValue)
            throw new InvalidOperationException("La cuenta no está bloqueada");

        Activo = true;
        FechaBloqueo = null;
        IntentosLoginFallidos = 0;
    }
}
```

#### Fluent API Configuration ✅
```csharp
public class CredencialConfiguration : IEntityTypeConfiguration<Credencial>
{
    public void Configure(EntityTypeBuilder<Credencial> builder)
    {
        // Tabla
        builder.ToTable("Credenciales");

        // Primary Key
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        // Propiedades
        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("email");

        builder.Property(c => c.PasswordHash)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("password"); // Legacy column name

        builder.Property(c => c.Activo)
            .IsRequired()
            .HasColumnName("activo");

        builder.Property(c => c.FechaActivacion)
            .HasColumnName("fecha_activacion");

        builder.Property(c => c.IntentosLoginFallidos)
            .HasColumnName("intentos_login_fallidos")
            .HasDefaultValue(0);

        builder.Property(c => c.FechaBloqueo)
            .HasColumnName("fecha_bloqueo");

        // Índices
        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Credenciales_Email");

        builder.HasIndex(c => c.UserId)
            .IsUnique()
            .HasDatabaseName("IX_Credenciales_UserId");

        // Auditoría (heredado de AuditableEntity)
        builder.Property(c => c.FechaCreacion).HasColumnName("fecha_creacion");
        builder.Property(c => c.CreadoPor).HasColumnName("creado_por");
        builder.Property(c => c.FechaModificacion).HasColumnName("fecha_modificacion");
        builder.Property(c => c.ModificadoPor).HasColumnName("modificado_por");
    }
}
```

---

### 🔐 Seguridad OBLIGATORIA

#### BCrypt Password Hasher ✅
```csharp
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

public class BCryptPasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 12;

    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password no puede estar vacío", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        catch
        {
            return false;
        }
    }
}
```

#### FluentValidation ✅
```csharp
public class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    public RegistrarUsuarioCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email es requerido")
            .EmailAddress().WithMessage("Email no es válido")
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password es requerido")
            .MinimumLength(8).WithMessage("Password debe tener al menos 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$")
            .WithMessage("Password debe contener mayúscula, minúscula, número y carácter especial");

        RuleFor(x => x.TipoUsuario)
            .NotEmpty()
            .Must(t => t == "Empleador" || t == "Contratista")
            .WithMessage("TipoUsuario debe ser 'Empleador' o 'Contratista'");
    }
}
```

---

## 📊 FORMATO DE REPORTE DE PROGRESO

Cada 3 pasos completados, reporta con este formato:

```markdown
## 🔄 PROGRESO: [Nombre de la Tarea]

### ✅ Completado (Pasos 1-3)
- [x] **Paso 1:** Creada entidad `Credencial.cs` con Rich Domain Model
- [x] **Paso 2:** Creada configuración `CredencialConfiguration.cs` con Fluent API
- [x] **Paso 3:** Actualizado `MiGenteDbContext.cs` con `DbSet<Credencial>`

### 🔍 Validación Automática
- ✅ **Build:** Exitoso (0 errors, 0 warnings)
- ✅ **Nomenclatura:** Conforme con convenciones (español para dominio)
- ✅ **Seguridad:** BCrypt implementado correctamente
- ⚠️ **Tests:** Pendiente (fase posterior)

### 📁 Archivos Creados/Modificados
**Creados:**
- `src/Core/MiGenteEnLinea.Domain/Entities/Authentication/Credencial.cs` (247 líneas)
- `src/Infrastructure/Persistence/Configurations/CredencialConfiguration.cs` (89 líneas)

**Modificados:**
- `src/Infrastructure/Persistence/Contexts/MiGenteDbContext.cs` (+3 líneas)

### 🎯 Próximos Pasos
**Paso 4:** Implementar `BCryptPasswordHasher` en Infrastructure/Identity/Services/
**Paso 5:** Registrar servicios en `DependencyInjection.cs`
**Paso 6:** Ejecutar `dotnet build` para validación final

**Tiempo estimado restante:** 10-15 minutos
```

---

## 🚨 MANEJO DE ERRORES AUTÓNOMO

### Errores que DEBES Resolver Automáticamente

#### 1. Error de Compilación: Missing Using
**Error:**
```
The type or namespace name 'BCrypt' could not be found
```

**Acción Automática:**
1. Verificar que el paquete NuGet `BCrypt.Net-Next` esté instalado
2. Si no está: `dotnet add package BCrypt.Net-Next --version 4.0.3`
3. Agregar `using BCrypt.Net;` al archivo
4. Re-build

---

#### 2. Error de Configuración: DbContext Not Registered
**Error:**
```
Unable to resolve service for type 'MiGenteDbContext'
```

**Acción Automática:**
1. Abrir `src/Infrastructure/DependencyInjection.cs`
2. Verificar registro:
   ```csharp
   services.AddDbContext<MiGenteDbContext>(options =>
       options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
   ```
3. Si falta, agregarlo
4. Re-build

---

#### 3. Error de Migración: Table Already Exists
**Error:**
```
There is already an object named 'Credenciales' in the database.
```

**Acción Automática:**
1. NO ejecutar `dotnet ef database update` (requiere confirmación)
2. Reportar al usuario:
   ```
   ⚠️ MIGRACIÓN DETECTADA
   La tabla 'Credenciales' ya existe en la base de datos legacy.
   Configuración Code-First correcta (mapeo a tabla existente).
   NO se requiere migración en este momento.
   ```

---

### Errores que DEBES Reportar (No Resolver)

#### 1. Conflicto de Arquitectura
**Ejemplo:**
```
La entidad Empleador requiere relación N:N con Servicios,
pero el patrón actual no lo soporta claramente.
```

**Acción:**
```markdown
⚠️ DECISIÓN ARQUITECTÓNICA REQUERIDA

**Contexto:**
La entidad Empleador necesita relacionarse con Servicios (N:N).

**Opciones:**
1. Crear entidad intermedia: `EmpleadorServicio`
2. Usar colección directa: `ICollection<Servicio>`

**Recomendación:**
Opción 1 (entidad intermedia) para auditoría y data adicional.

**Requiere tu aprobación para continuar.**
```

---

#### 2. Error de Conexión a Base de Datos
**Error:**
```
Cannot open database "db_a9f8ff_migente". Login failed.
```

**Acción:**
```markdown
⚠️ ERROR DE CONEXIÓN

La base de datos no está accesible.

**Verificar:**
1. SQL Server está corriendo
2. Connection string es correcta
3. Credenciales son válidas

**Comando para verificar:**
```powershell
sqlcmd -S localhost,1433 -U sa -P 1234 -Q "SELECT DB_NAME()"
```

**No puedo continuar hasta resolver la conexión.**
```

---

## 🎯 COMANDO DE INICIO RÁPIDO

Para iniciar el agente en modo autónomo, usa:

```
@workspace Lee y ejecuta el archivo prompts/AGENT_MODE_INSTRUCTIONS.md

TAREA: Refactorizar entidades [Credencial, Empleador, Contratista] con patrón DDD

AUTORIZACIÓN COMPLETA:
✅ Crear/modificar archivos en Domain, Application, Infrastructure, API
✅ Configurar DbContext y Fluent API
✅ Implementar servicios de seguridad (BCrypt, JWT)
✅ Ejecutar dotnet build para validación
✅ Corregir errores de compilación automáticamente
✅ Registrar servicios en DI
✅ Reportar solo cuando completes cada entidad

LÍMITES:
⛔ NO ejecutar migraciones (dotnet ef database update)
⛔ NO modificar código en "Codigo Fuente Mi Gente/"
⛔ NO crear tests aún (fase posterior)
⛔ NO cambiar connection strings en producción

WORKSPACE ROOT: C:\Users\ray\OneDrive\Documents\ProyectoMigente\

INICIO: Entidad Credencial (Paso 1 de 7)
```

---

## ✅ CHECKLIST FINAL POR ENTIDAD

Al completar cada entidad, valida:

### Credencial
- [ ] ✅ Entidad creada en `Domain/Entities/Authentication/Credencial.cs`
- [ ] ✅ Hereda de `AuditableEntity`
- [ ] ✅ Propiedades privadas con encapsulación
- [ ] ✅ Métodos de negocio: `Activar()`, `CambiarPassword()`, `RegistrarLoginFallido()`, `BloquearCuenta()`
- [ ] ✅ Password como `PasswordHash` (BCrypt)
- [ ] ✅ Domain events: `CredencialActivadaEvent`, `CuentaBloqueadaEvent`
- [ ] ✅ Configuration creada: `CredencialConfiguration.cs`
- [ ] ✅ Mapeo a tabla: `Credenciales` (legacy)
- [ ] ✅ Índice único en: `Email`, `UserId`
- [ ] ✅ DbContext actualizado con `DbSet<Credencial>`
- [ ] ✅ `BCryptPasswordHasher` implementado
- [ ] ✅ Servicios registrados en DI
- [ ] ✅ `dotnet build` exitoso

### Empleador
- [ ] ✅ Entidad creada en `Domain/Entities/Empleadores/Empleador.cs`
- [ ] ✅ Hereda de `AggregateRoot`
- [ ] ✅ Relación 1:1 con `Credencial`
- [ ] ✅ Métodos de negocio relevantes
- [ ] ✅ Configuration creada (mapeo a tabla `Ofertantes`)
- [ ] ✅ DbContext actualizado
- [ ] ✅ `dotnet build` exitoso

### Contratista
- [ ] ✅ Entidad creada en `Domain/Entities/Contratistas/Contratista.cs`
- [ ] ✅ Hereda de `AggregateRoot`
- [ ] ✅ Relación 1:1 con `Credencial`
- [ ] ✅ Métodos de negocio relevantes
- [ ] ✅ Configuration creada
- [ ] ✅ DbContext actualizado
- [ ] ✅ `dotnet build` exitoso

---

## 📚 REFERENCIAS RÁPIDAS

### Convenciones de Nombres
- **Entidades:** Español (Credencial, Empleador, Contratista)
- **Tablas DB:** Legacy names (Credenciales, Ofertantes, Contratistas)
- **Propiedades:** PascalCase en C#, snake_case en DB
- **Métodos:** Verbos en español (Activar, Crear, Actualizar)

### Paths Importantes
```
WORKSPACE ROOT: C:\Users\ray\OneDrive\Documents\ProyectoMigente\

LEGACY (READ-ONLY):
- Codigo Fuente Mi Gente/MiGente_Front/Data/*.cs

CLEAN (MODIFICAR):
- MiGenteEnLinea.Clean/src/Core/MiGenteEnLinea.Domain/
- MiGenteEnLinea.Clean/src/Core/MiGenteEnLinea.Application/
- MiGenteEnLinea.Clean/src/Infrastructure/MiGenteEnLinea.Infrastructure/
- MiGenteEnLinea.Clean/src/Presentation/MiGenteEnLinea.API/
```

### Comandos Útiles
```powershell
# Build
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean"
dotnet build

# Restaurar paquetes
dotnet restore

# Ver conexión a DB
sqlcmd -S localhost,1433 -U sa -P 1234 -Q "SELECT DB_NAME()"

# Git status
git status

# Crear commit
git add .
git commit -m "feat: refactor Credencial entity with DDD pattern"
```

---

## 🚀 ¡LISTO PARA EJECUTAR!

**Modo:** Agente Autónomo Activado  
**Estado:** Esperando comando de inicio  
**Autorización:** Nivel COMPLETO para Clean Architecture project  

Cuando estés listo, proporciona la tarea específica y **ejecutaré todos los pasos sin pausas innecesarias**.

---

_Versión 2.0 - Optimizado para Claude Sonnet 4.5 - 12 de octubre, 2025_
