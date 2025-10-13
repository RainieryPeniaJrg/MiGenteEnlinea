# 🚀 APPLICATION LAYER (CQRS) - IMPLEMENTACIÓN POR LOTES# 🚀 PLAN MAESTRO: Implementación de Application Layer (CQRS con MediatR)



**Fecha de Creación:** 12 de octubre, 2025  **Fecha de Creación:** 12 de octubre, 2025  

**Proyecto:** MiGente En Línea - Clean Architecture Migration  **Estado:** ⏳ **PENDIENTE - LISTO PARA EJECUTAR**  

**Fase:** Phase 4 - Application Layer (CQRS con MediatR)**Prerequisitos:** ✅ Domain Layer (36 entidades), ✅ Infrastructure (relaciones), ✅ Program.cs configurado



------



## 🎯 OBJETIVO PRINCIPAL## 🎯 OBJETIVO GENERAL



Migrar **TODA** la lógica de negocio de los servicios legacy (`.asmx.cs`, `*Service.cs`) a **CQRS** usando **MediatR**, manteniendo la **lógica EXACTAMENTE igual** al sistema legacy.Migrar la lógica de negocio desde los **22 servicios Legacy** (Web Forms) a **Application Layer** usando **CQRS** con **MediatR**. La lógica de negocio debe ser **EXACTAMENTE IDÉNTICA** al proyecto Legacy para garantizar compatibilidad funcional.



⚠️ **REGLA CRÍTICA:** Antes de implementar cualquier Command/Query, SIEMPRE leer el método correspondiente en Legacy para copiar la lógica exacta (validaciones, cálculos, reglas de negocio, excepciones).### ⚠️ REGLA CRÍTICA: PARIDAD 100% CON LEGACY



---```

ANTES de implementar cualquier Command/Query/Handler:

## 📊 SERVICIOS LEGACY ANALIZADOS

1. LEE el servicio/controlador Legacy correspondiente

### 1. LoginService.asmx.cs (10 métodos) - LOTE 12. IDENTIFICA el método exacto y su lógica

### 2. EmpleadosService.cs (30+ métodos) - LOTE 43. ANALIZA los parámetros de entrada y salida

### 3. ContratistasService.cs (10 métodos) - LOTE 34. REPLICA la lógica EXACTAMENTE (mismos pasos, mismas validaciones, mismo orden)

### 4. SuscripcionesService.cs (15+ métodos) - LOTE 55. USA las mismas queries EF Core (ajustadas a DbContext moderno)

### 5. PaymentService.cs (3 métodos) - LOTE 56. MANTÉN los mismos nombres de campos en DTOs

### 6. CalificacionesService.cs (4 métodos) - LOTE 67. RESPETA los mismos códigos de retorno y mensajes de error

```

Ver archivo completo en el prompt para detalles de cada servicio.

**NO inventes lógica nueva. NO mejores la lógica sin autorización explícita. COPIA el comportamiento Legacy al 100%.**

---

---

## 📦 LOTE 1: AUTHENTICATION & USER MANAGEMENT

## 📊 INVENTARIO DE SERVICIOS LEGACY

**Prioridad:** 🔴 CRÍTICA  

**Tiempo Estimado:** 8-10 horas### Servicios Identificados (22 total)



### Servicios Legacy:| # | Servicio Legacy | Ubicación | Métodos Públicos | Prioridad | Lote |

- LoginService.asmx.cs|---|-----------------|-----------|------------------|-----------|------|

- SuscripcionesService.cs (registro)| 1 | **LoginService.asmx.cs** | Services/ | 11 métodos | 🔴 CRÍTICA | LOTE 1 |

| 2 | **EmpleadosService.cs** | Services/ | 15 métodos | 🟠 ALTA | LOTE 4 |

### Commands & Queries a Implementar:| 3 | **ContratistasService.cs** | Services/ | 10 métodos | 🟠 ALTA | LOTE 3 |

| 4 | **CalificacionesService.cs** | Services/ | 6 métodos | 🟡 MEDIA | LOTE 5 |

Ver archivo completo en el workspace.| 5 | **SuscripcionesService.cs** | Services/ | 8 métodos | 🟡 MEDIA | LOTE 5 |

| 6 | **PaymentService.cs** | Services/ | 5 métodos | 🟡 MEDIA | LOTE 5 |

---| 7 | **EmailService.cs** | Services/ | 4 métodos | 🟢 BAJA | LOTE 6 |

| 8 | **BotServices.cs** | Services/ | 3 métodos | 🟢 BAJA | LOTE 7 |

## 🚀 COMANDO DE EJECUCIÓN| 9-22 | Otros servicios auxiliares | Services/ | Variable | 🟢 BAJA | LOTE 6-7 |



