# MiGente En Línea - AI Coding Instructions

> **📍 Workspace Location:** `C:\Users\ray\OneDrive\Documents\ProyectoMigente\`  
> **🤖 AI Agent Mode:** GitHub Copilot (IDE Integration)  
> **📚 Advanced Prompts:** See `/prompts/` folder for Claude Sonnet 4.5 and other agents

---

## 🚨 CRITICAL: Dual-Project Workspace Context

**⚠️ ACTIVE DEVELOPMENT**: This workspace contains TWO projects running simultaneously during migration:

### 🔷 PROJECT 1: Legacy Web Forms (Maintenance Mode)

**Location:** `Codigo Fuente Mi Gente/`  
**Purpose:** Production system being phased out  
**DO NOT:** Add new features or major refactoring  
**DO:** Only critical bug fixes and security patches

### 🚀 PROJECT 2: Clean Architecture (Active Development)

**Location:** `MiGenteEnLinea.Clean/`  
**Purpose:** New modern implementation being built  
**DO:** All new development, DDD refactoring, testing  
**DO:** Reference legacy code for business logic understanding

---

## 🤖 AI Agent Resources

This workspace provides specialized prompts for different AI agents:

### For GitHub Copilot (This File)

- **Mode:** IDE Integration (autocomplete, chat)
- **Purpose:** Quick suggestions, code completion, inline help
- **Scope:** Small to medium tasks
- **Location:** `.github/copilot-instructions.md` (auto-loaded by VS Code)

### For Claude Sonnet 4.5 / External Agents

- **Mode:** Autonomous Agent (batch execution)
- **Purpose:** Large refactoring, multi-file changes, DDD migration
- **Scope:** Complex architectural tasks
- **Location:** `/prompts/AGENT_MODE_INSTRUCTIONS.md`
- **Documentation:** `/prompts/README.md`

**📖 Quick Reference:**

```
/prompts/
├── README.md                        # Guide for using prompts
├── AGENT_MODE_INSTRUCTIONS.md       # Claude Sonnet 4.5 autonomous mode
└── ddd-migration-agent.md           # DDD migration workflow (coming soon)
```

---

## 🏗️ Workspace Structure

This is a **multi-root VS Code workspace** combining both projects:

```
ProyectoMigente/ (WORKSPACE ROOT = REPOSITORY ROOT)
├── .git/                                # ✅ Git repository
├── .github/                             # ✅ GitHub configuration
├── .gitignore                           # ✅ Workspace gitignore
├── README.md                            # ✅ Main documentation
├── WORKSPACE_README.md                  # ✅ Workspace guide
├── MiGenteEnLinea-Workspace.code-workspace  # ✅ VS Code config
│
├── 🔷 Codigo Fuente Mi Gente/          # LEGACY PROJECT
│   ├── MiGente.sln                      # .NET Framework 4.7.2
│   ├── MiGente_Front/                   # ASP.NET Web Forms
│   │   ├── Data/                        # EF6 Database-First (EDMX)
│   │   ├── Services/                    # Business logic
│   │   ├── Empleador/                   # Employer module
│   │   └── Contratista/                 # Contractor module
│   ├── docs/                            # Migration documentation
│   └── scripts/                         # Automation scripts
│
└── 🚀 MiGenteEnLinea.Clean/            # CLEAN ARCHITECTURE PROJECT
    ├── MiGenteEnLinea.Clean.sln         # .NET 8.0
    ├── src/
    │   ├── Core/
    │   │   ├── MiGenteEnLinea.Domain/           # ✅ Active development
    │   │   │   ├── Entities/                     # DDD entities
    │   │   │   ├── ValueObjects/                 # DDD value objects
    │   │   │   └── Common/                       # Base classes
    │   │   └── MiGenteEnLinea.Application/      # ✅ Active development
    │   │       ├── Features/                     # CQRS use cases
    │   │       └── Common/                       # DTOs, interfaces
    │   ├── Infrastructure/
    │   │   └── MiGenteEnLinea.Infrastructure/   # ✅ Active development
    │   │       ├── Persistence/
    │   │       │   ├── Contexts/                 # DbContext
    │   │       │   ├── Entities/Generated/       # 36 scaffolded entities
    │   │       │   └── Configurations/           # Fluent API
    │   │       └── Services/                     # External services
    │   └── Presentation/
    │       └── MiGenteEnLinea.API/              # ✅ Active development
    │           └── Controllers/                  # REST API endpoints
    └── tests/                                    # ✅ Active development
