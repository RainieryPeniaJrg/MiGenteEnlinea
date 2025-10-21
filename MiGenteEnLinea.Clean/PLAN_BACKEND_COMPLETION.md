# 🎯 PLAN DE COMPLETITUD BACKEND - Resumen Ejecutivo

**Objetivo:** Cerrar brechas entre Legacy Services y Clean Architecture API  
**Estado Actual:** 43% completado (35 de 81 métodos)  
**Meta:** 100% paridad con Legacy  
**Timeline:** 3 semanas (~40 horas)

---

## 📊 RESUMEN VISUAL

```
┌─────────────────────────────────────────────────────────────┐
│ PROGRESS: Backend API Completion                            │
│                                                              │
│ [████████████████░░░░░░░░░░░░░░░░░░░░░░] 43%              │
│                                                              │
│ ✅ Completado: 35 endpoints                                 │
│ ⏳ Pendiente:   46 endpoints                                │
│                                                              │
└─────────────────────────────────────────────────────────────┘

Módulos:
✅ Calificaciones     [████████████████████] 100%
✅ Pagos (Cardnet)    [████████████████████] 100%
✅ Email              [████████████████████] 100%
⚠️ Authentication     [███████████░░░░░░░░░]  55%
⚠️ Empleados/Nómina   [███████░░░░░░░░░░░░░]  38%
⚠️ Contratistas       [██████████░░░░░░░░░░]  50%
⚠️ Suscripciones      [█████░░░░░░░░░░░░░░░]  29%
❌ Bot OpenAI         [░░░░░░░░░░░░░░░░░░░░]   0%
```

---

## 🚀 PLAN DE EJECUCIÓN (7 LOTES)

### ⏱️ SEMANA 1: CRÍTICOS (12-15 horas)

#### ✅ LOTE 6.0.1: Authentication Completion (3-4h) 🔴 CRÍTICO

**Prioridad:** MÁXIMA - Bloquea frontend  
**Endpoints Faltantes:** 4 endpoints

| # | Endpoint | Método Legacy | Complejidad |
|---|----------|---------------|-------------|
| 1 | DELETE /api/auth/users/{userId}/credentials/{credentialId} | borrarUsuario() | 🟢 Baja |
| 2 | POST /api/auth/profile-info | agregarPerfilInfo() | 🟢 Baja |
| 3 | GET /api/auth/cuenta/{cuentaId} | getPerfilByID() | 🟢 Baja |
| 4 | PUT /api/auth/profile (mejorar) | actualizarPerfil() | 🟡 Media |

**Archivos a Crear:**

```
Application/Features/Authentication/
├── Commands/
│   ├── DeleteUserCredential/
│   │   ├── DeleteUserCredentialCommand.cs
│   │   ├── DeleteUserCredentialHandler.cs
│   │   └── DeleteUserCredentialValidator.cs
│   └── AddProfileInfo/
│       ├── AddProfileInfoCommand.cs
│       ├── AddProfileInfoHandler.cs
│       └── AddProfileInfoValidator.cs
└── Queries/
    └── GetCuentaById/
        ├── GetCuentaByIdQuery.cs
        └── GetCuentaByIdHandler.cs
```

**Testing:**

- [ ] Unit tests (Commands + Queries)
- [ ] Integration tests (AuthController)
- [ ] Swagger UI validation

---

#### ✅ LOTE 6.0.2: Empleados - Remuneraciones & TSS (4-5h) 🟠 ALTA

**Prioridad:** ALTA - Módulo más usado  
**Endpoints Faltantes:** 6 endpoints

| # | Endpoint | Descripción | API Externa |
|---|----------|-------------|-------------|
| 1 | GET /api/empleados/{id}/remuneraciones | Lista remuneraciones | No |
| 2 | DELETE /api/remuneraciones/{id} | Eliminar una remuneración | No |
| 3 | POST /api/empleados/{id}/remuneraciones/batch | Agregar múltiples | No |
| 4 | PUT /api/empleados/{id}/remuneraciones/batch | Actualizar todas | No |
| 5 | GET /api/empleados/consultar-padron/{cedula} | Validar cédula JCE | ✅ SÍ |
| 6 | GET /api/catalogos/deducciones-tss | Catálogo TSS | No |