```bash---

@workspace Lee prompts/APPLICATION_LAYER_CQRS_IMPLEMENTATION.md

## 🎯 ORGANIZACIÓN POR LOTES (CQRS)

EJECUTAR: LOTE 1 - AUTHENTICATION & USER MANAGEMENT

### ⚠️ METODOLOGÍA POR LOTE

METODOLOGÍA OBLIGATORIA:

1. ✅ LEER Legacy PRIMERO (LoginService.asmx.cs)Cada LOTE sigue este workflow estricto:

2. ✅ Implementar Command + Handler + Validator

3. ✅ Copiar lógica EXACTA del Legacy```

4. ✅ Crear DTOs con AutoMapperPARA CADA SERVICIO EN EL LOTE:

5. ✅ Crear Controller con documentación

6. ✅ Probar con Swagger1. 📖 LEER SERVICIO LEGACY

7. ✅ Documentar en LOTE_1_COMPLETADO.md   - Ubicar archivo en Codigo Fuente Mi Gente/MiGente_Front/Services/

   - Identificar TODOS los métodos públicos

COMENZAR EJECUCIÓN AUTOMÁTICA.   - Anotar firma (parámetros, retorno)

```   - Entender la lógica paso a paso



---2. 📝 CLASIFICAR OPERACIONES

   - COMMAND: Si modifica datos (INSERT, UPDATE, DELETE)

**Estado:** Listo para ejecución ✅   - QUERY: Si solo lee datos (SELECT)


3. ✏️ CREAR COMMAND/QUERY
   Ubicación: src/Core/MiGenteEnLinea.Application/Features/[Módulo]/Commands o Queries/
   
   Ejemplo: LoginCommand.cs
   ```csharp
   public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
   ```

4. 🛠️ CREAR HANDLER
   Ubicación: Mismo folder que Command/Query
   
   Ejemplo: LoginCommandHandler.cs
   ```csharp
   public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
   {
       private readonly MiGenteDbContext _context;
       private readonly IPasswordHasher _passwordHasher;
       
       public LoginCommandHandler(MiGenteDbContext context, IPasswordHasher passwordHasher)
       {
           _context = context;
           _passwordHasher = passwordHasher;
       }
       
       public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
       {
           // AQUÍ: Copiar lógica EXACTA del método login() en LoginService.asmx.cs
           
           // Ejemplo de paridad:
           // LEGACY: var result = db.Credenciales.Where(x => x.email == email && x.password == crypted).FirstOrDefault();
           // CLEAN:  var credencial = await _context.Credenciales.Where(x => x.Email == request.Email).FirstOrDefaultAsync(ct);
           
           // IMPORTANTE: Mantener el mismo orden de operaciones, mismas validaciones, mismos códigos de retorno
       }
   }
   ```

5. ✅ CREAR VALIDATOR
   Ubicación: src/Core/MiGenteEnLinea.Application/Features/[Módulo]/Validators/
   
   Ejemplo: LoginCommandValidator.cs
   ```csharp
   public class LoginCommandValidator : AbstractValidator<LoginCommand>
   {
       public LoginCommandValidator()
       {
           RuleFor(x => x.Email)
               .NotEmpty().WithMessage("El correo electrónico es requerido")
               .EmailAddress().WithMessage("El correo electrónico no es válido");
               
           RuleFor(x => x.Password)
               .NotEmpty().WithMessage("La contraseña es requerida")
               .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");
       }
   }
   ```

6. 📦 CREAR DTOs
   Ubicación: src/Core/MiGenteEnLinea.Application/Features/[Módulo]/DTOs/
   
   Ejemplo: LoginResult.cs
   ```csharp
   public class LoginResult
   {
       public int StatusCode { get; set; } // 2=success, 0=invalid, -1=inactive (IGUAL AL LEGACY)
       public string? UserId { get; set; }
       public string? Email { get; set; }
       public string? Nombre { get; set; }
       public int? Tipo { get; set; } // 1=Empleador, 2=Contratista
       public int? PlanId { get; set; }
       public DateTime? VencimientoPlan { get; set; }
   }
   ```

7. 🌐 CREAR CONTROLLER ENDPOINT
   Ubicación: src/Presentation/MiGenteEnLinea.API/Controllers/
   
   Ejemplo: AuthController.cs
   ```csharp
   [ApiController]
   [Route("api/[controller]")]
   public class AuthController : ControllerBase
   {
       private readonly IMediator _mediator;
       
       public AuthController(IMediator mediator)
       {
           _mediator = mediator;
       }
       
       [HttpPost("login")]
       public async Task<ActionResult<LoginResult>> Login([FromBody] LoginCommand command)
       {
           var result = await _mediator.Send(command);
           
           if (result.StatusCode == 0)
               return Unauthorized(new { message = "Credenciales inválidas" });
           
           if (result.StatusCode == -1)
               return Unauthorized(new { message = "Cuenta inactiva" });
               
           return Ok(result);
       }
   }
   ```

8. 🧪 VALIDAR COMPILACIÓN
   ```bash
   dotnet build --no-restore
   # Esperado: Build succeeded. 0 Error(s)
   ```

9. 📄 DOCUMENTAR
   - Crear LOTE_X_[MODULO]_COMPLETADO.md
   - Listar Commands/Queries implementados
   - Comparar con Legacy (tabla de paridad)
   - Notas de diferencias (si las hay)
```