```

**⚠️ IMPORTANT NAVIGATION RULES:**

- When asked about **"legacy"**, **"Web Forms"**, or **"old project"** → Work in `Codigo Fuente Mi Gente/`
- When asked about **"clean"**, **"new project"**, or **"API"** → Work in `MiGenteEnLinea.Clean/`
- When asked about **"migration"** or **"refactoring"** → Reference legacy, implement in clean
- When asked about **"business logic"** → Check legacy first to understand, then implement properly in clean

---

## 🚨 CRITICAL: Security Remediation in Progress

**🔒 SECURITY PRIORITY**: All AI agents must prioritize security fixes identified in September 2025 audit before implementing new features.

## Project Overview

**MiGente En Línea** is a platform for managing employment relationships in the Dominican Republic. It connects **Empleadores** (employers) and **Contratistas** (contractors/service providers) with subscription-based access and integrated payment processing.

### 🔷 Legacy System (Current Production)

- ASP.NET Web Forms (.NET Framework 4.7.2)
- Database-First Entity Framework 6 with EDMX
- Forms Authentication with cookies
- Multiple critical security vulnerabilities identified
- Monolithic architecture without layer separation
- Database: `db_a9f8ff_migente` on SQL Server

### 🚀 Clean Architecture System (Under Development)

- ASP.NET Core 8.0 Web API
- Clean Architecture (Onion Architecture)
- Code-First Entity Framework Core 8
- JWT Authentication with refresh tokens
- Domain-Driven Design (DDD) with rich domain models
- CQRS pattern with MediatR
- Comprehensive security hardening
- Same database: `db_a9f8ff_migente` (gradual migration)

## 🔷 Legacy Architecture & Technology Stack

### Core Framework

- **ASP.NET Web Forms** (.NET Framework 4.7.2)
- **Entity Framework 6** for data access (Database-First approach with EDMX)
- **SQL Server** database (`db_a9f8ff_migente`)
- **IIS Express** for local development (port 44358 with SSL)

### Key Dependencies

- **DevExpress v23.1**: Commercial UI component library (ASPxGridView, Bootstrap controls)
- **iText 8.0.5**: PDF generation (contracts, receipts, payroll documents)
- **Cardnet Payment Gateway**: Dominican payment processor integration
- **OpenAI Integration**: Virtual legal assistant ("abogado virtual")
- **RestSharp 112.1.0**: HTTP client for external API calls
- **Newtonsoft.Json 13.0.3**: JSON serialization

### Authentication & Authorization

- **Forms Authentication** with cookie-based sessions (`~/Login.aspx` as login URL)
- **Two user roles** stored in cookies:
  - `tipo = "1"`: Empleador (Employer) → redirects to `/comunidad.aspx`
  - `tipo = "2"`: Contratista (Contractor) → redirects to `/Contratista/index_contratista.aspx`
- Cookie structure: `login` cookie contains `userID`, `nombre`, `tipo`, `planID`, `vencimientoPlan`, `email`

---

## 🚀 Clean Architecture & Technology Stack

### Core Framework

- **ASP.NET Core 8.0** Web API
- **Entity Framework Core 8.0** for data access (Code-First approach)
- **SQL Server** database (`db_a9f8ff_migente` - same as legacy)
- **Kestrel** web server (ports: 5000 HTTP, 5001 HTTPS)

### Architecture Layers

#### 1. Domain Layer (`MiGenteEnLinea.Domain`)

**Purpose:** Core business logic and entities (no dependencies)

- **Entities/**: Rich domain models with business logic
  - `Authentication/Credencial.cs` - User authentication entity
  - `Empleadores/Empleador.cs` - Employer aggregate root
  - `Contratistas/Contratista.cs` - Contractor aggregate root
  - `Empleados/Empleado.cs` - Employee entity
  - `Suscripciones/Suscripcion.cs` - Subscription entity
- **ValueObjects/**: Immutable value objects (Email, Money, DateRange, etc.)
- **Common/**: Base classes (`AuditableEntity`, `SoftDeletableEntity`, `AggregateRoot`)
- **Events/**: Domain events for communication between aggregates
- **Interfaces/**: Repository interfaces, domain services

#### 2. Application Layer (`MiGenteEnLinea.Application`)

**Purpose:** Use cases and application logic

- **Features/**: Organized by feature (CQRS pattern)
  - `Authentication/`
    - `Commands/`: Register, Login, ChangePassword, ResetPassword
    - `Queries/`: GetUser, ValidateToken
    - `DTOs/`: UsuarioDto, CredencialDto
    - `Validators/`: FluentValidation rules
  - `Empleadores/`, `Contratistas/`, `Empleados/`, etc.
- **Common/**: Shared application logic
  - `Interfaces/`: IDateTime, IEmailService, IFileStorage
  - `Behaviors/`: MediatR pipelines (Validation, Logging, Transaction)
  - `Mappings/`: AutoMapper profiles
  - `Exceptions/`: Application-specific exceptions

**Dependencies:**

- `MediatR 12.2.0` - CQRS implementation
- `AutoMapper 12.0.1` - Object mapping
- `FluentValidation 11.9.0` - Input validation

#### 3. Infrastructure Layer (`MiGenteEnLinea.Infrastructure`)

**Purpose:** External concerns and persistence

- **Persistence/**
  - `Contexts/MiGenteDbContext.cs` - EF Core DbContext
  - `Entities/Generated/` - 36 scaffolded entities from legacy DB
  - `Configurations/` - Fluent API configurations
  - `Repositories/` - Repository implementations
  - `Interceptors/` - Audit interceptor for automatic field updates
  - `Migrations/` - EF Core migrations
- **Identity/**
  - `JwtTokenService.cs` - JWT token generation/validation
  - `PasswordHasher.cs` - BCrypt password hashing
  - `CurrentUserService.cs` - Get current authenticated user
- **Services/**
  - `EmailService.cs` - SMTP email sending
  - `CardnetPaymentService.cs` - Payment gateway integration
  - `PdfGenerationService.cs` - PDF generation with iText
  - `StorageService.cs` - File storage (Azure Blob/Local)

**Dependencies:**

- `Microsoft.EntityFrameworkCore.SqlServer 8.0.0` - SQL Server provider
- `BCrypt.Net-Next 4.0.3` - Password hashing
- `Serilog.AspNetCore 8.0.0` - Structured logging
- `Serilog.Sinks.MSSqlServer 6.5.0` - Log to database

#### 4. Presentation Layer (`MiGenteEnLinea.API`)

**Purpose:** REST API endpoints and HTTP concerns

- **Controllers/**: REST API endpoints
  - `AuthController.cs` - `/api/auth` (register, login, refresh)
  - `EmpleadoresController.cs` - `/api/empleadores`
  - `ContratistasController.cs` - `/api/contratistas`
  - `EmpleadosController.cs` - `/api/empleados`
  - `NominasController.cs` - `/api/nominas`
  - `SuscripcionesController.cs` - `/api/suscripciones`
- **Middleware/**
  - `GlobalExceptionHandlerMiddleware.cs` - Centralized error handling
  - `RequestLoggingMiddleware.cs` - Request/response logging
  - `PerformanceMonitoringMiddleware.cs` - Performance tracking
- **Filters/**
  - `ValidateModelStateFilter.cs` - Automatic model validation
  - `ApiKeyAuthFilter.cs` - API key authentication for external services
- **Extensions/**
  - `ServiceCollectionExtensions.cs` - DI registration
  - `ApplicationBuilderExtensions.cs` - Middleware configuration

**Dependencies:**

- `Microsoft.AspNetCore.Authentication.JwtBearer 8.0.0` - JWT authentication
- `AspNetCoreRateLimit 5.0.0` - Rate limiting
- `Swashbuckle.AspNetCore 6.5.0` - Swagger/OpenAPI documentation

### Authentication & Authorization

#### JWT Token Structure

```json
{
  "nameid": "123",
  "unique_name": "user@example.com",
  "email": "user@example.com",
  "role": "Empleador",
  "PlanID": "5",
  "exp": 1726000000,
  "iss": "MiGenteEnLinea.API",
  "aud": "MiGenteEnLinea.Client"
}
```

#### Authorization Policies

- `RequireEmpleadorRole` - Only Empleadores
- `RequireContratistaRole` - Only Contratistas
- `RequireActivePlan` - Only users with active subscription
- `RequireVerifiedEmail` - Only users with verified email

#### Rate Limiting

- `/api/auth/login` - 5 requests per minute per IP
- `/api/auth/register` - 3 requests per hour per IP
- All other endpoints - 10 requests per second per IP

### Database Access Patterns

#### Code-First with Fluent API

```csharp
// Entity configuration example
public class CredencialConfiguration : IEntityTypeConfiguration<Credencial>
{
    public void Configure(EntityTypeBuilder<Credencial> builder)
    {
        builder.ToTable("Credenciales"); // Maps to existing table

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasColumnName("id");

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("email");

        builder.HasIndex(c => c.Email).IsUnique();
    }
}
```

#### Repository Pattern

```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```

#### CQRS with MediatR

```csharp
// Command
public record RegistrarUsuarioCommand(string Email, string Password) : IRequest<int>;