**Dependencias Externas:**

- API Padrón Electoral: <https://abcportal.online/Sigeinfo/public/api>
- Credenciales en appsettings.json
- Implementar retry logic (Polly)

---

#### ✅ LOTE 6.0.4: Contratistas - Servicios (5-6h) 🟠 ALTA

**Prioridad:** ALTA - Marketplace de servicios  
**Endpoints Faltantes:** 5 endpoints

| # | Endpoint | Método Legacy |
|---|----------|---------------|
| 1 | GET /api/contratistas/{id}/servicios | getServicios() |
| 2 | POST /api/contratistas/{id}/servicios | agregarServicio() |
| 3 | DELETE /api/contratistas/{id}/servicios/{servicioId} | removerServicio() |
| 4 | POST /api/contratistas/{id}/activar | ActivarPerfil() |
| 5 | POST /api/contratistas/{id}/desactivar | DesactivarPerfil() |

**Entidad Nueva:**

- Contratistas_Servicios (many-to-many)
- Servicios_Catalogo (catálogo de servicios disponibles)

---

### ⏱️ SEMANA 2: COMPLEJOS (14-18 horas)

#### ✅ LOTE 6.0.3: Contrataciones Temporales (8-10h) 🔴 CRÍTICA

**Prioridad:** CRÍTICA - Lógica más compleja del sistema  
**Endpoints Faltantes:** 8 endpoints

⚠️ **ADVERTENCIA:** Múltiples tablas relacionadas, cascade deletes complejos

| # | Endpoint | Complejidad | Notas |
|---|----------|-------------|-------|
| 1 | GET /api/contrataciones/{id}/detalle/{detalleId}/pagos | 🟡 Media | Vista con joins |
| 2 | GET /api/contrataciones/recibos/{pagoId} | 🟡 Media | Header + Detalles |
| 3 | POST /api/contrataciones/{id}/detalle/{detalleId}/cancelar | 🟢 Baja | Update status |
| 4 | DELETE /api/contrataciones/recibos/{pagoId} | 🟡 Media | 2 tablas |
| 5 | DELETE /api/contrataciones/{id} | 🔴 Alta | CASCADE 3+ tablas |
| 6 | POST /api/contrataciones/{id}/calificar | 🟢 Baja | Update flag |
| 7 | GET /api/contrataciones/{id}/vista | 🟡 Media | Vista completa |
| 8 | POST /api/contrataciones/procesar-pago | 🔴 Alta | Multi-step logic |

**Testing Crítico:**

- Transacciones con rollback
- Cascade deletes en QA environment
- Performance con datos reales

---

#### ✅ LOTE 6.0.5: Suscripciones - Gestión Avanzada (4-5h) 🟡 MEDIA

**Prioridad:** MEDIA - Monetización  
**Endpoints Faltantes:** 3 endpoints

| # | Endpoint | Método Legacy |
|---|----------|---------------|
| 1 | PUT /api/auth/credentials/{id}/password | actualizarPassByID() |
| 2 | GET /api/auth/validar-correo?userID={id} | validarCorreoCuentaActual() |
| 3 | GET /api/suscripciones/{userId}/ventas | obtenerDetalleVentasBySuscripcion() |

---

#### ✅ LOTE 6.0.6: Bot & Configuración (2-3h) 🟡 MEDIA

**Prioridad:** BAJA - Feature opcional  
**Endpoints Faltantes:** 1 endpoint

| # | Endpoint | Descripción |
|---|----------|-------------|
| 1 | GET /api/configuracion/openai | Obtener config bot (API key, model) |

**Decisión Arquitectural:**

