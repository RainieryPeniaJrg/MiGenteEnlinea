# 🚀 LOTE 2: EMPLEADORES - CRUD BÁSICO

**Fecha de Inicio:** 13 de octubre, 2025  
**Migración desde:** Legacy ASP.NET Web Forms → Clean Architecture .NET 8.0  
**Patrón:** CQRS con MediatR  
**Estado:** 🔄 EN PROGRESO

---

## 📊 ANÁLISIS DE LEGACY

### Archivos Legacy Analizados

| Archivo | Ubicación | Funcionalidad |
|---------|-----------|---------------|
| `MiPerfilEmpleador.aspx.cs` | Empleador/ | Actualizar perfil de empleador (perfilesInfo + Cuentas) |
| `colaboradores.aspx.cs` | Empleador/ | Listar y gestionar empleados (NO empleadores) |
| `LoginService.asmx.cs` | Services/ | getPerfilInfo() - Obtener VPerfiles |
| **Tabla Legacy:** `Ofertantes` | Data/DataModel.edmx | Entidad de empleadores |
| **View Legacy:** `VPerfiles` | ReadModel | Vista para queries |

### ⚠️ HALLAZGOS CRÍTICOS

#### 1. Confusión de Terminología
- **Legacy:** "Ofertantes" = Empleadores (tabla)
- **Clean:** "Empleador" (DDD entity)
- **UI Legacy:** "Mi Perfil Empleador" actualiza `perfilesInfo` + `Cuentas` (NO Ofertantes directamente)

#### 2. Arquitectura Legacy
```
MiPerfilEmpleador.aspx.cs:
1. Actualiza perfilesInfo (tipoIdentificacion, identificacion, direccion, nombreComercial, etc.)
2. Actualiza Cuentas (Nombre, Apellido, Email, telefono1, telefono2)
3. NO actualiza tabla Ofertantes

colaboradores.aspx.cs:
- GetColaboradores() → gestiona EMPLEADOS (tabla Empleados)
- NO gestiona empleadores (tabla Ofertantes)
```

**CONCLUSIÓN:** La tabla Legacy "Ofertantes" parece tener poco uso directo en el código. La gestión del perfil del empleador se hace vía `perfilesInfo` + `Cuentas`.

#### 3. Entidad Empleador (Clean DDD)

**Ubicación:** `Domain/Entities/Empleadores/Empleador.cs`

**Propiedades:**
```csharp
public sealed class Empleador : AggregateRoot
{
    public int Id { get; private set; }                      // ofertanteID
    public DateTime? FechaPublicacion { get; private set; }   // fechaPublicacion
    public string UserId { get; private set; }                // userID (FK a Credencial)
    public string? Habilidades { get; private set; }          // habilidades (max 200)
    public string? Experiencia { get; private set; }          // experiencia (max 200)
    public string? Descripcion { get; private set; }          // descripcion (max 500)
    public byte[]? Foto { get; private set; }                 // foto (image binary)
}
```

**Domain Methods:**
- `Create(userId, habilidades, experiencia, descripcion)` - Factory method
- `ActualizarPerfil(habilidades, experiencia, descripcion)` - Update profile
- `ActualizarFoto(byte[] foto)` - Update photo (max 5MB)
- `EliminarFoto()` - Remove photo
- `PuedePublicarOfertas()` - Business rule validation

---

## 🎯 MAPEO: LEGACY → CLEAN ARCHITECTURE

### Operaciones Identificadas

| # | Operación Legacy | Command/Query Clean | Prioridad |
|---|------------------|---------------------|-----------|
| 1 | Crear perfil empleador (al registrar) | `CreateEmpleadorCommand` | 🔴 CRÍTICA |
| 2 | Actualizar perfil empleador (habilidades, experiencia) | `UpdateEmpleadorCommand` | 🔴 CRÍTICA |
| 3 | Actualizar foto/logo | `UpdateEmpleadorFotoCommand` | 🟡 MEDIA |
| 4 | Obtener perfil completo por userId | `GetEmpleadorByUserIdQuery` | 🔴 CRÍTICA |
| 5 | Obtener perfil por empleadorId | `GetEmpleadorByIdQuery` | 🟢 BAJA |
| 6 | Buscar empleadores (filtros) | `SearchEmpleadoresQuery` | 🟢 BAJA |
| 7 | Eliminar empleador (soft delete) | `DeleteEmpleadorCommand` | 🟢 BAJA |

### Priorización para Implementación

#### 🔥 FASE 1: CRUD Básico (Esta Sesión)
1. **CreateEmpleadorCommand** - Crear empleador al registrar usuario
2. **GetEmpleadorByUserIdQuery** - Obtener perfil por userId
3. **UpdateEmpleadorCommand** - Actualizar habilidades, experiencia, descripción
4. **UpdateEmpleadorFotoCommand** - Actualizar foto/logo

**Tiempo estimado:** 6-8 horas