// Handler
public class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioCommand, int>
{
    public async Task<int> Handle(RegistrarUsuarioCommand request, CancellationToken ct)
    {
        // Business logic
    }
}

// Usage in controller
[HttpPost("register")]
public async Task<IActionResult> Register([FromBody] RegistrarUsuarioCommand command)
{
    var userId = await _mediator.Send(command);
    return Ok(new { userId });
}
```

### Migration Status

#### ✅ Phase 1: Domain Layer - COMPLETADO 100%

- ✅ 36 entidades migradas a Rich Domain Models (24) y Read Models (12)
- ✅ Clean Architecture solution structure creada
- ✅ DbContext configurado con assembly scanning
- ✅ Base classes creadas (AuditableEntity, SoftDeletableEntity, AggregateRoot)
- ✅ Value Objects implementados (~15 value objects)
- ✅ Domain Events creados (~60 events)
- ✅ ~12,053 líneas de código limpio y documentado
- ✅ 0 errores de compilación
- ✅ Reporte: `MiGenteEnLinea.Clean/MIGRATION_100_COMPLETE.md`

#### ✅ Phase 2: Infrastructure Layer - COMPLETADO 100%

- ✅ 9/9 FK relationships validadas y configuradas (paridad 100% con EDMX)
- ✅ 27 Fluent API configurations con constraint names exactos
- ✅ DeleteBehavior correcto para cada relación
- ✅ Shadow properties sin propiedades de navegación (DDD puro)
- ✅ Reporte: `MiGenteEnLinea.Clean/DATABASE_RELATIONSHIPS_REPORT.md`

#### ✅ Phase 3: Application Configuration - COMPLETADO 100%

- ✅ Program.cs completo con Serilog, CORS, Swagger, Health Check
- ✅ DependencyInjection.cs (Infrastructure) con DbContext + Interceptors
- ✅ DependencyInjection.cs (Application) con MediatR, FluentValidation, AutoMapper
- ✅ appsettings.json configurado (Connection strings, JWT, Cardnet, Email)
- ✅ NuGet packages instalados (10 packages totales)
- ✅ API ejecutándose en puerto 5015
- ✅ Swagger UI accesible en http://localhost:5015/
- ✅ Health Check endpoint funcionando en /health
- ✅ Reporte: `MiGenteEnLinea.Clean/PROGRAM_CS_CONFIGURATION_REPORT.md`

#### 🔄 Phase 4: Application Layer (CQRS) - EN PROGRESO

**Estado:** Listo para comenzar implementación  
**Objetivo:** Migrar lógica de negocio desde Legacy Services a CQRS con MediatR

**Servicios Legacy Identificados (12 servicios principales):**

1. **LoginService.asmx.cs** - Autenticación, perfiles, credenciales (10 métodos)
2. **EmpleadosService.cs** - Gestión de empleados, nómina, contrataciones (30+ métodos)
3. **ContratistasService.cs** - Gestión de contratistas y servicios (10 métodos)
4. **CalificacionesService.cs** - Sistema de ratings y reviews (4 métodos)
5. **SuscripcionesService.cs** - Planes, suscripciones, registro (15+ métodos)
6. **PaymentService.cs** - Procesamiento de pagos Cardnet (3 métodos)
7. **EmailService.cs** - Envío de correos (5 métodos)
8. **BotServices.cs** - Asistente virtual OpenAI (3 métodos)
9. **Utilitario.cs** - Funciones auxiliares (5 métodos)
10. Y otros servicios auxiliares

**Prioridad de Implementación (6 LOTES CQRS):**

**LOTE 1 (CRÍTICO):** Authentication & User Management

- LoginCommand, RegisterCommand, ChangePasswordCommand, ResetPasswordCommand
- GetUserByIdQuery, GetUserByEmailQuery, ValidateCredentialsQuery
- **Tiempo estimado:** 8-10 horas
- **Archivos Legacy:** LoginService.asmx.cs, SuscripcionesService.cs (parcial)

**LOTE 2 (ALTA):** Empleadores - CRUD Básico

- CreateEmpleadorCommand, UpdateEmpleadorCommand, DeleteEmpleadorCommand
- GetEmpleadorByIdQuery, GetEmpleadoresQuery, SearchEmpleadoresQuery
- **Tiempo estimado:** 6-8 horas
- **Archivos Legacy:** Empleador/\*.aspx.cs (páginas de empleador)

**LOTE 3 (ALTA):** Contratistas - CRUD + Búsqueda

- CreateContratistaCommand, UpdateContratistaCommand, ActivarPerfilCommand
- GetContratistaByIdQuery, SearchContratistasQuery, GetServiciosQuery
- AddServicioCommand, RemoveServicioCommand
- **Tiempo estimado:** 8-10 horas
- **Archivos Legacy:** ContratistasService.cs

**LOTE 4 (MEDIA):** Empleados y Nómina - CRUD + Procesamiento

- CreateEmpleadoCommand, UpdateEmpleadoCommand, DarDeBajaCommand
- ProcesarPagoCommand, ProcesarPagoContratacionCommand
- GetEmpleadosQuery, GetRecibosQuery, GetDeduccionesQuery
- **Tiempo estimado:** 12-15 horas
- **Archivos Legacy:** EmpleadosService.cs (métodos más complejos)

**LOTE 5 (MEDIA):** Suscripciones y Pagos

- CreateSuscripcionCommand, UpdateSuscripcionCommand, ProcesarVentaCommand
- ProcessPaymentCommand (Cardnet integration)
- GetPlanesQuery, GetSuscripcionQuery, GetVentasQuery
- **Tiempo estimado:** 10-12 horas
- **Archivos Legacy:** SuscripcionesService.cs, PaymentService.cs

**LOTE 6 (BAJA):** Calificaciones y Extras

- CreateCalificacionCommand, UpdateCalificacionCommand
- GetCalificacionesQuery, GetPromedioQuery
- EnviarEmailCommand (EmailService)
- ConsultarPadronCommand (API externa)
- **Tiempo estimado:** 6-8 horas
- **Archivos Legacy:** CalificacionesService.cs, EmailService.cs

**Metodología OBLIGATORIA (CRITICAL):**

⚠️ **REGLA #1:** Antes de implementar CUALQUIER Command/Query, SIEMPRE leer el método correspondiente en Legacy para copiar la lógica exacta.

1. ✅ **LEER controlador/servicio en Legacy** (SIEMPRE PRIMERO)
2. ✅ Identificar métodos públicos (lógica de negocio)
3. ✅ Crear Command o Query según operación (Write/Read)
4. ✅ Implementar Handler con **lógica EXACTAMENTE igual al Legacy**
5. ✅ Crear Validator con FluentValidation (validar inputs)
6. ✅ Crear DTO para request/response (AutoMapper)
7. ✅ Crear Controller REST API endpoint
8. ✅ Probar con Swagger UI
9. ✅ Documentar en `LOTE_X_CQRS_COMPLETADO.md`

#### ⏳ Phase 5: REST API Controllers - PENDIENTE

- Implementar Controllers consumiendo Commands/Queries
- JWT authentication middleware
- Global exception handler
- API versioning
- Rate limiting

#### ⏳ Phase 6: Testing - PENDIENTE

- Unit tests para Domain entities
- Unit tests para Handlers
- Integration tests para Controllers
- Unit tests para Validators
- Cobertura objetivo: 80%+

#### ⏳ Phase 7: Security & Deployment - PENDIENTE

- Migrar plain text passwords a BCrypt
- Implementar refresh tokens
- Configurar CI/CD pipeline
- Performance testing
- Security audit validation

## Project Structure

### Master Pages (Role-Based Layouts)

- `Platform.Master`: Base layout for public/general pages
- `Comunity1.Master`: Empleador dashboard layout (checks `tipo = "1"`)
- `ContratistaM.Master`: Contratista dashboard layout (checks `tipo = "2"`)
- **Plan enforcement**: Both master pages redirect to subscription purchase if `planID = "0"` or plan is expired

### Key Directories

```
MiGente_Front/
├── Contratista/          # Contractor-specific pages
│   ├── index_contratista.aspx
│   ├── AdquirirPlanContratista.aspx
│   └── MisCalificaciones.aspx
├── Empleador/            # Employer-specific pages
│   ├── colaboradores.aspx
│   ├── nomina.aspx
│   ├── fichaEmpleado.aspx
│   ├── Checkout.aspx
│   └── Impresion/        # Print templates for contracts/receipts
├── Data/                 # Entity Framework models (auto-generated from EDMX)
│   ├── DataModel.edmx
│   └── [Entity classes].cs
├── Services/             # Business logic & API services
│   ├── LoginService.cs
│   ├── EmailService.cs
│   ├── PaymentService.cs
│   ├── BotServices.cs (OpenAI integration)
│   └── *.asmx (SOAP web services)
├── UserControls/         # Reusable ASCX components
├── HtmlTemplates/        # Static HTML content (terms, authorizations)
└── MailTemplates/        # Email templates (HTML)
```

### Database Connection

```xml
<!-- Web.config -->
<connectionStrings>
  <add name="migenteEntities"
       connectionString="metadata=res://*/Data.DataModel.csdl|...;
       provider=System.Data.SqlClient;
       provider connection string='data source=.;initial catalog=migenteV2;
       user id=sa;password=1234;...'"
       providerName="System.Data.EntityClient"/>
