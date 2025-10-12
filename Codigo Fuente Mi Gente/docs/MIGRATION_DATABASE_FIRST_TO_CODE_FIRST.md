# 🔄 Guía de Migración: Database-First a Code-First

## 📊 Análisis del Estado Actual

### Estado Actual (Database-First con EDMX)
```
MiGente_Front/Data/
├── DataModel.edmx                    # Modelo visual Entity Framework 6
├── DataModel.Context.cs              # DbContext auto-generado
├── DataModel.Designer.cs             # Designer auto-generado
├── DataModel.tt / DataModel.Context.tt # T4 Templates
└── [45 archivos de entidades].cs    # Entidades auto-generadas
```

### Entidades Identificadas (45 total)

#### 🔐 Autenticación y Usuarios
- `Credenciales` - Credenciales de login (⚠️ password en texto plano)
- `Ofertantes` - Perfiles de empleadores
- `Contratistas` - Perfiles de contratistas
- `Contratistas_Fotos` - Fotos de perfil de contratistas
- `Contratistas_Servicios` - Servicios ofrecidos por contratistas
- `perfilesInfo` - Información adicional de perfiles
- `VPerfiles` - Vista de perfiles

#### 💼 Módulo de Empleadores
- `Empleados` - Empleados registrados
- `EmpleadosTemporales` - Empleados temporales/contrataciones
- `VEmpleados` - Vista de empleados

#### 💰 Nómina y Pagos
- `Empleador_Recibos_Header` - Encabezado de recibos de nómina
- `Empleador_Recibos_Detalle` - Detalle de recibos de nómina
- `Empleador_Recibos_Header_Contrataciones` - Encabezado recibos de contrataciones
- `Empleador_Recibos_Detalle_Contrataciones` - Detalle recibos de contrataciones
- `Remuneraciones` - Tipos de remuneraciones
- `Deducciones_TSS` - Deducciones de seguridad social (TSS)
- `VRecibosEmpleados` - Vista de recibos de empleados
- `VPagosContrataciones` - Vista de pagos de contrataciones

#### 🏦 Contrataciones Temporales
- `DetalleContrataciones` - Detalle de contrataciones
- `VContratacionesTemporales` - Vista de contrataciones temporales

#### ⭐ Calificaciones
- `Calificaciones` - Calificaciones de servicios
- `VCalificaciones` - Vista de calificaciones
- `VMisCalificaciones` - Vista de mis calificaciones

#### 💳 Suscripciones y Pagos
- `Suscripciones` - Suscripciones activas
- `Planes_empleadores` - Planes para empleadores
- `Planes_Contratistas` - Planes para contratistas
- `Ventas` - Registro de ventas
- `PaymentGateway` - Configuración de pasarela de pago (Cardnet)
- `Cuentas` - Cuentas bancarias

#### 📍 Catálogos y Configuración
- `Provincias` - Catálogo de provincias (República Dominicana)
- `Sectores` - Catálogo de sectores económicos
- `Servicios` - Catálogo de servicios ofrecidos
- `Config_Correo` - Configuración de correo electrónico
- `OpenAi_Config` - Configuración de OpenAI
- `sysdiagrams` - Diagramas del sistema (SQL Server)

#### 📊 Stored Procedures y Funciones
- `ObtenerSuscripcion_Result` - Resultado de SP de suscripción
- `sp_MisCalificaciones_Result` - Resultado de SP de calificaciones

#### 🔍 Modelos Adicionales
- `VContratistas` - Vista de contratistas
- `PadronModel` - Modelo para consulta de padrón nacional

---

## 🎯 Estrategia de Migración

### Fase 1: Preparación y Setup

#### 1.1 Crear Backup de Base de Datos
```sql
-- Ejecutar en SQL Server Management Studio
BACKUP DATABASE migenteV2 
TO DISK = 'C:\Backups\migenteV2_PreMigration_{YYYYMMDD}.bak'
WITH FORMAT, 
     NAME = 'Pre-Migration Full Backup',
     DESCRIPTION = 'Backup antes de migración a Code-First';
GO

-- Generar script de schema completo
USE migenteV2
GO
EXEC sp_help -- Revisar todas las tablas
```

#### 1.2 Documentar Schema Actual
```bash
# Exportar schema con SMO
# O usar herramienta visual de SQL Server para generar script CREATE
```

#### 1.3 Instalar Herramientas Necesarias
```powershell
# Instalar Entity Framework Core Tools
dotnet tool install --global dotnet-ef

# Verificar instalación
dotnet ef --version
```

