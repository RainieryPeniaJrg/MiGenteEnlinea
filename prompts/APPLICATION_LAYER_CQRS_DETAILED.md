# 🚀 APPLICATION LAYER (CQRS) - IMPLEMENTACIÓN COMPLETA CON MediatR

**Fecha de Creación:** 12 de octubre, 2025  
**Proyecto:** MiGente En Línea - Clean Architecture Migration  
**Fase:** Phase 4 - Application Layer (CQRS con MediatR)  
**Estado:** LOTE 1 al 85% (bloqueado), LOTES 2-6 pendientes

---

## 🎯 OBJETIVO PRINCIPAL

Implementar **Application Layer** completo migrando la lógica de negocio desde los servicios Legacy (ASP.NET Web Forms) a **Clean Architecture** usando **CQRS pattern con MediatR**, manteniendo **100% de paridad funcional** con el sistema Legacy.

### ⚠️ REGLA CRÍTICA #1: ANÁLISIS EXHAUSTIVO ANTES DE IMPLEMENTAR

```
ANTES de escribir UNA SOLA LÍNEA de código:

PASO 1: ANALIZAR PROYECTO LEGACY (2-3 horas)
========================================
1.1. Leer TODOS los archivos en Services/ (12 servicios)
1.2. Identificar TODOS los métodos públicos por servicio
1.3. Crear mapa de dependencias (servicio → método → entidades usadas)
1.4. Documentar lógica de negocio por método (validaciones, cálculos, reglas)
1.5. Identificar queries EF6 (LINQ) que deben convertirse a EF Core

PASO 2: ANALIZAR PROYECTO CLEAN ARCHITECTURE (1 hora)
====================================================
2.1. Revisar Domain Layer (36 entidades migradas)
2.2. Verificar Infrastructure Layer (relaciones, DbContext)
2.3. Revisar Program.cs (MediatR ya configurado)
2.4. Identificar interfaces faltantes (IPasswordHasher, IJwtTokenService, etc.)

PASO 3: CREAR PLAN DE IMPLEMENTACIÓN (30 mins)
==============================================
3.1. Mapear servicios Legacy → Features en Application
3.2. Clasificar métodos en Commands vs Queries
3.3. Identificar Commands/Queries reutilizables
3.4. Definir orden de implementación por dependencias

PASO 4: IMPLEMENTAR LOTE 1 (4-6 horas)
====================================
4.1. Implementar Commands con lógica EXACTA del Legacy
4.2. Implementar Queries con lógica EXACTA del Legacy
4.3. Crear Validators con FluentValidation
4.4. Crear DTOs con AutoMapper
4.5. Crear Controllers con documentación Swagger

PASO 5: VALIDAR Y DOCUMENTAR (1 hora)
====================================
5.1. Compilar sin errores (dotnet build)
5.2. Probar TODOS los endpoints con Swagger
5.3. Comparar resultados con Legacy (inputs idénticos)
5.4. Documentar en LOTE_X_COMPLETADO.md
```

**NO saltes el PASO 1 y 2. Son OBLIGATORIOS para garantizar paridad funcional.**

---

## 📊 ESTADO ACTUAL DEL PROYECTO

### ✅ Fases Completadas (100%)

1. **Domain Layer:** 36 entidades migradas con DDD
   - 24 Rich Domain Models
   - 9 Read Models (vistas)
   - 3 Catálogos
   - Reporte: `MIGRATION_100_COMPLETE.md`

2. **Infrastructure Layer:** Relaciones DB validadas
   - 9 FK relationships configuradas
   - DbContext con Audit Interceptor
   - BCryptPasswordHasher implementado
   - Reporte: `DATABASE_RELATIONSHIPS_REPORT.md`

3. **Program.cs Configuration:** API configurado
   - Serilog (logging)
   - CORS (Development + Production policies)
   - Swagger UI
   - Health Check endpoint
   - Reporte: `PROGRAM_CS_CONFIGURATION_REPORT.md`

### 🔄 Fase Actual: Application Layer (0-85% según lote)