---

## 🔴 LOTE 1: AUTHENTICATION (CRÍTICO)

**Prioridad:** 🔴 **MÁXIMA - COMENZAR AQUÍ**  
**Servicio Legacy:** `LoginService.asmx.cs`  
**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/LoginService.asmx.cs`  
**Duración Estimada:** 4-6 horas  
**Estado:** ⏳ PENDIENTE

### Métodos a Migrar (11 total)

| # | Método Legacy | Operación | Command/Query | Descripción | Prioridad |
|---|---------------|-----------|---------------|-------------|-----------|
| 1 | `login(string email, string pass)` | READ/WRITE | **LoginCommand** | Autenticación + cookie | 🔴 CRÍTICA |
| 2 | `obtenerPerfil(string userID)` | READ | **GetPerfilQuery** | Obtener perfil VPerfiles | 🟠 ALTA |
| 3 | `obtenerPerfilByEmail(string email)` | READ | **GetPerfilByEmailQuery** | Buscar perfil por email | 🟠 ALTA |
| 4 | `getPerfilByID(int cuentaID)` | READ | **GetPerfilByIdQuery** | Obtener Cuenta por ID | 🟠 ALTA |
| 5 | `getPerfilInfo(Guid userID)` | READ | **GetPerfilInfoQuery** | Obtener VPerfiles por GUID | 🟡 MEDIA |
| 6 | `obtenerCredenciales(string userID)` | READ | **GetCredencialesQuery** | Lista credenciales user | 🟡 MEDIA |
| 7 | `validarCorreo(string correo)` | READ | **ValidarCorreoQuery** | Check email exists | 🟡 MEDIA |
| 8 | `actualizarPerfil(perfilesInfo, Cuentas)` | WRITE | **ActualizarPerfilCommand** | Update perfil completo | 🟡 MEDIA |
| 9 | `actualizarPerfil1(Cuentas cuenta)` | WRITE | **ActualizarCuentaCommand** | Update solo Cuenta | 🟡 MEDIA |
| 10 | `agregarPerfilInfo(perfilesInfo info)` | WRITE | **AgregarPerfilInfoCommand** | Agregar perfilesInfo | 🟢 BAJA |
| 11 | `borrarUsuario(string userID, int credencialID)` | WRITE | **BorrarUsuarioCommand** | Eliminar usuario | 🟢 BAJA |

### Estructura de Archivos LOTE 1

```
src/Core/MiGenteEnLinea.Application/Features/Authentication/
├── Commands/
│   ├── Login/
│   │   ├── LoginCommand.cs
│   │   ├── LoginCommandHandler.cs
│   │   └── LoginCommandValidator.cs
│   ├── ActualizarPerfil/
│   │   ├── ActualizarPerfilCommand.cs
│   │   ├── ActualizarPerfilCommandHandler.cs
│   │   └── ActualizarPerfilCommandValidator.cs
│   ├── ActualizarCuenta/
│   │   ├── ActualizarCuentaCommand.cs
│   │   └── ActualizarCuentaCommandHandler.cs
│   ├── AgregarPerfilInfo/
│   │   ├── AgregarPerfilInfoCommand.cs
│   │   └── AgregarPerfilInfoCommandHandler.cs
│   └── BorrarUsuario/
│       ├── BorrarUsuarioCommand.cs
│       └── BorrarUsuarioCommandHandler.cs
├── Queries/
│   ├── GetPerfil/
│   │   ├── GetPerfilQuery.cs
│   │   └── GetPerfilQueryHandler.cs
│   ├── GetPerfilByEmail/
│   │   ├── GetPerfilByEmailQuery.cs
│   │   └── GetPerfilByEmailQueryHandler.cs
│   ├── GetPerfilById/
│   │   ├── GetPerfilByIdQuery.cs
│   │   └── GetPerfilByIdQueryHandler.cs
│   ├── GetPerfilInfo/
│   │   ├── GetPerfilInfoQuery.cs
│   │   └── GetPerfilInfoQueryHandler.cs
│   ├── GetCredenciales/
│   │   ├── GetCredencialesQuery.cs
│   │   └── GetCredencialesQueryHandler.cs
│   └── ValidarCorreo/
│       ├── ValidarCorreoQuery.cs
│       └── ValidarCorreoQueryHandler.cs
├── DTOs/
│   ├── LoginResult.cs
│   ├── PerfilDto.cs
│   ├── CuentaDto.cs
│   └── CredencialDto.cs
└── Validators/
    └── (validators ya incluidos en carpetas de Commands)