### Fase 2: Scaffolding Inicial (Generación Automática)

#### 2.1 Usar Scaffold-DbContext para Generar Entidades

```powershell
# Navegar a la carpeta del nuevo proyecto (a crear)
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\Codigo Fuente Mi Gente"

# Crear nueva solución Clean Architecture
dotnet new sln -n MiGenteEnLinea.Clean

# Crear proyectos
dotnet new classlib -n MiGenteEnLinea.Domain -f net8.0
dotnet new classlib -n MiGenteEnLinea.Application -f net8.0
dotnet new classlib -n MiGenteEnLinea.Infrastructure -f net8.0
dotnet new webapi -n MiGenteEnLinea.API -f net8.0

# Agregar proyectos a la solución
dotnet sln add MiGenteEnLinea.Domain/MiGenteEnLinea.Domain.csproj
dotnet sln add MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj
dotnet sln add MiGenteEnLinea.Infrastructure/MiGenteEnLinea.Infrastructure.csproj
dotnet sln add MiGenteEnLinea.API/MiGenteEnLinea.API.csproj

# Instalar paquetes en Infrastructure
cd MiGenteEnLinea.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0

# SCAFFOLD: Generar entidades desde base de datos existente
dotnet ef dbcontext scaffold "Server=.;Database=migenteV2;User Id=sa;Password=1234;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --output-dir Persistence/Entities/Generated --context-dir Persistence/Contexts --context MiGenteDbContext --force --data-annotations --no-onconfiguring
```

**Opciones del comando scaffold explicadas:**
- `--output-dir Persistence/Entities/Generated` - Carpeta para entidades generadas
- `--context-dir Persistence/Contexts` - Carpeta para DbContext
- `--context MiGenteDbContext` - Nombre del DbContext
- `--force` - Sobrescribir archivos existentes
- `--data-annotations` - Usar data annotations además de Fluent API
- `--no-onconfiguring` - No generar OnConfiguring en DbContext

#### 2.2 Resultado Esperado del Scaffolding

```
MiGenteEnLinea.Infrastructure/
└── Persistence/
    ├── Contexts/
    │   └── MiGenteDbContext.cs          # DbContext generado
    └── Entities/
        └── Generated/                    # 45 archivos de entidades
            ├── Credenciale.cs
            ├── Empleado.cs
            ├── Contratista.cs
            ├── Suscripcione.cs
            └── ... (41 más)
```

### Fase 3: Refactoring Manual de Entidades

#### 3.1 Mover Entidades a Domain Layer

Las entidades generadas deben moverse y refactorizarse:

```
MiGenteEnLinea.Domain/
└── Entities/
    ├── Authentication/
    │   ├── Credencial.cs                 # Renombrado, singular
    │   └── PerfilInfo.cs
    ├── Empleadores/
    │   ├── Ofertante.cs
    │   ├── Empleado.cs
    │   └── EmpleadoTemporal.cs
    ├── Contratistas/
    │   ├── Contratista.cs
    │   ├── ContratistaFoto.cs
    │   └── ContratistaServicio.cs
    ├── Nomina/
    │   ├── ReciboHeader.cs
    │   ├── ReciboDetalle.cs
    │   ├── Remuneracion.cs
    │   └── DeduccionTSS.cs
    ├── Contrataciones/
    │   ├── DetalleContratacion.cs
    │   └── ReciboContratacion.cs
    ├── Calificaciones/
    │   └── Calificacion.cs
    ├── Suscripciones/
    │   ├── Suscripcion.cs
    │   ├── PlanEmpleador.cs
    │   ├── PlanContratista.cs
    │   └── Venta.cs
    ├── Pagos/
    │   ├── PaymentGateway.cs
    │   └── Cuenta.cs
    └── Catalogos/
        ├── Provincia.cs
        ├── Sector.cs
        └── Servicio.cs
```

#### 3.2 Aplicar Domain-Driven Design a las Entidades

**ANTES (Generado automáticamente):**
```csharp
// Generated/Credenciale.cs
public partial class Credenciale
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }  // ⚠️ Texto plano
    public bool? Activo { get; set; }
}
```