#### LOTE 1: Authentication & User Management
**Estado:** 85% COMPLETADO - BLOQUEADO por errores NuGet  
**Archivos Creados:** 23 archivos (~1,380 líneas)  
**Reporte:** `LOTE_1_AUTHENTICATION_PARCIAL.md`

**Completado:**
- ✅ 2/5 Commands (Login, ChangePassword)
- ✅ 4/5 Queries (GetPerfil, GetPerfilByEmail, ValidarCorreo, GetCredenciales)
- ✅ 5/5 DTOs
- ✅ 4/4 Interfaces (IApplicationDbContext, IPasswordHasher, IJwtTokenService, IEmailService)
- ✅ 1/1 Controller (AuthController con 6 endpoints)

**Pendiente:**
- ❌ RegisterCommand
- ❌ ActivateAccountCommand
- ❌ UpdateProfileCommand
- ❌ Fix errores NuGet (27 errores de compilación)

#### LOTE 2-6: Pendientes (0% cada uno)
- ⏳ LOTE 2: Empleadores CRUD
- ⏳ LOTE 3: Contratistas CRUD + Búsqueda
- ⏳ LOTE 4: Empleados y Nómina
- ⏳ LOTE 5: Suscripciones y Pagos
- ⏳ LOTE 6: Calificaciones y Extras

---

## 📋 PASO 1: ANÁLISIS EXHAUSTIVO DEL LEGACY (OBLIGATORIO)