src/Presentation/MiGenteEnLinea.API/Controllers/
└── AuthController.cs (11 endpoints)
```

### Ejemplo Completo: LoginCommand

#### 1. LoginCommand.cs
```csharp
namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
```

#### 2. LoginCommandHandler.cs
```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    private readonly MiGenteDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(MiGenteDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // LÓGICA COPIADA DE: LoginService.asmx.cs -> login(string email, string pass)
        
        // 1. Buscar credencial por email
        var credencial = await _context.Credenciales
            .Where(x => x.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (credencial == null)
            return new LoginResult { StatusCode = 0 }; // Credenciales inválidas

        // 2. Verificar password (BCrypt en lugar de Crypt legacy)
        if (!_passwordHasher.VerifyPassword(request.Password, credencial.PasswordHash))
            return new LoginResult { StatusCode = 0 }; // Credenciales inválidas

        // 3. Verificar si está activo
        if (!credencial.Activo)
            return new LoginResult { StatusCode = -1 }; // Cuenta inactiva

        // 4. Obtener datos de cuenta
        var cuenta = await _context.Cuentas
            .Where(x => x.UserId == credencial.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (cuenta == null)
            return new LoginResult { StatusCode = 0 }; // Cuenta no encontrada

        // 5. Obtener datos de suscripción (IGUAL AL LEGACY)
        var suscripcion = await _context.Suscripciones
            .Include(s => s.PlanEmpleador) // Eager loading
            .Where(x => x.UserId == credencial.UserId)
            .OrderByDescending(x => x.SuscripcionId)
            .FirstOrDefaultAsync(cancellationToken);

        int? planId = 0;
        DateTime? vencimientoPlan = null;
        int? nomina = null;
        int? empleados = null;
        bool? historico = null;

        if (suscripcion != null)
        {
            planId = suscripcion.PlanId;
            vencimientoPlan = suscripcion.Vencimiento;
            nomina = suscripcion.PlanEmpleador?.Nomina;
            empleados = suscripcion.PlanEmpleador?.Empleados;
            historico = suscripcion.PlanEmpleador?.Historico;
        }

        // 6. Obtener perfil (VPerfiles view)
        var perfil = await _context.VPerfiles
            .Where(x => x.UserId == credencial.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        // 7. Retornar resultado (ESTRUCTURA IGUAL AL LEGACY)
        return new LoginResult
        {
            StatusCode = 2, // Success
            UserId = credencial.UserId,
            Email = credencial.Email,
            Nombre = $"{cuenta.Nombre} {cuenta.Apellido}",
            Tipo = cuenta.Tipo,
            PlanId = planId,
            VencimientoPlan = vencimientoPlan,
            Nomina = nomina,
            Empleados = empleados,
            Historico = historico,
            Perfil = perfil != null ? new PerfilDto
            {
                // Mapear propiedades de VPerfiles
                UserId = perfil.UserId,
                EmailUsuario = perfil.EmailUsuario,
                Nombre = perfil.Nombre,
                // ... más propiedades según VPerfiles
            } : null
        };
    }
}
```

#### 3. LoginCommandValidator.cs
```csharp
using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido")
            .EmailAddress().WithMessage("El correo electrónico no es válido")
            .MaximumLength(100).WithMessage("El correo no puede exceder 100 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es requerida")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres")
            .MaximumLength(100).WithMessage("La contraseña no puede exceder 100 caracteres");
    }
}
```

#### 4. LoginResult.cs (DTO)
```csharp
namespace MiGenteEnLinea.Application.Features.Authentication.DTOs;

public class LoginResult
{
    public int StatusCode { get; set; } // 2=success, 0=invalid, -1=inactive
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public string? Nombre { get; set; }
    public int? Tipo { get; set; } // 1=Empleador, 2=Contratista
    public int? PlanId { get; set; }
    public DateTime? VencimientoPlan { get; set; }
    public int? Nomina { get; set; }
    public int? Empleados { get; set; }
    public bool? Historico { get; set; }
    public PerfilDto? Perfil { get; set; }
}

public class PerfilDto
{
    public string? UserId { get; set; }
    public string? EmailUsuario { get; set; }
    public string? Nombre { get; set; }
    // ... más propiedades según VPerfiles
}
```

#### 5. AuthController.cs (Endpoint)
```csharp
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