- [ ] Opción A: Endpoint público con Authorization
- [ ] Opción B: Mover a Infrastructure Layer (IOpenAiService)
- [ ] **Recomendación:** Opción B (config interna)

---

### ⏱️ SEMANA 3: VALIDACIÓN (6-8 horas)

#### ✅ LOTE 6.0.7: Testing & Validación (6-8h) ✅ OBLIGATORIO

**Prioridad:** MÁXIMA - Garantiza calidad  

**Checklist de Calidad:**

**1. Unit Testing (2h)**

- [ ] 80%+ code coverage en Application layer
- [ ] All Commands con business logic tested
- [ ] All Queries con filtros tested
- [ ] Validators tested con edge cases

**2. Integration Testing (2h)**

- [ ] All Controllers tested (Request → Response)
- [ ] Authorization scenarios tested
- [ ] Validation failures tested
- [ ] Error responses tested (400, 401, 404, 500)

**3. Manual Testing con Swagger UI (2h)**

- [ ] Crear Excel checklist: 81 endpoints × Status
- [ ] Probar con datos reales de DB
- [ ] Comparar responses con Legacy (screenshot)
- [ ] Validar tiempos de respuesta (<500ms p95)

**4. Security Audit (1h)**

- [ ] All endpoints con [Authorize]
- [ ] SQL injection impossible (EF Core)
- [ ] Input validation (FluentValidation)
- [ ] Sensitive data not logged

**5. Documentation (1h)**

- [ ] Swagger XML comments completos
- [ ] Postman collection exportada
- [ ] README actualizado con ejemplos
- [ ] Arquitectura diagrams actualizados

---

## 📋 CHECKLIST DE IMPLEMENTACIÓN (Por Endpoint)

### Template de Implementación

```markdown
## Endpoint: [HTTP METHOD] /api/[resource]

### 1. Análisis Legacy
- [x] Leer método Legacy completo
- [x] Documentar lógica de negocio
- [x] Identificar validaciones
- [x] Notar códigos de retorno

### 2. Domain Layer (si aplica)
- [ ] Agregar propiedades a Entity
- [ ] Crear Value Objects
- [ ] Agregar Domain Events

### 3. Application Layer
- [ ] Crear Command/Query
- [ ] Crear Handler
- [ ] Crear Validator (FluentValidation)
- [ ] Crear DTO (Request + Response)
- [ ] Agregar AutoMapper profile

### 4. API Layer
- [ ] Agregar método a Controller
- [ ] Agregar [Http*] attribute
- [ ] Agregar XML documentation
- [ ] Agregar [ProducesResponseType]
- [ ] Agregar [Authorize] si aplica

### 5. Testing
- [ ] Unit test del Handler
- [ ] Unit test del Validator
- [ ] Integration test del Controller
- [ ] Manual test con Swagger UI

### 6. Validación
- [ ] Compilación sin errores
- [ ] Comparar con Legacy (mismo input → output)
- [ ] Performance aceptable (<500ms)
- [ ] Security checklist passed
```

---

## 🎯 QUICK WIN: Empezar AHORA con LOTE 6.0.1

### Paso 1: Setup (5 min)

```bash
cd MiGenteEnLinea.Clean
git checkout -b feature/lote-6.0.1-authentication-completion
git pull origin main
```

### Paso 2: Crear Estructura (10 min)

```bash
# Commands
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/DeleteUserCredential
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/AddProfileInfo

# Queries
mkdir -p src/Core/MiGenteEnLinea.Application/Features/Authentication/Queries/GetCuentaById

# Touch files
cd src/Core/MiGenteEnLinea.Application/Features/Authentication/Commands/DeleteUserCredential
touch DeleteUserCredentialCommand.cs DeleteUserCredentialHandler.cs DeleteUserCredentialValidator.cs
```

### Paso 3: Implementar Endpoint #1 (30-45 min)

**DeleteUserCredentialCommand.cs:**