### 1.1. INVENTARIO COMPLETO DE SERVICIOS LEGACY

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/`

| # | Servicio Legacy | Métodos Públicos | Líneas | Complejidad | Prioridad |
|---|----------------|------------------|---------|-------------|-----------|
| 1 | **LoginService.asmx.cs** | 10 métodos | ~250 | 🟡 MEDIA | 🔴 CRÍTICA |
| 2 | **EmpleadosService.cs** | 32 métodos | ~800 | 🔴 ALTA | 🟠 ALTA |
| 3 | **ContratistasService.cs** | 10 métodos | ~200 | 🟢 BAJA | 🟠 ALTA |
| 4 | **SuscripcionesService.cs** | 17 métodos | ~400 | 🟡 MEDIA | 🟡 MEDIA |
| 5 | **CalificacionesService.cs** | 4 métodos | ~80 | 🟢 BAJA | 🟢 BAJA |
| 6 | **PaymentService.cs** | 3 métodos | ~150 | 🟡 MEDIA | 🟡 MEDIA |
| 7 | **EmailService.cs** | 5 métodos | ~100 | 🟢 BAJA | 🟢 BAJA |
| 8 | **BotServices.cs** | 3 métodos | ~120 | 🟢 BAJA | 🟢 BAJA |
| 9 | **Utilitario.cs** | 5 métodos | ~80 | 🟢 BAJA | 🟢 BAJA |
| **TOTAL** | **9 servicios** | **89 métodos** | **~2,180 líneas** | | |

---

### 1.2. ANÁLISIS DETALLADO POR SERVICIO

#### 🔴 SERVICIO 1: LoginService.asmx.cs (CRÍTICO)

**Ubicación:** `Services/LoginService.asmx.cs`  
**Métodos Públicos:** 10  
**Dependencias:** Credenciales, Cuentas, Suscripciones, VPerfiles

**Tabla de Métodos:**

| # | Método Legacy | Params | Return | Lógica Crítica | Command/Query |
|---|---------------|--------|--------|----------------|---------------|
| 1 | `login(email, pass)` | email, pass | int (2=success, 0=invalid, -1=inactive) | - Buscar credencial<br>- Verificar password con Crypt.Encrypt()<br>- Validar activo<br>- Obtener cuenta<br>- Obtener suscripción más reciente<br>- Crear cookie con 8 valores | **LoginCommand** ✅ |
| 2 | `borrarUsuario(userID, credencialID)` | userID, credencialID | void | - Buscar credencial<br>- Eliminar (hard delete) | **DeleteUsuarioCommand** ❌ |
| 3 | `obtenerPerfil(userID)` | userID | VPerfiles | - Query a vista VPerfiles | **GetPerfilQuery** ✅ |
| 4 | `obtenerPerfilByEmail(email)` | email | VPerfiles | - Query a vista VPerfiles por email | **GetPerfilByEmailQuery** ✅ |
| 5 | `obtenerCredenciales(userID)` | userID | List<Credenciales> | - Query a Credenciales | **GetCredencialesQuery** ✅ |
| 6 | `actualizarPerfil(info, cuenta)` | perfilesInfo, Cuentas | bool | - 2 DbContext separados<br>- Update perfilesInfo<br>- Update Cuentas | **UpdatePerfilCommand** ❌ |
| 7 | `agregarPerfilInfo(info)` | perfilesInfo | bool | - Add perfilesInfo | **CreatePerfilInfoCommand** ❌ |
| 8 | `getPerfilByID(cuentaID)` | cuentaID | Cuentas | - Query Cuentas by ID | **GetPerfilByIdQuery** ❌ |
| 9 | `validarCorreo(correo)` | correo | bool | - Check if email exists | **ValidarCorreoQuery** ✅ |
| 10 | `getPerfilInfo(userID)` | Guid userID | VPerfiles | - Query vista por userID | **GetPerfilInfoQuery** ❌ |

**Lógica Crítica a Replicar:**

```csharp
// EJEMPLO: login() - LÓGICA EXACTA A COPIAR
public int login(string email, string pass)
{
    using (var db = new migenteEntities())
    {
        // PASO 1: Encriptar password (LEGACY usa Crypt, Clean usará BCrypt)
        Crypt crypt = new Crypt();
        var crypted = crypt.Encrypt(pass);
        
        // PASO 2: Buscar credencial
        var result = db.Credenciales.Where(x => x.email == email && x.password == crypted).FirstOrDefault();
        
        if (result != null)
        {
            // PASO 3: Validar si está activo
            if (!(bool)result.activo)
            {
                return -1; // ⚠️ IMPORTANTE: -1 = usuario inactivo
            }
            
            // PASO 4: Obtener cuenta
            var cuenta = db.Cuentas.Where(x => x.userID == result.userID).FirstOrDefault();
            
            // PASO 5: Obtener suscripción MÁS RECIENTE (OrderByDescending)
            var suscripcion = db.Suscripciones
                .Where(x => x.userID == result.userID)
                .Include(a => a.Planes_empleadores)
                .OrderByDescending(x => x.suscripcionID) // ⚠️ CRÍTICO: Más reciente
                .FirstOrDefault();
            
            // PASO 6: Manejar planID (0 si no tiene suscripción)
            if (suscripcion == null)
            {
                myCookie["planID"] = "0"; // ⚠️ IMPORTANTE: "0" = sin plan
            }
            else
            {
                // Guardar datos de suscripción
                myCookie["planID"] = suscripcion.planID.ToString();
                myCookie["vencimientoPlan"] = Convert.ToDateTime(suscripcion.vencimiento).ToString("d");
                myCookie["nomina"] = suscripcion.Planes_empleadores.nomina.ToString();
                // ... resto de cookies
            }
            
            // PASO 7: Obtener perfil completo
            var vPerfil = obtenerPerfil(result.userID);
            
            return 2; // ⚠️ IMPORTANTE: 2 = login exitoso
        }
        else
        {
            return 0; // ⚠️ IMPORTANTE: 0 = credenciales inválidas
        }
    }
}
```

**⚠️ PUNTOS CRÍTICOS A NO OLVIDAR:**
1. Códigos de retorno: 2=success, 0=invalid, -1=inactive
2. Suscripción MÁS RECIENTE (OrderByDescending)
3. planID = "0" si no tiene suscripción
4. Encriptación: Legacy usa Crypt, Clean usa BCrypt (necesita compatibilidad)

---

#### 🟠 SERVICIO 2: EmpleadosService.cs (ALTA PRIORIDAD)

**Ubicación:** `Services/EmpleadosService.cs`  
**Métodos Públicos:** 32  
**Dependencias:** Empleados, EmpleadosTemporales, DetalleContrataciones, Empleador_Recibos_Header, Empleador_Recibos_Detalle

**Métodos Más Complejos:**

| # | Método | Complejidad | Lógica Crítica |
|---|--------|-------------|----------------|
| 1 | `procesarPago(header, detalle)` | 🔴 ALTA | - 2 DbContext separados<br>- Guardar header primero (genera pagoID)<br>- Asignar pagoID a todos los detalles<br>- Guardar detalles | **Command** |
| 2 | `procesarPagoContratacion(header, detalle)` | 🔴 ALTA | - Mismo patrón que procesarPago<br>- ADICIONAL: Si concepto="Pago Final" → actualizar estatus contratación a 2 | **Command** |
| 3 | `darDeBaja(empleadoID, fechaBaja, prestaciones, motivo)` | 🟡 MEDIA | - Soft delete (Activo=false)<br>- Guardar fecha, prestaciones, motivo | **Command** |
| 4 | `consultarPadron(cedula)` | 🟡 MEDIA | - API externa (República Dominicana)<br>- Autenticación con token<br>- Retornar PadronModel | **Query** |
| 5 | `getEmpleados(userID)` | 🟢 BAJA | - Query simple con filtro | **Query** |

**Lógica Crítica: procesarPago()**

```csharp
public int procesarPago(Empleador_Recibos_Header header, List<Empleador_Recibos_Detalle> detalle)
{
    // PASO 1: Guardar header PRIMERO (esto genera el pagoID)
    using (var db = new migenteEntities())
    {
        db.Empleador_Recibos_Header.Add(header);
        db.SaveChanges(); // ⚠️ AQUÍ SE GENERA pagoID (auto-increment)
    }
    
    // PASO 2: Asignar pagoID a TODOS los detalles
    using (var db1 = new migenteEntities()) // ⚠️ Nuevo DbContext
    {
        foreach (var item in detalle)
        {
            item.pagoID = header.pagoID; // Usa el pagoID generado
        }
        
        db1.Empleador_Recibos_Detalle.AddRange(detalle);
        db1.SaveChanges();
    }
    
    return header.pagoID; // Retorna el pagoID para referencia
}
```

**⚠️ CRÍTICO:** NO usar transacción única. Legacy usa 2 DbContext separados deliberadamente (genera pagoID primero).

---

#### 🟠 SERVICIO 3: ContratistasService.cs (ALTA PRIORIDAD)

**Métodos Principales:**

| # | Método | Lógica Crítica |
|---|--------|----------------|
| 1 | `getTodasUltimos20()` | - OrderByDescending(a => contratistaID)<br>- Where(activo == true)<br>- Take(20) | **Query** |
| 2 | `getConCriterio(palabrasClave, zona)` | - Búsqueda case-insensitive en titulo<br>- Filtro opcional por provincia<br>- Solo activos | **Query** |
| 3 | `GuardarPerfil(ct, userID)` | - Update selectivo de 15 propiedades<br>- NO actualiza todas las columnas | **Command** |
| 4 | `ActivarPerfil(userID)` | - Update activo=true | **Command** |

---

## 📦 PASO 2: ANÁLISIS DEL PROYECTO CLEAN ARCHITECTURE

### 2.1. Verificar Entidades Migradas (Domain Layer)

**Comando de validación:**
```powershell
cd "c:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean\src\Core\MiGenteEnLinea.Domain\Entities"
Get-ChildItem -Recurse -Filter *.cs | Select-Object Name, Directory | Format-Table
```

**Entidades Críticas para LOTE 1:**
- ✅ `Authentication/Credencial.cs`
- ✅ `Seguridad/Cuenta.cs` (NO en Catalogos, verificar namespace)
- ✅ `Suscripciones/Suscripcion.cs`
- ✅ `Suscripciones/PlanEmpleador.cs`
- ✅ `ReadModels/VistaPerfil.cs`

### 2.2. Verificar DbContext (Infrastructure Layer)

**Archivo:** `Infrastructure/Persistence/Contexts/MiGenteDbContext.cs`

**Validar que tiene:**
```csharp
public DbSet<Credencial> Credenciales { get; set; }
public DbSet<Cuenta> Cuentas { get; set; }
public DbSet<Suscripcion> Suscripciones { get; set; }
public DbSet<PlanEmpleador> PlanesEmpleadores { get; set; }
public DbSet<VistaPerfil> VPerfiles { get; set; }
```

### 2.3. Verificar Configuración MediatR

**Archivo:** `Application/DependencyInjection.cs`

```csharp
public static IServiceCollection AddApplication(this IServiceCollection services)
{
    services.AddMediatR(config => {
        config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });
    
    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    services.AddAutoMapper(Assembly.GetExecutingAssembly());
    
    return services;
}
```

---

## 🚀 PASO 3: PLAN DE IMPLEMENTACIÓN POR LOTES

### LOTE 1: Authentication & User Management (PRIORIDAD CRÍTICA)

**Estado Actual:** 85% completado, bloqueado por errores NuGet

**Acción Inmediata Requerida:**

1. **Fix Errores NuGet (5 minutos)**
```powershell
cd "c:\Users\ray\OneDrive\Documents\ProyectoMigente\MiGenteEnLinea.Clean"