</connectionStrings>
```

**Note**: Connection uses SQL Server on localhost (`.`) with hardcoded credentials.

### Payment Integration (Cardnet)

```xml
<appSettings>
  <add key="CardnetMerchantId" value="349000001"/>
  <add key="CardnetApiKey" value="TU_API_KEY"/>
  <add key="CardnetApiUrlSales" value="https://ecommerce.cardnet.com.do/api/payment/transactions/sales"/>
  <add key="CardnetApiUrlIdempotency" value="https://ecommerce.cardnet.com.do/api/payment/idenpotency-keys"/>
</appSettings>
```

## Critical Workflows

### User Registration & Activation

1. User registers via `Registrar.aspx` → creates `Credenciales` + `Ofertantes`/`Contratistas` record
2. Activation email sent with URL: `activarperfil.aspx?userID={id}&email={email}`
3. User activates account → sets `Activo = true` in database
4. First login redirects to subscription purchase if no plan

### Subscription Management

- Plans stored in `Planes_empleadores` / `Planes_Contratistas` tables
- Subscription data in `Suscripciones` table (with `FechaVencimiento`)
- Master pages enforce active subscription before page access
- Checkout flow: `AdquirirPlan*.aspx` → `Checkout.aspx` → Cardnet payment → update subscription

### Payroll & Document Generation

- Employers create employees in `Empleados` table
- Payroll generation creates `Empleador_Recibos_Header` + `Empleador_Recibos_Detalle`
- TSS (social security) deductions calculated via `Deducciones_TSS` table
- PDF generation using iText: contracts (`ContratoPersonaFisica.html`), receipts in `Empleador/Impresion/`

## Development Conventions

### Code-Behind Pattern

All `.aspx` pages follow the standard Web Forms pattern:

```csharp
namespace MiGente_Front
{
    public partial class PageName : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { /* initialization */ }
        }
    }
}
```

### Service Layer Pattern

Services are instantiated in code-behind, not via dependency injection:

```csharp
LoginService service = new LoginService();
var result = service.login(username, password);
```

### SweetAlert for User Feedback

All user messages use SweetAlert2 via `ClientScript.RegisterStartupScript`:

```csharp
string script = @"<script>
    Swal.fire({
        title: 'Título',
        text: 'Mensaje',
        icon: 'success|error|warning|info',
        confirmButtonText: 'Aceptar'
    });