```csharp
using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUserCredential;

/// <summary>
/// Command para eliminar una credencial específica de un usuario
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.borrarUsuario(string userID, int credencialID)
/// 
/// Reglas de negocio:
/// - Usuario debe tener al menos 1 credencial activa (no puede eliminar la última)
/// - Solo el propio usuario o admin puede eliminar
/// </remarks>
public record DeleteUserCredentialCommand(
    string UserId,
    int CredentialId
) : IRequest<Unit>;
```

**DeleteUserCredentialHandler.cs:**

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Exceptions;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUserCredential;

public class DeleteUserCredentialHandler 
    : IRequestHandler<DeleteUserCredentialCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<DeleteUserCredentialHandler> _logger;

    public DeleteUserCredentialHandler(
        IApplicationDbContext context,
        ILogger<DeleteUserCredentialHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(
        DeleteUserCredentialCommand request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Eliminando credencial {CredentialId} del usuario {UserId}",
            request.CredentialId,
            request.UserId);

        // Validar que la credencial existe
        var credential = await _context.Credenciales
            .Where(c => c.id == request.CredentialId && c.userID == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (credential == null)
        {
            throw new NotFoundException(
                $"Credencial {request.CredentialId} no encontrada para usuario {request.UserId}");
        }

        // Validar que no es la última credencial activa
        var activeCredentialsCount = await _context.Credenciales
            .Where(c => c.userID == request.UserId && c.activo == true)
            .CountAsync(cancellationToken);

        if (activeCredentialsCount <= 1 && credential.activo == true)
        {
            throw new ValidationException(
                "No se puede eliminar la única credencial activa del usuario");
        }

        // Eliminar credencial (mismo patrón que Legacy)
        _context.Credenciales.Remove(credential);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Credencial {CredentialId} eliminada exitosamente",
            request.CredentialId);

        return Unit.Value;
    }
}
```

**DeleteUserCredentialValidator.cs:**

```csharp
using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUserCredential;

public class DeleteUserCredentialValidator 
    : AbstractValidator<DeleteUserCredentialCommand>
{
    public DeleteUserCredentialValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .Must(BeValidGuid).WithMessage("UserId debe ser un GUID válido");

        RuleFor(x => x.CredentialId)
            .GreaterThan(0).WithMessage("CredentialId debe ser mayor a 0");
    }

    private bool BeValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
```

**AuthController.cs (agregar método):**

```csharp
/// <summary>
/// Eliminar credencial específica de usuario
/// </summary>
/// <param name="userId">ID del usuario</param>
/// <param name="credentialId">ID de la credencial a eliminar</param>
/// <returns>204 No Content si se eliminó exitosamente</returns>
/// <response code="204">Credencial eliminada exitosamente</response>
/// <response code="400">Validación falló (ej: última credencial activa)</response>
/// <response code="404">Credencial no encontrada</response>
/// <response code="401">No autorizado</response>
[HttpDelete("users/{userId}/credentials/{credentialId}")]
[ProducesResponseType(StatusCodes.Status204NoContent)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status404NotFound)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<IActionResult> DeleteUserCredential(
    string userId, 
    int credentialId)
{
    _logger.LogInformation(
        "DELETE /api/auth/users/{UserId}/credentials/{CredentialId}",
        userId,
        credentialId);

    var command = new DeleteUserCredentialCommand(userId, credentialId);
    await _mediator.Send(command);

    return NoContent();
}
```

### Paso 4: Testing (20 min)

**Unit Test:**

```csharp
[Fact]
public async Task Handle_ValidCredential_ShouldDeleteSuccessfully()
{
    // Arrange
    var command = new DeleteUserCredentialCommand(
        userId: "550e8400-e29b-41d4-a716-446655440000",
        credentialId: 5);

    // Act
    var result = await _handler.Handle(command, CancellationToken.None);

    // Assert
    result.Should().Be(Unit.Value);
    _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
}