# Agregar referencias faltantes
dotnet add src/Core/MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add src/Core/MiGenteEnLinea.Application/MiGenteEnLinea.Application.csproj package Microsoft.Extensions.Logging.Abstractions --version 8.0.0

# Verificar
dotnet build --no-restore
```

2. **Fix Namespace de Cuenta (2 minutos)**

En `Application/Common/Interfaces/IApplicationDbContext.cs` línea 16:
```csharp
// CAMBIAR:
using MiGenteEnLinea.Domain.Entities.Catalogos;

// A:
using MiGenteEnLinea.Domain.Entities.Seguridad;
```

3. **Implementar 3 Commands Faltantes (2-3 horas)**

#### 3.1. RegisterCommand (1 hora)

**Legacy Reference:** `SuscripcionesService.cs→GuardarPerfil() + guardarCredenciales()`

**Archivos a crear:**
```
Features/Authentication/Commands/Register/
├── RegisterCommand.cs
├── RegisterCommandHandler.cs
└── RegisterCommandValidator.cs
```

**Lógica a replicar (7 pasos):**
```csharp
public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken ct)
{
    // PASO 1: Validar email no existe
    var emailExists = await _context.Credenciales
        .AnyAsync(c => c.Email == request.Email, ct);
    
    if (emailExists)
        throw new ValidationException("El correo electrónico ya está registrado");
    
    // PASO 2: Crear Cuenta
    var cuenta = new Cuenta
    {
        UserId = Guid.NewGuid().ToString(),
        Nombre = request.Nombre,
        Apellido = request.Apellido,
        Email = request.Email,
        Tipo = request.Tipo, // 1=Empleador, 2=Contratista
        FechaCreacion = DateTime.UtcNow,
        Telefono1 = request.Telefono1
    };
    await _context.Cuentas.AddAsync(cuenta, ct);
    
    // PASO 3: Crear Credencial con password hasheado
    var credencial = new Credencial
    {
        UserId = cuenta.UserId,
        Email = request.Email,
        PasswordHash = _passwordHasher.HashPassword(request.Password),
        Activo = false, // ⚠️ IMPORTANTE: false hasta activar
        FechaCreacion = DateTime.UtcNow
    };
    await _context.Credenciales.AddAsync(credencial, ct);
    
    // PASO 4: Crear Contratista si Tipo=2 (lógica Legacy)
    if (request.Tipo == 2)
    {
        var contratista = new Contratista
        {
            UserId = cuenta.UserId,
            Nombre = cuenta.Nombre,
            Apellido = cuenta.Apellido,
            Email = request.Email,
            Tipo = 1, // 1=Persona física (default Legacy)
            Activo = false,
            Telefono1 = request.Telefono1,
            FechaIngreso = DateTime.UtcNow
        };
        await _context.Contratistas.AddAsync(contratista, ct);
    }
    
    // PASO 5: Crear Suscripción inicial (planID=0)
    var suscripcion = new Suscripcion
    {
        UserId = cuenta.UserId,
        PlanId = 0, // ⚠️ IMPORTANTE: 0 = sin plan
        Estado = "Pendiente",
        FechaInicio = DateTime.UtcNow
    };
    await _context.Suscripciones.AddAsync(suscripcion, ct);
    
    // PASO 6: Guardar cambios
    await _context.SaveChangesAsync(ct);
    
    // PASO 7: Enviar email de activación
    await _emailService.SendActivationEmailAsync(
        request.Email, 
        cuenta.UserId,
        request.Host // URL del frontend para generar link de activación
    );
    
    _logger.LogInformation("Usuario registrado: {UserId}, Email: {Email}", cuenta.UserId, request.Email);
    
    return new RegisterResult
    {
        IsSuccess = true,
        UserId = cuenta.UserId,
        Message = "Registro exitoso. Por favor revisa tu correo para activar tu cuenta."
    };
}
```

#### 3.2. ActivateAccountCommand (30 minutos)

**Legacy Reference:** `activarperfil.aspx.cs`

#### 3.3. UpdateProfileCommand (30 minutos)

**Legacy Reference:** `LoginService.asmx.cs→actualizarPerfil()`

---

## 📝 CHECKLIST DE IMPLEMENTACIÓN

### Pre-Implementación (OBLIGATORIO)

- [ ] **PASO 1 COMPLETADO:** Leer TODOS los servicios Legacy
- [ ] **PASO 2 COMPLETADO:** Verificar Domain/Infrastructure layers
- [ ] **Mapa de dependencias creado:** Servicio → Método → Entidades
- [ ] **Plan de lotes definido:** Orden de implementación

### LOTE 1: Authentication

- [ ] Fix errores NuGet (5 min)
- [ ] Fix namespace Cuenta (2 min)
- [ ] RegisterCommand implementado (1h)
- [ ] ActivateAccountCommand implementado (30 min)
- [ ] UpdateProfileCommand implementado (30 min)
- [ ] `dotnet build` exitoso (0 errores)
- [ ] Swagger UI funciona (todos los endpoints)
- [ ] Pruebas manuales completadas:
  - [ ] POST /api/auth/register (crear usuario)
  - [ ] POST /api/auth/activate (activar cuenta)
  - [ ] POST /api/auth/login (autenticar)
  - [ ] GET /api/auth/perfil/{userId}
  - [ ] POST /api/auth/change-password
  - [ ] PUT /api/auth/perfil/{userId}
- [ ] Resultados comparados con Legacy (paridad 100%)
- [ ] Documentado en `LOTE_1_AUTHENTICATION_COMPLETADO.md`

### LOTE 2-6

- [ ] Análisis Legacy completado
- [ ] Commands/Queries implementados
- [ ] Validators creados
- [ ] DTOs creados
- [ ] Controllers implementados
- [ ] Testing completado
- [ ] Documentado

---

## 🎯 COMANDO DE EJECUCIÓN PARA AGENTE AUTÓNOMO

```
@workspace Lee prompts/APPLICATION_LAYER_CQRS_DETAILED.md