**DESPUÉS (Refactorizado con DDD):**
```csharp
// Domain/Entities/Authentication/Credencial.cs
namespace MiGenteEnLinea.Domain.Entities.Authentication
{
    public class Credencial
    {
        // Constructor privado para Entity Framework
        private Credencial() { }
        
        // Factory method con validaciones
        public static Credencial Create(string userId, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("UserId es requerido", nameof(userId));
            
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email es requerido", nameof(email));
            
            if (!IsValidEmail(email))
                throw new ArgumentException("Email inválido", nameof(email));
            
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("PasswordHash es requerido", nameof(passwordHash));
            
            return new Credencial
            {
                UserId = userId,
                Email = email.ToLowerInvariant(),
                PasswordHash = passwordHash,
                Activo = false, // Requiere activación
                FechaCreacion = DateTime.UtcNow
            };
        }
        
        // Propiedades con encapsulación
        public int Id { get; private set; }
        public string UserId { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }  // ✅ Hasheado
        public bool Activo { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime? FechaActivacion { get; private set; }
        public DateTime? UltimoAcceso { get; private set; }
        
        // Métodos de negocio
        public void Activar()
        {
            if (Activo)
                throw new InvalidOperationException("La credencial ya está activa");
            
            Activo = true;
            FechaActivacion = DateTime.UtcNow;
        }
        
        public void Desactivar()
        {
            Activo = false;
        }
        
        public void ActualizarPasswordHash(string nuevoPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(nuevoPasswordHash))
                throw new ArgumentException("PasswordHash no puede estar vacío");
            
            PasswordHash = nuevoPasswordHash;
        }
        
        public void RegistrarAcceso()
        {
            UltimoAcceso = DateTime.UtcNow;
        }
        
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
```

#### 3.3 Crear Fluent API Configurations

```csharp
// Infrastructure/Persistence/Configurations/CredencialConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiGenteEnLinea.Domain.Entities.Authentication;

namespace MiGenteEnLinea.Infrastructure.Persistence.Configurations
{
    public class CredencialConfiguration : IEntityTypeConfiguration<Credencial>
    {
        public void Configure(EntityTypeBuilder<Credencial> builder)
        {
            builder.ToTable("Credenciales");
            
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();
            
            builder.Property(c => c.UserId)
                .HasColumnName("userID")
                .IsRequired()
                .HasMaxLength(50);
            
            builder.Property(c => c.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(c => c.PasswordHash)
                .HasColumnName("password")
                .IsRequired()
                .HasMaxLength(500); // BCrypt genera ~60 caracteres
            
            builder.Property(c => c.Activo)
                .HasColumnName("activo")
                .HasDefaultValue(false);
            
            builder.Property(c => c.FechaCreacion)
                .HasColumnName("fechaCreacion")
                .HasDefaultValueSql("GETUTCDATE()");
            
            builder.Property(c => c.FechaActivacion)
                .HasColumnName("fechaActivacion")
                .IsRequired(false);
            
            builder.Property(c => c.UltimoAcceso)
                .HasColumnName("ultimoAcceso")
                .IsRequired(false);
            
            // Índices
            builder.HasIndex(c => c.UserId)
                .IsUnique()
                .HasDatabaseName("IX_Credenciales_UserId");
            
            builder.HasIndex(c => c.Email)
                .IsUnique()
                .HasDatabaseName("IX_Credenciales_Email");
        }
    }
}
```

### Fase 4: Crear DbContext con Fluent API

```csharp
// Infrastructure/Persistence/Contexts/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Authentication;
using MiGenteEnLinea.Domain.Entities.Empleadores;
using MiGenteEnLinea.Domain.Entities.Contratistas;
// ... más imports

namespace MiGenteEnLinea.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        // DbSets
        public DbSet<Credencial> Credenciales { get; set; }
        public DbSet<Ofertante> Ofertantes { get; set; }
        public DbSet<Contratista> Contratistas { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Suscripcion> Suscripciones { get; set; }
        public DbSet<PlanEmpleador> PlanesEmpleadores { get; set; }
        public DbSet<PlanContratista> PlanesContratistas { get; set; }
        // ... más DbSets (45 total)
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Aplicar todas las configuraciones automáticamente
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            
            // Configuraciones globales
            ConfigureGlobalFilters(modelBuilder);
            ConfigureConventions(modelBuilder);
        }
        
        private void ConfigureGlobalFilters(ModelBuilder modelBuilder)
        {
            // Soft delete global
            modelBuilder.Entity<Credencial>().HasQueryFilter(c => c.Activo);
            modelBuilder.Entity<Empleado>().HasQueryFilter(e => e.Activo.HasValue && e.Activo.Value);
            // ... más filtros
        }
        
        private void ConfigureConventions(ModelBuilder modelBuilder)
        {
            // Convención: Todas las propiedades string con MaxLength(255) por defecto
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(255);
                    }
                }
            }
        }
    }
}
```