#### ⏳ FASE 2: Búsqueda y Admin (Próxima Sesión)
5. **GetEmpleadorByIdQuery** - Obtener por ID directo
6. **SearchEmpleadoresQuery** - Búsqueda con filtros y paginación
7. **DeleteEmpleadorCommand** - Soft delete

**Tiempo estimado:** 2-3 horas

---

## 📐 ESTRUCTURA A CREAR

### 1. Commands (4 archivos x 3 = 12 archivos)

```
src/Core/MiGenteEnLinea.Application/Features/Empleadores/
├── Commands/
│   ├── CreateEmpleador/
│   │   ├── CreateEmpleadorCommand.cs
│   │   ├── CreateEmpleadorCommandHandler.cs
│   │   └── CreateEmpleadorCommandValidator.cs
│   ├── UpdateEmpleador/
│   │   ├── UpdateEmpleadorCommand.cs
│   │   ├── UpdateEmpleadorCommandHandler.cs
│   │   └── UpdateEmpleadorCommandValidator.cs
│   ├── UpdateEmpleadorFoto/
│   │   ├── UpdateEmpleadorFotoCommand.cs
│   │   ├── UpdateEmpleadorFotoCommandHandler.cs
│   │   └── UpdateEmpleadorFotoCommandValidator.cs
│   └── DeleteEmpleador/
│       ├── DeleteEmpleadorCommand.cs
│       ├── DeleteEmpleadorCommandHandler.cs
│       └── DeleteEmpleadorCommandValidator.cs
```

### 2. Queries (3 archivos x 2 = 6 archivos)

```
├── Queries/
│   ├── GetEmpleadorByUserId/
│   │   ├── GetEmpleadorByUserIdQuery.cs
│   │   └── GetEmpleadorByUserIdQueryHandler.cs
│   ├── GetEmpleadorById/
│   │   ├── GetEmpleadorByIdQuery.cs
│   │   └── GetEmpleadorByIdQueryHandler.cs
│   └── SearchEmpleadores/
│       ├── SearchEmpleadoresQuery.cs
│       └── SearchEmpleadoresQueryHandler.cs
```

### 3. DTOs (3 archivos)

```
└── DTOs/
    ├── EmpleadorDto.cs
    ├── CreateEmpleadorDto.cs
    └── UpdateEmpleadorDto.cs
```

### 4. Controller (1 archivo)

```
src/Presentation/MiGenteEnLinea.API/Controllers/
└── EmpleadoresController.cs (6 endpoints REST)
```

**Total archivos:** 22 archivos  
**Estimación líneas:** ~1,800 líneas

---

## 🔧 DETALLES DE IMPLEMENTACIÓN

### Command 1: CreateEmpleadorCommand

**Réplica de:** Registro de usuario (implícito al crear cuenta tipo Empleador)

**Input:**
```csharp
public record CreateEmpleadorCommand(
    string UserId,
    string? Habilidades = null,
    string? Experiencia = null,
    string? Descripcion = null
) : IRequest<int>; // Retorna empleadorId
```

**Lógica del Handler:**
```csharp
public async Task<int> Handle(CreateEmpleadorCommand request, CancellationToken ct)
{
    // PASO 1: Validar que userId existe en Credenciales
    var credencial = await _context.Credenciales
        .FirstOrDefaultAsync(c => c.UserId == request.UserId, ct);
    
    if (credencial == null)
        throw new NotFoundException($"Usuario {request.UserId} no encontrado");
    
    // PASO 2: Validar que no exista empleador para ese userId
    var existeEmpleador = await _context.Empleadores
        .AnyAsync(e => e.UserId == request.UserId, ct);
    
    if (existeEmpleador)
        throw new InvalidOperationException($"Ya existe un empleador para el usuario {request.UserId}");
    
    // PASO 3: Crear empleador con factory method de dominio
    var empleador = Empleador.Create(
        userId: request.UserId,
        habilidades: request.Habilidades,
        experiencia: request.Experiencia,
        descripcion: request.Descripcion
    );
    
    // PASO 4: Agregar a DbContext
    _context.Empleadores.Add(empleador);
    
    // PASO 5: Guardar cambios
    await _context.SaveChangesAsync(ct);
    
    _logger.LogInformation("Empleador creado: {EmpleadorId} para usuario {UserId}", 
        empleador.Id, request.UserId);
    
    return empleador.Id;
}
```

**Validaciones (FluentValidation):**
```csharp
public class CreateEmpleadorCommandValidator : AbstractValidator<CreateEmpleadorCommand>
{
    public CreateEmpleadorCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .Must(BeValidGuid).WithMessage("UserId debe ser un GUID válido");
        
        RuleFor(x => x.Habilidades)
            .MaximumLength(200).WithMessage("Habilidades no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Habilidades));
        
        RuleFor(x => x.Experiencia)
            .MaximumLength(200).WithMessage("Experiencia no puede exceder 200 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Experiencia));
        
        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("Descripcion no puede exceder 500 caracteres")
            .When(x => !string.IsNullOrWhiteSpace(x.Descripcion));
    }
    
    private bool BeValidGuid(string userId)
    {
        return Guid.TryParse(userId, out _);
    }
}
```