[Fact]
public async Task Handle_LastActiveCredential_ShouldThrowValidationException()
{
    // Arrange
    var command = new DeleteUserCredentialCommand(
        userId: "550e8400-e29b-41d4-a716-446655440000",
        credentialId: 1);

    // Act & Assert
    await Assert.ThrowsAsync<ValidationException>(
        () => _handler.Handle(command, CancellationToken.None));
}
```

### Paso 5: Build & Test (10 min)

```bash
# Build
dotnet build --no-restore

# Run unit tests
dotnet test --filter "FullyQualifiedName~DeleteUserCredential"

# Run API y probar con Swagger
dotnet run --project src/Presentation/MiGenteEnLinea.API
# Navegar a: http://localhost:5015/swagger
```

### Paso 6: Commit (5 min)

```bash
git add .
git commit -m "feat(auth): Implement DELETE /api/auth/users/{userId}/credentials/{credentialId}

- Add DeleteUserCredentialCommand with validation
- Add unit tests (2 scenarios)
- Update AuthController with new endpoint
- Migrated from Legacy: LoginService.borrarUsuario()

Refs: LOTE-6.0.1"

git push origin feature/lote-6.0.1-authentication-completion
```

---

## 📊 TRACKING DE PROGRESO

### Crear Issue en GitHub

```markdown
## LOTE 6.0.1: Authentication Completion

**Objetivo:** Completar todos los métodos del LoginService que faltan

**Endpoints:**
- [ ] DELETE /api/auth/users/{userId}/credentials/{credentialId}
- [ ] POST /api/auth/profile-info
- [ ] GET /api/auth/cuenta/{cuentaId}
- [ ] PUT /api/auth/profile (mejorar)

**Estimación:** 3-4 horas
**Prioridad:** 🔴 CRÍTICA

**Criterios de Aceptación:**
- [ ] 4 Commands/Queries implementados
- [ ] 4 endpoints en AuthController
- [ ] Unit tests (80%+ coverage)
- [ ] Integration tests passed
- [ ] Swagger documentation completa
- [ ] Manual testing con screenshots
```

---

## 🎉 RESULTADOS ESPERADOS

Al finalizar las 3 semanas:

### Métricas Objetivo

- ✅ **100% paridad** con Legacy (81/81 métodos)
- ✅ **80%+ code coverage** en tests
- ✅ **0 errores** de compilación
- ✅ **0 warnings** críticos
- ✅ **<500ms p95** en response times
- ✅ **100% endpoints** documentados en Swagger

### Entregables

1. **Código:**
   - 46 nuevos Commands/Queries
   - 46 nuevos endpoints en Controllers
   - 92+ unit tests
   - 46+ integration tests

2. **Documentación:**
   - Swagger UI completo con ejemplos
   - Postman collection para QA
   - README actualizado
   - Diagramas de arquitectura

3. **Calidad:**
   - Security audit passed
   - Performance benchmark passed
   - All tests green
   - Code review approved

---

## 🚀 COMANDO PARA EMPEZAR AHORA

```bash
# Clone y setup
cd MiGenteEnLinea.Clean
git checkout main
git pull origin main
git checkout -b feature/lote-6.0.1-authentication-completion

# Abrir VS Code
code .

# Mensaje para usuario
echo "✅ Setup completo!"
echo "📝 Siguiente: Leer Legacy LoginService.borrarUsuario() línea 80-90"
echo "🎯 Implementar: DeleteUserCredentialCommand"
echo "⏱️  Tiempo estimado: 45 minutos"
```

---

**¿Listo para empezar?** 💪

**Pregunta para Usuario:**

1. ¿Quieres que empiece con LOTE 6.0.1 ahora?
2. ¿Prefieres revisar primero qué endpoints del Legacy están realmente en uso en producción?
3. ¿Algún módulo tiene prioridad diferente por necesidades de negocio?

---

**Última Actualización:** 2025-01-15  
**Próximo Checkpoint:** Después de completar LOTE 6.0.1 (3-4 horas)