### Fase 5: Generar Migration Inicial

```powershell
# En Infrastructure project
cd MiGenteEnLinea.Infrastructure

# Crear migration inicial desde DB existente
dotnet ef migrations add InitialCreate --startup-project ../MiGenteEnLinea.API --context ApplicationDbContext --output-dir Persistence/Migrations

# Esto NO aplicará cambios a la DB, solo genera el migration script
# El migration script tendrá Up() y Down() vacíos o casi vacíos porque la DB ya existe
```

#### 5.1 Modificar Migration Inicial

```csharp
// Migrations/XXXXXX_InitialCreate.cs
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // VACÍO o comentado
        // La base de datos ya existe, no necesitamos crear tablas
        
        // Solo agregar columnas nuevas si es necesario
        // Por ejemplo, agregar FechaCreacion, FechaActivacion, etc.
        
        migrationBuilder.AddColumn<DateTime>(
            name: "fechaCreacion",
            table: "Credenciales",
            type: "datetime2",
            nullable: false,
            defaultValueSql: "GETUTCDATE()");
        
        migrationBuilder.AddColumn<DateTime>(
            name: "fechaActivacion",
            table: "Credenciales",
            type: "datetime2",
            nullable: true);
        
        migrationBuilder.AddColumn<DateTime>(
            name: "ultimoAcceso",
            table: "Credenciales",
            type: "datetime2",
            nullable: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "fechaCreacion", table: "Credenciales");
        migrationBuilder.DropColumn(name: "fechaActivacion", table: "Credenciales");
        migrationBuilder.DropColumn(name: "ultimoAcceso", table: "Credenciales");
    }
}
```

### Fase 6: Migración Gradual de Passwords

```csharp
// Infrastructure/Services/PasswordMigrationService.cs
public class PasswordMigrationService
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<PasswordMigrationService> _logger;
    
    public async Task MigratePasswordsAsync()
    {
        var credenciales = await _context.Credenciales
            .Where(c => !c.PasswordHash.StartsWith("$2a$")) // No empiezan con BCrypt
            .ToListAsync();
        
        _logger.LogInformation($"Encontradas {credenciales.Count} contraseñas sin hashear");
        
        foreach (var credencial in credenciales)
        {
            // OPCIÓN 1: Forzar reset de password
            credencial.ActualizarPasswordHash("RESET_REQUIRED");
            // Enviar email de reset
            
            // OPCIÓN 2: Hash del password existente (solo si tenemos acceso)
            // var hashedPassword = _passwordHasher.HashPassword(credencial.PasswordHash);
            // credencial.ActualizarPasswordHash(hashedPassword);
        }
        
        await _context.SaveChangesAsync();
        _logger.LogInformation("Migración de contraseñas completada");
    }
}
```

### Fase 7: Testing y Validación

#### 7.1 Crear Integration Tests

```csharp
// Tests/Infrastructure.Tests/Persistence/ApplicationDbContextTests.cs
public class ApplicationDbContextTests : IClassFixture<DatabaseFixture>
{
    private readonly ApplicationDbContext _context;
    
    public ApplicationDbContextTests(DatabaseFixture fixture)
    {
        _context = fixture.CreateContext();
    }
    
    [Fact]
    public async Task CanConnectToDatabase()
    {
        var canConnect = await _context.Database.CanConnectAsync();
        Assert.True(canConnect);
    }
    
    [Fact]
    public async Task CanQueryCredenciales()
    {
        var credenciales = await _context.Credenciales.ToListAsync();
        Assert.NotNull(credenciales);
    }
    
    [Fact]
    public async Task CanCreateCredencial()
    {
        var credencial = Credencial.Create("testuser", "test@example.com", "hashedpassword");
        _context.Credenciales.Add(credencial);
        await _context.SaveChangesAsync();
        
        Assert.True(credencial.Id > 0);
    }
}
```

#### 7.2 Verificar Mapping

```powershell
# Generar script de la base de datos desde Code-First
dotnet ef migrations script --startup-project ../MiGenteEnLinea.API --context ApplicationDbContext --output DBScript.sql

# Comparar con schema actual usando herramientas como:
# - SQL Server Data Tools (SSDT) Schema Compare
# - Redgate SQL Compare
# - ApexSQL Diff
```