namespace MiGenteEnLinea.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Autenticar usuario con email y contraseña
    /// </summary>
    /// <param name="command">Credenciales de login</param>
    /// <returns>Resultado de autenticación</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginCommand command)
    {
        _logger.LogInformation("Intento de login para email: {Email}", command.Email);

        var result = await _mediator.Send(command);

        if (result.StatusCode == 0)
        {
            _logger.LogWarning("Login fallido - Credenciales inválidas para: {Email}", command.Email);
            return Unauthorized(new { message = "Credenciales inválidas" });
        }

        if (result.StatusCode == -1)
        {
            _logger.LogWarning("Login fallido - Cuenta inactiva para: {Email}", command.Email);
            return Unauthorized(new { message = "Cuenta inactiva. Contacte al administrador." });
        }

        _logger.LogInformation("Login exitoso para usuario: {UserId}", result.UserId);
        return Ok(result);
    }

    // ... otros 10 endpoints para los demás métodos de LoginService
}
```

### Validación LOTE 1

```bash
# Compilar
cd "C:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean"
dotnet build --no-restore

# Ejecutar API
cd src/Presentation/MiGenteEnLinea.API
dotnet run

# Probar endpoint en Swagger
# http://localhost:5015/swagger/index.html
# POST /api/auth/login
# Body: { "email": "test@example.com", "password": "Test123!" }

# Validar logs
cat logs/migente-*.txt | grep "Login"
```

### Criterios de Éxito LOTE 1

- [ ] 11 Commands/Queries creados
- [ ] 11 Handlers implementados
- [ ] 11 Validators creados
- [ ] 4 DTOs creados
- [ ] AuthController con 11 endpoints funcionando
- [ ] dotnet build sin errores
- [ ] Swagger UI muestra todos los endpoints
- [ ] Login endpoint responde correctamente (200 OK o 401)
- [ ] Logs muestran eventos de autenticación
- [ ] Documento `LOTE_1_AUTHENTICATION_COMPLETADO.md` creado

---

## 🟠 LOTE 2: EMPLEADORES (ALTA PRIORIDAD)

**Prioridad:** 🟠 **ALTA**  
**Prerequisito:** ✅ LOTE 1 completado  
**Servicio Legacy:** (Varios archivos en Empleador/)  
**Duración Estimada:** 3-4 horas  
**Estado:** ⏳ PENDIENTE

### Métodos a Migrar (6-8 métodos estimados)

| # | Método/Operación | Command/Query | Descripción | Archivo Legacy |
|---|------------------|---------------|-------------|----------------|
| 1 | Crear Empleador | **CrearEmpleadorCommand** | INSERT Empleador | (a identificar) |
| 2 | Actualizar Empleador | **ActualizarEmpleadorCommand** | UPDATE Empleador | (a identificar) |
| 3 | Obtener Empleador por ID | **GetEmpleadorByIdQuery** | SELECT por ID | (a identificar) |
| 4 | Obtener Empleadores de Usuario | **GetEmpleadoresByUserQuery** | SELECT por userID | (a identificar) |
| 5 | Eliminar Empleador | **EliminarEmpleadorCommand** | Soft delete | (a identificar) |
| 6 | Buscar Empleadores | **BuscarEmpleadoresQuery** | Búsqueda filtrada | (a identificar) |

**Nota:** Identificar archivos Legacy exactos al comenzar LOTE 2.

---

## 🟠 LOTE 3: CONTRATISTAS (ALTA PRIORIDAD)

**Prioridad:** 🟠 **ALTA**  
**Prerequisito:** ✅ LOTE 2 completado  
**Servicio Legacy:** `ContratistasService.cs`  
**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/ContratistasService.cs`  
**Duración Estimada:** 4-5 horas  
**Estado:** ⏳ PENDIENTE

### Métodos a Migrar (10 total)

| # | Método Legacy | Operación | Command/Query | Descripción |
|---|---------------|-----------|---------------|-------------|
| 1 | `getTodasUltimos20()` | READ | **GetUltimosContratistasQuery** | Últimos 20 activos |
| 2 | `getMiPerfil(string userID)` | READ | **GetMiPerfilContratistaQuery** | Perfil del contratista |
| 3 | `getServicios(int contratistaID)` | READ | **GetServiciosContratistaQuery** | Servicios ofrecidos |
| 4 | `agregarServicio(Contratistas_Servicios)` | WRITE | **AgregarServicioCommand** | Agregar servicio |
| 5 | `removerServicio(int servicioID, int contratistaID)` | WRITE | **RemoverServicioCommand** | Eliminar servicio |
| 6 | `GuardarPerfil(Contratistas ct, string userID)` | WRITE | **GuardarPerfilContratistaCommand** | Actualizar perfil |
| 7 | `ActivarPerfil(string userID)` | WRITE | **ActivarPerfilContratistaCommand** | Activar perfil |
| 8 | `DesactivarPerfil(string userID)` | WRITE | **DesactivarPerfilContratistaCommand** | Desactivar perfil |
| 9 | `getConCriterio(string palabrasClave, string zona)` | READ | **BuscarContratistasQuery** | Búsqueda filtrada |