</script>";
ClientScript.RegisterStartupScript(GetType(), "SweetAlert", script);
```

### Session & Cookie Management

- Session cleared on logout: `Session.Clear(); Session.Abandon();`
- Forms authentication: `FormsAuthentication.SignOut();`
- Cookie access: `HttpCookie myCookie = Request.Cookies["login"];`

## Build & Run

### Prerequisites

- Visual Studio 2017+ (solution targets VS 17.6)
- IIS Express configured
- SQL Server with `migenteV2` database
- DevExpress v23.1 license (commercial component)

### Build Configuration

```bash
# Debug build
msbuild MiGente.sln /p:Configuration=Debug

# Publish to Azure/IIS (Web Deploy configured in Properties/PublishProfiles/)
```

### Local Development URL

- **HTTPS**: `https://localhost:44358/`
- **Start page**: `Login.aspx`

## Important Notes for AI Agents

### Do NOT Modify

- Entity Framework EDMX and auto-generated model classes in `Data/`
- DevExpress control configurations (proprietary markup)
- Payment gateway integration endpoints
- Database connection strings without explicit approval

### External Dependencies Reference

- **ClassLibrary CSharp.dll**: External utility library at `..\..\Utility_Suite\Utility_POS\Utility_POS\bin\Debug\` (not in repository)
- DevExpress assemblies: Requires valid license for development

### Security Considerations

⚠️ **CRITICAL VULNERABILITIES IDENTIFIED (Sept 2025 Audit)**:

#### 🔴 CRITICAL - Fix Immediately

1. **SQL Injection**: Multiple instances of SQL string concatenation in controllers and services
2. **Plain Text Passwords**: Passwords stored without hashing in database
3. **Missing Authentication**: Critical endpoints accessible without authentication
4. **Information Disclosure**: Detailed error messages with stack traces exposed to clients
5. **Hardcoded Credentials**: Database credentials and API keys in Web.config

#### 🟡 HIGH - Address This Sprint

6. **Permissive CORS**: Allow-all CORS policy in production
7. **No Rate Limiting**: Brute force attacks possible on login endpoints
8. **Missing Input Validation**: No systematic validation framework
9. **No Audit Logging**: Security events not logged
10. **Session Management**: Insecure cookie configuration

#### 🟢 MEDIUM - Address in Next Sprint

11. **CSRF Protection**: Forms lack anti-forgery tokens
12. **Missing HTTPS Enforcement**: HTTP not redirected to HTTPS
13. **Weak Password Policy**: No password complexity requirements
14. **No API Versioning**: Breaking changes risk
15. **Large Attack Surface**: Monolithic architecture

### 🚫 MANDATORY SECURITY RULES FOR AI AGENTS

**NEVER DO (Will be rejected in code review)**:

```csharp
// ❌ SQL Injection vulnerability
string query = $"SELECT * FROM Users WHERE Username = '{username}'";