### Fase 8: Configuración de Dependency Injection

```csharp
// Infrastructure/DependencyInjection.cs
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                });
            
            // Solo en desarrollo
            if (configuration.GetValue<bool>("EnableSensitiveDataLogging"))
            {
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }
        });
        
        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        // Services
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<IPasswordMigrationService, PasswordMigrationService>();
        
        return services;
    }
}
```

---

## 📋 Checklist de Migración

### Pre-Migración
- [ ] Backup de base de datos creado
- [ ] Script de schema exportado
- [ ] Documentación de relaciones revisada
- [ ] Stored procedures documentados
- [ ] Vistas documentadas
- [ ] Funciones documentadas

### Generación Code-First
- [ ] Solución Clean Architecture creada
- [ ] Paquetes NuGet instalados
- [ ] Scaffold-DbContext ejecutado exitosamente
- [ ] Entidades generadas revisadas
- [ ] DbContext generado revisado

### Refactoring
- [ ] Entidades movidas a Domain layer
- [ ] Nombres singularizados (Credencial vs Credenciales)
- [ ] Encapsulación aplicada (private setters)
- [ ] Factory methods creados
- [ ] Métodos de negocio agregados
- [ ] Value Objects extraídos si es necesario

### Configuraciones
- [ ] Fluent API configurations creadas (45 archivos)
- [ ] ApplicationDbContext configurado
- [ ] Índices configurados
- [ ] Foreign keys configuradas
- [ ] Check constraints configuradas
- [ ] Default values configuradas
- [ ] Query filters configurados (soft delete)

### Migrations
- [ ] Migration inicial creada
- [ ] Migration inicial revisada
- [ ] Columnas nuevas agregadas en migration
- [ ] Script de migración de passwords preparado
- [ ] Migrations aplicadas en ambiente de desarrollo

### Testing
- [ ] Integration tests creados
- [ ] Conexión a DB testeada
- [ ] CRUD operations testeadas
- [ ] Relaciones testeadas
- [ ] Query filters testeados
- [ ] Performance baseline capturado

### Deployment
- [ ] Connection strings actualizadas
- [ ] Secrets movidos a User Secrets / Key Vault
- [ ] Dependency Injection configurado
- [ ] Logging configurado
- [ ] Health checks configurados
- [ ] Backup final pre-deployment

---

## 🚨 Riesgos y Mitigaciones

### Riesgo 1: Pérdida de Datos
**Mitigación**: 
- Backup completo antes de cualquier cambio
- Testing exhaustivo en ambiente de desarrollo primero
- Migration scripts revisados manualmente
- Rollback plan documentado

### Riesgo 2: Downtime Prolongado
**Mitigación**:
- Aplicar migrations fuera de horas pico
- Preparar scripts de rollback
- Estimar tiempo basado en tamaño de DB
- Blue-green deployment si es posible

### Riesgo 3: Incompatibilidad con Legacy Code
**Mitigación**:
- Mantener Web Forms funcionando en paralelo
- Usar feature flags para switch gradual
- Crear adapters si es necesario
- Migración gradual por módulo

### Riesgo 4: Performance Degradation
**Mitigación**:
- Profiling antes y después
- Índices optimizados
- Query optimization
- Connection pooling configurado
- Monitoring en producción

---

## 📅 Timeline Estimado

### Sprint 1 (Semanas 1-2): Setup y Generación
- Días 1-2: Backup y análisis
- Días 3-5: Scaffolding y setup inicial
- Días 6-10: Refactoring de primeras 10 entidades críticas

### Sprint 2 (Semanas 3-4): Refactoring Completo
- Días 1-5: Refactoring de 20 entidades restantes
- Días 6-10: Fluent API configurations

### Sprint 3 (Semanas 5-6): Testing y Deployment
- Días 1-3: Integration tests
- Días 4-6: Migration scripts y password migration
- Días 7-10: Deployment y monitoring

---

## 🔗 Próximos Pasos

1. **Aprobar esta guía** y ajustar timeline según disponibilidad
2. **Crear backup** de la base de datos
3. **Ejecutar Scaffold-DbContext** para ver resultado inicial
4. **Revisar entidades generadas** y planificar refactoring
5. **Comenzar con entidad Credencial** como proof of concept

---

**Última actualización**: Octubre 2025
**Revisado por**: Equipo de Desarrollo
**Aprobado por**: [Pendiente]