---

## 🟡 LOTE 4: EMPLEADOS Y NÓMINA (MEDIA PRIORIDAD)

**Prioridad:** 🟡 **MEDIA**  
**Prerequisito:** ✅ LOTE 3 completado  
**Servicio Legacy:** `EmpleadosService.cs`  
**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/EmpleadosService.cs`  
**Duración Estimada:** 6-8 horas  
**Estado:** ⏳ PENDIENTE

### Métodos a Migrar (15 total)

| # | Método Legacy | Operación | Command/Query | Complejidad |
|---|---------------|-----------|---------------|-------------|
| 1 | `getEmpleados(string userID)` | READ | **GetEmpleadosQuery** | Baja |
| 2 | `getVEmpleados(string userID)` | READ | **GetVEmpleadosQuery** | Baja |
| 3 | `getContrataciones(Guid userID)` | READ | **GetContratacionesQuery** | Media |
| 4 | `getEmpleadosByID(Guid userID, int id)` | READ | **GetEmpleadoByIdQuery** | Baja |
| 5 | `obtenerRemuneraciones(string userID, int empleadoID)` | READ | **GetRemuneracionesQuery** | Baja |
| 6 | `quitarRemuneracion(string userID, int id)` | WRITE | **QuitarRemuneracionCommand** | Baja |
| 7 | `guardarEmpleado(Empleados empleado)` | WRITE | **GuardarEmpleadoCommand** | Media |
| 8 | `actualizarEmpleado(Empleados empleado)` | WRITE | **ActualizarEmpleadoCommand** | Media |
| 9 | `ActualizarEmpleado(Empleados empleado)` | WRITE | (duplicado, ignorar) | - |
| 10 | `procesarPago(Header, Detalle)` | WRITE | **ProcesarPagoCommand** | 🔴 ALTA |
| 11 | `procesarPagoContratacion(Header, Detalle)` | WRITE | **ProcesarPagoContratacionCommand** | 🔴 ALTA |

**Nota:** `procesarPago` y `procesarPagoContratacion` son operaciones complejas con múltiples tablas. Requieren atención especial.

---

## 🟡 LOTE 5: SUSCRIPCIONES Y PAGOS (MEDIA PRIORIDAD)

**Prioridad:** 🟡 **MEDIA**  
**Prerequisito:** ✅ LOTE 4 completado  
**Servicios Legacy:** `SuscripcionesService.cs`, `PaymentService.cs`, `CalificacionesService.cs`  
**Duración Estimada:** 5-6 horas  
**Estado:** ⏳ PENDIENTE

### Métodos a Migrar (19 métodos total)

**SuscripcionesService.cs (8 métodos)**
**PaymentService.cs (5 métodos)**
**CalificacionesService.cs (6 métodos)**

---

## 🟢 LOTE 6: SERVICIOS AUXILIARES (BAJA PRIORIDAD)

**Prioridad:** 🟢 **BAJA**  
**Prerequisito:** ✅ LOTE 5 completado  
**Servicios:** `EmailService.cs`, `Utilitario.cs`, otros auxiliares  
**Duración Estimada:** 3-4 horas  
**Estado:** ⏳ PENDIENTE

---

## 🟢 LOTE 7: BOT Y SERVICIOS AVANZADOS (BAJA PRIORIDAD)

**Prioridad:** 🟢 **BAJA**  
**Prerequisito:** ✅ LOTE 6 completado  
**Servicios:** `BotServices.cs`, `botService.asmx.cs` (OpenAI integration)  
**Duración Estimada:** 2-3 horas  
**Estado:** ⏳ PENDIENTE

---

## 📋 COMANDO DE EJECUCIÓN (PARA AGENTE AUTÓNOMO)

### Comando para LOTE 1 (Authentication)

```
@workspace Lee prompts/APPLICATION_LAYER_CQRS_IMPLEMENTATION.md

EJECUTAR: LOTE 1 completo (Authentication)

OBJETIVO: Migrar 11 métodos de LoginService.asmx.cs a CQRS con MediatR

METODOLOGÍA ESTRICTA:
1. LEER Codigo Fuente Mi Gente/MiGente_Front/Services/LoginService.asmx.cs
2. IDENTIFICAR los 11 métodos públicos listados en la tabla
3. Para CADA método:
   a. ANALIZAR la lógica paso a paso
   b. CREAR Command o Query según operación
   c. CREAR Handler con lógica IDÉNTICA al Legacy
   d. CREAR Validator con FluentValidation
   e. CREAR DTOs necesarios
   f. CREAR endpoint en AuthController