CONTEXTO:
Este es un proyecto de migración de ASP.NET Web Forms a Clean Architecture.
La fase de Domain Layer está 100% completada (36 entidades).
La fase de Infrastructure Layer está configurada.
La fase de Application Layer (CQRS) está al 15% (LOTE 1 al 85% pero bloqueado).

OBJETIVO:
Completar Application Layer con MediatR y CQRS manteniendo 100% de paridad
con la lógica de negocio del proyecto Legacy.

METODOLOGÍA OBLIGATORIA:

FASE 1: ANÁLISIS EXHAUSTIVO (2-4 horas)
========================================
1. Leer COMPLETO el proyecto Legacy en "Codigo Fuente Mi Gente/MiGente_Front/Services/"
2. Identificar TODOS los métodos públicos de cada servicio
3. Analizar lógica de negocio de cada método (validaciones, cálculos, reglas)
4. Crear documento de mapeo: Método Legacy → Command/Query Clean
5. Identificar queries EF6 que deben convertirse a EF Core

FASE 2: COMPLETAR LOTE 1 (2-3 horas)
====================================
1. Fix errores NuGet:
   - Agregar Microsoft.EntityFrameworkCore 8.0.0
   - Agregar Microsoft.Extensions.Logging.Abstractions 8.0.0