// ❌ Plain text passwords
usuario.Password = password;

// ❌ Missing authentication
[HttpGet]
public ActionResult GetSensitiveData() { }

// ❌ Exposing errors
catch (Exception ex) {
    return Json(new { error = ex.Message, stack = ex.StackTrace });
}
```

**ALWAYS DO (Required pattern)**:

```csharp
// ✅ Parameterized queries / Entity Framework
var user = await _context.Users
    .Where(u => u.Username == username)
    .FirstOrDefaultAsync();

// ✅ Password hashing (BCrypt work factor 12)
string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, 12);
bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

// ✅ Authentication required
[Authorize(Roles = "Empleador,Contratista")]
[HttpGet]
public ActionResult GetSensitiveData() { }

// ✅ Safe error handling
catch (Exception ex) {
    _logger.LogError(ex, "Error in operation");
    return Json(new { error = "An error occurred processing your request" });
}
```

### Testing Strategy

- No unit tests currently exist in solution
- Manual testing required for all changes
- Test with both user types (Empleador and Contratista)
- Verify subscription enforcement on protected pages

## Domain-Specific Terms (Dominican Context)

- **TSS**: Tesorería de la Seguridad Social (Social Security Treasury)
- **RNC/Cédula**: Tax ID / National ID numbers
- **Padrón**: National registry/database
- **Recibo de pago**: Payment receipt
- **Nómina**: Payroll
- **Colaborador**: Employee/collaborator

## 🏗️ Migration to Clean Architecture (Target State)

### Proposed Architecture Structure

```
MiGenteEnLinea/
├── src/
│   ├── Core/
│   │   ├── MiGenteEnLinea.Domain/              # Entities, Value Objects, Interfaces
│   │   │   ├── Entities/
│   │   │   │   ├── Usuario.cs
│   │   │   │   ├── Empleador.cs
│   │   │   │   ├── Contratista.cs
│   │   │   │   ├── Empleado.cs
│   │   │   │   ├── Nomina.cs
│   │   │   │   └── Suscripcion.cs
│   │   │   ├── ValueObjects/
│   │   │   ├── Enums/
│   │   │   └── Interfaces/
│   │   │       ├── IRepository.cs
│   │   │       └── IUnitOfWork.cs
│   │   │
│   │   └── MiGenteEnLinea.Application/         # Use Cases, DTOs, Validators
│   │       ├── Common/
│   │       │   ├── Interfaces/
│   │       │   ├── Behaviors/
│   │       │   └── Exceptions/
│   │       ├── Features/
│   │       │   ├── Authentication/
│   │       │   │   ├── Commands/
│   │       │   │   │   ├── LoginCommand.cs
│   │       │   │   │   └── RegisterCommand.cs
│   │       │   │   ├── Queries/
│   │       │   │   ├── DTOs/
│   │       │   │   └── Validators/
│   │       │   ├── Empleadores/
│   │       │   ├── Contratistas/
│   │       │   ├── Empleados/
│   │       │   └── Nominas/
│   │       └── DependencyInjection.cs
│   │
│   ├── Infrastructure/
│   │   ├── MiGenteEnLinea.Infrastructure/      # EF Core, Identity, External Services
│   │   │   ├── Persistence/
│   │   │   │   ├── Contexts/
│   │   │   │   │   └── ApplicationDbContext.cs
│   │   │   │   ├── Configurations/
│   │   │   │   │   ├── UsuarioConfiguration.cs
│   │   │   │   │   └── EmpleadoConfiguration.cs
│   │   │   │   ├── Repositories/
│   │   │   │   └── Migrations/
│   │   │   ├── Identity/
│   │   │   │   ├── IdentityService.cs
│   │   │   │   └── JwtTokenService.cs
│   │   │   ├── Services/
│   │   │   │   ├── EmailService.cs
│   │   │   │   ├── CardnetPaymentService.cs
│   │   │   │   └── PdfGenerationService.cs
│   │   │   └── DependencyInjection.cs
│   │   │
│   │   └── MiGenteEnLinea.Shared/              # Cross-cutting concerns
│   │       ├── Extensions/
│   │       ├── Helpers/
│   │       └── Constants/
│   │
│   └── Presentation/
│       └── MiGenteEnLinea.API/                 # ASP.NET Core Web API
│           ├── Controllers/
│           │   ├── AuthController.cs
│           │   ├── EmpleadoresController.cs
│           │   ├── ContratistasController.cs
│           │   └── NominasController.cs
│           ├── Middleware/
│           │   ├── GlobalExceptionHandlerMiddleware.cs
│           │   └── RequestLoggingMiddleware.cs
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