4. COMPILAR: dotnet build --no-restore (debe ser exitoso)
5. EJECUTAR: dotnet run (verificar API arranca)
6. PROBAR: Swagger UI - endpoint /api/auth/login
7. DOCUMENTAR: Crear LOTE_1_AUTHENTICATION_COMPLETADO.md

PATRÓN DE REFERENCIA:
Seguir el ejemplo completo de LoginCommand (incluido arriba).
La lógica en Handler DEBE SER IDÉNTICA al método login() en LoginService.asmx.cs

AUTORIZACIÓN COMPLETA:
- Leer todos los archivos Legacy necesarios
- Crear todos los archivos en Application Layer
- Crear AuthController en API Layer
- Ejecutar dotnet build y dotnet run
- NO aplicar migraciones (solo uso de DbContext existente)

DURACIÓN ESTIMADA: 4-6 horas

CRITERIO DE ÉXITO:
- 11 Commands/Queries funcionando
- 11 endpoints en AuthController
- dotnet build sin errores
- Swagger UI accesible
- Login endpoint funcional

COMENZAR EJECUCIÓN AUTOMÁTICA AHORA.
```

---

## 📊 MÉTRICAS Y PROGRESO

### Estimación Total

| Lote | Prioridad | Métodos | Duración | Estado |
|------|-----------|---------|----------|--------|
| **LOTE 1** | 🔴 CRÍTICA | 11 | 4-6h | ⏳ Pendiente |
| **LOTE 2** | 🟠 ALTA | 6-8 | 3-4h | 🚫 Bloqueado |
| **LOTE 3** | 🟠 ALTA | 10 | 4-5h | 🚫 Bloqueado |
| **LOTE 4** | 🟡 MEDIA | 15 | 6-8h | 🚫 Bloqueado |
| **LOTE 5** | 🟡 MEDIA | 19 | 5-6h | 🚫 Bloqueado |
| **LOTE 6** | 🟢 BAJA | 10-12 | 3-4h | 🚫 Bloqueado |
| **LOTE 7** | 🟢 BAJA | 3-5 | 2-3h | 🚫 Bloqueado |
| **TOTAL** | - | **74-80** | **27-36h** | **0% Complete** |

### Progreso Visual

```
LOTE 1 (Authentication):        ░░░░░░░░░░░░░░░░░░░░ 0% (0/11)
LOTE 2 (Empleadores):           ░░░░░░░░░░░░░░░░░░░░ 0% (0/8)
LOTE 3 (Contratistas):          ░░░░░░░░░░░░░░░░░░░░ 0% (0/10)
LOTE 4 (Empleados/Nómina):      ░░░░░░░░░░░░░░░░░░░░ 0% (0/15)
LOTE 5 (Suscripciones/Pagos):   ░░░░░░░░░░░░░░░░░░░░ 0% (0/19)
LOTE 6 (Servicios Auxiliares):  ░░░░░░░░░░░░░░░░░░░░ 0% (0/12)
LOTE 7 (Bot/Avanzados):         ░░░░░░░░░░░░░░░░░░░░ 0% (0/5)
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
TOTAL IMPLEMENTADO:             ░░░░░░░░░░░░░░░░░░░░ 0% (0/80)
```

---

## ✅ CHECKLIST GENERAL

### Pre-Ejecución (antes de LOTE 1)

- [ ] Domain Layer completo (36 entidades) ✅
- [ ] Infrastructure configurado (relaciones) ✅
- [ ] Program.cs configurado (MediatR, FluentValidation, AutoMapper) ✅
- [ ] API ejecutándose en puerto 5015 ✅
- [ ] Swagger UI accesible ✅
- [ ] Leer este documento completo
- [ ] Entender metodología de paridad con Legacy
- [ ] Ubicar archivos Legacy en `Codigo Fuente Mi Gente/`

### Post-Ejecución (después de cada LOTE)

- [ ] Todos los Commands/Queries creados
- [ ] Todos los Handlers implementados con lógica Legacy
- [ ] Todos los Validators creados
- [ ] DTOs necesarios creados
- [ ] Controller con endpoints funcionando
- [ ] `dotnet build` sin errores
- [ ] `dotnet run` - API ejecutándose
- [ ] Swagger UI muestra nuevos endpoints
- [ ] Probar al menos 3 endpoints con Postman/Swagger
- [ ] Logs muestran operaciones
- [ ] Documento `LOTE_X_COMPLETADO.md` creado

---

## 🚨 NOTAS IMPORTANTES

### Sobre Paridad con Legacy

**⚠️ REGLA DE ORO: NO INNOVAR SIN AUTORIZACIÓN**

- Si el Legacy hace `db.SaveChanges()` dos veces separadas, hacerlo igual
- Si el Legacy retorna códigos numéricos (2, 0, -1), mantenerlos igual
- Si el Legacy usa nombres en español, mantenerlos igual
- Si el Legacy tiene lógica "fea" pero funcional, copiarla tal cual

**Ejemplos de HACER:**
```csharp
// Legacy usa códigos numéricos
if (result == null) return 0;
if (!result.activo) return -1;
return 2;