**Endpoint:**
```http
POST /api/empleadores
Content-Type: application/json

{
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "habilidades": "Gestión de proyectos, Recursos humanos",
  "experiencia": "15 años en construcción",
  "descripcion": "Empresa líder en construcción residencial en Santo Domingo"
}

Response 201 Created:
{
  "empleadorId": 123,
  "message": "Empleador creado exitosamente"
}
```

---

### Query 1: GetEmpleadorByUserIdQuery

**Réplica de:** `LoginService.getPerfilInfo(userID)` pero específico para Empleador

**Input:**
```csharp
public record GetEmpleadorByUserIdQuery(string UserId) : IRequest<EmpleadorDto?>;
```

**Lógica del Handler:**
```csharp
public async Task<EmpleadorDto?> Handle(GetEmpleadorByUserIdQuery request, CancellationToken ct)
{
    var empleador = await _context.Empleadores
        .AsNoTracking()
        .Where(e => e.UserId == request.UserId)
        .FirstOrDefaultAsync(ct);
    
    if (empleador == null)
    {
        _logger.LogWarning("Empleador no encontrado para userId: {UserId}", request.UserId);
        return null;
    }
    
    // Mapear a DTO
    return new EmpleadorDto
    {
        EmpleadorId = empleador.Id,
        UserId = empleador.UserId,
        FechaPublicacion = empleador.FechaPublicacion,
        Habilidades = empleador.Habilidades,
        Experiencia = empleador.Experiencia,
        Descripcion = empleador.Descripcion,
        TieneFoto = empleador.Foto != null && empleador.Foto.Length > 0
    };
}
```

**DTO:**
```csharp
public sealed class EmpleadorDto
{
    public int EmpleadorId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public DateTime? FechaPublicacion { get; init; }
    public string? Habilidades { get; init; }
    public string? Experiencia { get; init; }
    public string? Descripcion { get; init; }
    public bool TieneFoto { get; init; }
    
    // NO incluir byte[] Foto en DTO (muy grande)
    // Usar endpoint separado: GET /api/empleadores/{id}/foto
}
```

**Endpoint:**
```http
GET /api/empleadores/by-user/{userId}

Response 200 OK:
{
  "empleadorId": 123,
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "fechaPublicacion": "2025-01-15T10:30:00Z",
  "habilidades": "Gestión de proyectos, Recursos humanos",
  "experiencia": "15 años en construcción",
  "descripcion": "Empresa líder en construcción residencial",
  "tieneFoto": true
}
```

---

## 📋 CHECKLIST DE IMPLEMENTACIÓN

### FASE 1: Setup (15 minutos)
- [ ] Crear estructura de carpetas en Application/Features/Empleadores
- [ ] Actualizar IApplicationDbContext con DbSet<Empleador>
- [ ] Verificar que Empleador entity existe en Domain Layer

### FASE 2: Commands (3-4 horas)
- [ ] CreateEmpleadorCommand + Handler + Validator (1h)
- [ ] UpdateEmpleadorCommand + Handler + Validator (1h)
- [ ] UpdateEmpleadorFotoCommand + Handler + Validator (1h)
- [ ] DeleteEmpleadorCommand + Handler + Validator (30 min)

### FASE 3: Queries (2-3 horas)
- [ ] GetEmpleadorByUserIdQuery + Handler (45 min)
- [ ] GetEmpleadorByIdQuery + Handler (30 min)
- [ ] SearchEmpleadoresQuery + Handler (1.5h - con paginación)

### FASE 4: DTOs (30 minutos)
- [ ] EmpleadorDto
- [ ] CreateEmpleadorDto
- [ ] UpdateEmpleadorDto

### FASE 5: Controller (1 hora)
- [ ] EmpleadoresController con 6 endpoints
- [ ] Documentación Swagger con XML comments

### FASE 6: Testing (1 hora)
- [ ] Compilar sin errores
- [ ] Ejecutar API (dotnet run)
- [ ] Probar TODOS los endpoints con Swagger UI
- [ ] Comparar resultados con Legacy (si aplica)

### FASE 7: Documentación (30 minutos)
- [ ] Crear LOTE_2_EMPLEADORES_COMPLETADO.md
- [ ] Métricas: archivos, líneas, tiempo
- [ ] Screenshots de Swagger
- [ ] Comparación Legacy vs Clean

---

## 🚀 SIGUIENTE PASO

**Ejecutar:** Implementación de CreateEmpleadorCommand

```bash
# 1. Crear carpeta estructura
mkdir -p "src/Core/MiGenteEnLinea.Application/Features/Empleadores/Commands/CreateEmpleador"

# 2. Crear archivos
# - CreateEmpleadorCommand.cs
# - CreateEmpleadorCommandHandler.cs
# - CreateEmpleadorCommandValidator.cs
```

---

**Estado:** ✅ Análisis completado - Listo para implementación  
**Próximo:** Crear CreateEmpleadorCommand