### Migration Phases

#### Phase 1: Security Remediation (Weeks 1-2) - CRITICAL

- [ ] Implement BCrypt password hashing for all user authentication
- [ ] Replace all SQL concatenation with Entity Framework queries
- [ ] Add `[Authorize]` attributes to all protected endpoints
- [ ] Implement global exception handling middleware
- [ ] Move secrets to User Secrets / Azure Key Vault
- [ ] Configure secure CORS policies
- [ ] Add rate limiting to authentication endpoints

#### Phase 2: Foundation Setup (Week 3)

- [ ] Create Clean Architecture solution structure
- [ ] Setup Entity Framework Core Code-First
- [ ] Create domain entities with proper encapsulation
- [ ] Implement repository pattern and unit of work
- [ ] Configure dependency injection

#### Phase 3: Application Layer (Week 4)

- [ ] Implement CQRS with MediatR
- [ ] Create Commands and Queries for all operations
- [ ] Add FluentValidation for all inputs
- [ ] Implement AutoMapper for DTOs
- [ ] Add logging with Serilog

#### Phase 4: Authentication & Authorization (Week 5)

- [ ] Implement JWT authentication
- [ ] Add refresh token mechanism
- [ ] Configure role-based authorization
- [ ] Implement policy-based authorization
- [ ] Add multi-factor authentication (future)

#### Phase 5: Testing & Documentation (Week 6)

- [ ] Write unit tests (80%+ coverage target)
- [ ] Create integration tests for critical paths
- [ ] Security testing (OWASP validation)
- [ ] API documentation with Swagger
- [ ] Performance testing

### Required NuGet Packages for Migration

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

## 🎯 AI Agent Checklist - Before ANY Code Change

**Security Validation** (Must answer YES to all):

- [ ] Does this change eliminate SQL injection risks?
- [ ] Are passwords properly hashed (BCrypt work factor 12+)?
- [ ] Are all endpoints properly authenticated/authorized?
- [ ] Is input validated using FluentValidation?
- [ ] Are errors handled without exposing sensitive information?
- [ ] Are security events properly logged?
- [ ] Is this change following OWASP best practices?

**Architecture Validation**:

- [ ] Does this follow Clean Architecture principles?
- [ ] Is dependency injection used properly?
- [ ] Are domain entities properly encapsulated?
- [ ] Is separation of concerns maintained?
- [ ] Are interfaces used for abstraction?

**Code Quality**:

- [ ] Is the code testable?
- [ ] Are there unit tests for new functionality?
- [ ] Is documentation updated?
- [ ] Does code follow C# naming conventions?
- [ ] Are there no hardcoded values?

## 📚 Essential Resources