// Clean debe hacer lo mismo
public class LoginResult 
{
    public int StatusCode { get; set; } // 2=success, 0=invalid, -1=inactive
}
```

**Ejemplos de NO HACER:**
```csharp
// ❌ NO cambiar códigos a bool/enum sin autorización
public class LoginResult 
{
    public bool IsSuccess { get; set; } // ❌ Legacy usa int
    public LoginStatus Status { get; set; } // ❌ Legacy no usa enum
}
```

### Sobre Encriptación de Passwords

**Legacy usa:** `Crypt.Encrypt(pass)` (custom encryption)  
**Clean usa:** `BCrypt.Net.BCrypt.HashPassword(password, 12)` (modern hashing)

**⚠️ IMPORTANTE:** Durante la migración gradual, la base de datos tendrá:
- Passwords antiguos con `Crypt` (legacy)
- Passwords nuevos con `BCrypt` (clean)

**Solución temporal en Handler:**
```csharp
// Intentar BCrypt primero (nuevo estándar)
if (credencial.PasswordHash.StartsWith("$2a$") || credencial.PasswordHash.StartsWith("$2b$"))
{
    // Es BCrypt
    if (!_passwordHasher.VerifyPassword(request.Password, credencial.PasswordHash))
        return new LoginResult { StatusCode = 0 };
}
else
{
    // Es legacy Crypt - usar servicio de compatibilidad
    var crypt = new LegacyCrypt();
    var crypted = crypt.Encrypt(request.Password);
    if (credencial.PasswordHash != crypted)
        return new LoginResult { StatusCode = 0 };
}
```

### Sobre Queries EF Core

**Legacy usa:**
```csharp
var result = db.Credenciales.Where(x => x.email == email && x.password == crypted).FirstOrDefault();
```

**Clean debe usar:**
```csharp
var credencial = await _context.Credenciales
    .Where(x => x.Email == request.Email)
    .FirstOrDefaultAsync(cancellationToken);
```

**Diferencias clave:**
- `.FirstOrDefault()` → `.FirstOrDefaultAsync(cancellationToken)`
- Propiedades en PascalCase (convención C#): `email` → `Email`
- Siempre usar `await` y `CancellationToken`

### Sobre Views (VPerfiles, VEmpleados, VContratistas)

Las **views** del Legacy (`VPerfiles`, `VEmpleados`, etc.) están en el DbContext como **Read Models**. Se usan para consultas optimizadas.

**Legacy:**
```csharp
var perfil = db.VPerfiles.Where(a => a.userID == userID).FirstOrDefault();
```

**Clean:**
```csharp
var perfil = await _context.VPerfiles
    .Where(x => x.UserId == request.UserId)
    .AsNoTracking() // Read-only
    .FirstOrDefaultAsync(cancellationToken);
```

**Usar `.AsNoTracking()` en Queries** para mejor performance (no tracking de cambios).

---

## 📞 SOPORTE Y TROUBLESHOOTING

### Problemas Comunes

#### Problema 1: "No se encuentra el namespace MiGenteEnLinea.Application.Features.X"

**Causa:** Carpeta no creada o namespace incorrecto.

**Solución:**
```csharp
// Crear estructura de carpetas:
Features/
  Authentication/
    Commands/
      Login/

// Namespace correcto:
namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;
```

#### Problema 2: "MediatR no está registrado"

**Causa:** DependencyInjection.cs no llamado en Program.cs

**Solución:**
```csharp
// En Program.cs
builder.Services.AddApplication(); // Debe estar presente
```

#### Problema 3: "Handler no se ejecuta"

**Causa:** Handler no implementa `IRequestHandler<TRequest, TResponse>`

**Solución:**
```csharp
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken ct)
    {
        // ...
    }
}
```

#### Problema 4: "Validator no se ejecuta automáticamente"

**Causa:** Behavior de validación no configurado

**Solución:**
```csharp
// En Application/DependencyInjection.cs
services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Agregar ValidationBehavior (opcional - para validación automática pre-handler)
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
```

---

## 🎯 PRÓXIMO PASO

**🚀 COMENZAR CON LOTE 1 (AUTHENTICATION)**

Copiar el **Comando de Ejecución para LOTE 1** (arriba) y ejecutarlo en Claude Sonnet 4.5 en modo agente.

**Duración esperada:** 4-6 horas  
**Output esperado:** AuthController con 11 endpoints funcionando, documento LOTE_1_AUTHENTICATION_COMPLETADO.md

---

_Documento creado: 2025-10-12_  
_Última actualización: 2025-10-12_  
_Estado: Listo para ejecución_