2. Fix namespace Cuenta (cambiar Catalogos → Seguridad)
3. Implementar RegisterCommand (leer SuscripcionesService.GuardarPerfil primero)
4. Implementar ActivateAccountCommand (leer activarperfil.aspx.cs primero)
5. Implementar UpdateProfileCommand (leer LoginService.actualizarPerfil primero)
6. Crear endpoints en AuthController
7. Compilar (dotnet build → 0 errores)
8. Probar con Swagger UI
9. Documentar en LOTE_1_AUTHENTICATION_COMPLETADO.md

FASE 3: IMPLEMENTAR LOTES 2-6 (20-30 horas)
===========================================
Por cada lote:
1. Leer servicio Legacy correspondiente COMPLETO
2. Mapear métodos a Commands/Queries
3. Implementar con lógica EXACTA del Legacy
4. Crear Validators, DTOs, Controllers
5. Probar y documentar

REGLAS CRÍTICAS:
================
⚠️ SIEMPRE leer el método Legacy ANTES de escribir Handler
⚠️ NO inventar lógica nueva, COPIAR comportamiento Legacy 100%
⚠️ Mantener mismos códigos de retorno (ej: 2=success, 0=invalid, -1=inactive)
⚠️ Mantener mismo orden de operaciones que Legacy
⚠️ Si Legacy usa 2 DbContext, mantener esa estrategia