### Security References

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [OWASP Cheat Sheet Series](https://cheatsheetseries.owasp.org/)
- [Microsoft Security Best Practices](https://docs.microsoft.com/en-us/aspnet/core/security/)

### Architecture References

- [Clean Architecture - Jason Taylor](https://github.com/jasontaylordev/CleanArchitecture)
- [Clean Architecture - Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Domain-Driven Design](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/)

### Implementation Patterns

- [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [Repository Pattern](https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design)
- [JWT Authentication in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)

## Quick Reference: Key Files

- `Login.aspx.cs`: Authentication entry point
- `Comunity1.Master.cs`: Empleador session/plan validation
- `ContratistaM.Master.cs`: Contratista session/plan validation
- `Web.config`: All configuration (DB, APIs, DevExpress)
- `NumeroEnLetras.cs`: Number-to-words conversion (for legal documents)

## 🔧 Code Examples - Security Fixes

### Example 1: Fixing SQL Injection in LoginService

**BEFORE (Vulnerable)**:

```csharp
public class LoginService
{
    public Usuario Login(string username, string password)
    {
        string query = $"SELECT * FROM Usuarios WHERE Username = '{username}' AND Password = '{password}'";
        // Execute raw SQL...
    }
}
```

**AFTER (Secure)**:

```csharp
public class LoginService
{
    private readonly migenteEntities _context;
    private readonly IPasswordHasher _passwordHasher;

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var usuario = await _context.Usuarios
            .Include(u => u.Rol)
            .Where(u => u.Username == username && u.Activo)
            .FirstOrDefaultAsync();

        if (usuario == null || !_passwordHasher.VerifyPassword(password, usuario.PasswordHash))
        {
            _logger.LogWarning("Failed login attempt for username: {Username}", username);
            return LoginResult.Failed("Credenciales inválidas");
        }

        _logger.LogInformation("Successful login for user: {UserId}", usuario.Id);
        return LoginResult.Success(usuario);
    }
}
```

### Example 2: Implementing Password Hashing

**Password Hasher Service**:

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
            throw new ArgumentException("Password cannot be empty", nameof(password));

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

### Example 3: Global Exception Handler Middleware

```csharp
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation error occurred");
            await HandleValidationExceptionAsync(context, ex);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access attempt");
            await HandleUnauthorizedAccessAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Ha ocurrido un error procesando su solicitud",
            requestId = Activity.Current?.Id ?? context.TraceIdentifier
        };

        return context.Response.WriteAsJsonAsync(response);
    }

    private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "Error de validación",
            errors = exception.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
        };

        return context.Response.WriteAsJsonAsync(response);
    }

    private static Task HandleUnauthorizedAccessAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var response = new { message = "No autorizado" };
        return context.Response.WriteAsJsonAsync(response);
    }
}
```

### Example 4: FluentValidation for Input

```csharp
public class RegistrarUsuarioCommand
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string TipoUsuario { get; set; } // "Empleador" or "Contratista"
}

public class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    public RegistrarUsuarioCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es requerido")
            .Length(3, 50).WithMessage("El nombre de usuario debe tener entre 3 y 50 caracteres")
            .Matches("^[a-zA-Z0-9_]+$").WithMessage("El nombre de usuario solo puede contener letras, números y guión bajo");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido")
            .EmailAddress().WithMessage("El correo electrónico no es válido")
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$")
            .WithMessage("La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(100);

        RuleFor(x => x.Apellido)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(100);

        RuleFor(x => x.TipoUsuario)
            .NotEmpty()
            .Must(x => x == "Empleador" || x == "Contratista")
            .WithMessage("El tipo de usuario debe ser 'Empleador' o 'Contratista'");
    }
}
```

### Example 5: JWT Token Generation

```csharp
public class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Usuario usuario)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.TipoUsuario), // "Empleador" or "Contratista"
            new Claim("PlanID", usuario.PlanID?.ToString() ?? "0"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public RefreshToken GenerateRefreshToken(int userId)
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            UsuarioId = userId,
            ExpiryDate = DateTime.UtcNow.AddDays(7),
            CreatedDate = DateTime.UtcNow
        };
    }
}
```

### Example 6: Rate Limiting Configuration

```csharp
// appsettings.json
{
  "IpRateLimiting": {
    "EnableEndpointRateLimiting": true,
    "StackBlockedRequests": false,
    "RealIpHeader": "X-Real-IP",
    "ClientIdHeader": "X-ClientId",
    "HttpStatusCode": 429,
    "GeneralRules": [
      {
        "Endpoint": "POST:/api/auth/login",
        "Period": "1m",
        "Limit": 5
      },
      {
        "Endpoint": "POST:/api/auth/register",
        "Period": "1h",
        "Limit": 3
      },
      {
        "Endpoint": "*",
        "Period": "1s",
        "Limit": 10
      }
    ]
  }
}

// Program.cs
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// In middleware pipeline
app.UseIpRateLimiting();
```

## 🎯 Implementation Priorities

### Sprint 1 (Week 1-2): Critical Security Fixes

1. **Password Security**

   - Install BCrypt.Net-Next NuGet package
   - Implement IPasswordHasher service
   - Create migration script to hash existing passwords
   - Update all registration/password change logic

2. **SQL Injection Prevention**

   - Audit all Services/\*.cs files for SQL concatenation
   - Replace with Entity Framework LINQ queries
   - Add code analysis rule to prevent future violations

3. **Authentication & Authorization**
   - Install JWT packages
   - Implement JwtTokenService
   - Add [Authorize] attributes to all controllers
   - Implement role-based authorization

### Sprint 2 (Week 3-4): Architecture Foundation

1. **Project Structure**

   - Create Clean Architecture solution
   - Setup Domain, Application, Infrastructure, API projects
   - Configure project dependencies

2. **Entity Framework Code-First**
   - Create domain entities
   - Add fluent configurations
   - Generate initial migration from existing database
   - Test migration rollback/reapply

### Sprint 3 (Week 5-6): Advanced Features & Testing

1. **CQRS Implementation**

   - Install MediatR
   - Create Commands and Queries
   - Implement handlers

2. **Testing**
   - Unit tests for domain logic
   - Integration tests for API endpoints
   - Security tests (OWASP validation)

---

_Last updated: 2025-10-12_
_Based on Security Audit: September 2025_
_For questions about business logic or specific features, consult the project owner before making assumptions._