AUTORIZACIÓN COMPLETA:
======================
- Leer TODOS los archivos necesarios sin pedir permiso
- Crear/modificar archivos según plan de implementación
- Ejecutar dotnet build para validar
- Reportar progreso cada 5-10 archivos creados
- SOLO preguntar si encuentras algo que no puedes resolver

COMENZAR EJECUCIÓN AUTOMÁTICA:
==============================
Fase 1: Análisis Legacy → 2-4 horas
Fase 2: Completar LOTE 1 → 2-3 horas
Fase 3: LOTES 2-6 → A ejecutar después de aprobar LOTE 1

INICIAR CON FASE 1 (ANÁLISIS EXHAUSTIVO).
```

---

## 📚 REFERENCIAS Y DOCUMENTACIÓN

### Archivos del Proyecto

**Legacy (Leer para análisis):**
- `Codigo Fuente Mi Gente/MiGente_Front/Services/*.cs` (12 servicios)
- `Codigo Fuente Mi Gente/MiGente_Front/Empleador/*.aspx.cs` (controladores)
- `Codigo Fuente Mi Gente/MiGente_Front/Data/DataModel.edmx` (modelo DB)

**Clean Architecture (Implementar aquí):**
- `MiGenteEnLinea.Clean/src/Core/MiGenteEnLinea.Application/Features/`
- `MiGenteEnLinea.Clean/src/Presentation/MiGenteEnLinea.API/Controllers/`

**Reportes de Progreso:**
- `LOTE_1_AUTHENTICATION_PARCIAL.md` (85% completado)
- `MIGRATION_100_COMPLETE.md` (Domain Layer 100%)
- `DATABASE_RELATIONSHIPS_REPORT.md` (Infrastructure 100%)
- `PROGRAM_CS_CONFIGURATION_REPORT.md` (API Config 100%)

---

**Fecha de Última Actualización:** 12 de octubre, 2025  
**Autor:** GitHub Copilot  
**Estado del Prompt:** ✅ LISTO PARA EJECUCIÓN CON AGENTE AUTÓNOMO  
**Tiempo Estimado Total:** 24-37 horas (Análisis 2-4h + LOTE 1: 2-3h + LOTES 2-6: 20-30h)
