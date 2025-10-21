# ⭐ PLAN DE EJECUCIÓN 2: LOTE 6 - CALIFICACIONES (RATINGS & REVIEWS)

**Prioridad:** 🔴 **ALTA** (Funcionalidad core para marketplace)  
**Esfuerzo Estimado:** 2-3 días (16-24 horas)  
**Estado:** ⏳ PENDIENTE  
**Dependencias:** Ninguna (puede iniciarse inmediatamente)

---

## 🎯 OBJETIVO

Migrar el sistema completo de calificaciones/reviews desde Legacy (`CalificacionesService.cs` - 63 líneas) a Clean Architecture usando el patrón CQRS con MediatR. Esto permitirá que contratistas muestren su reputación y que empleadores dejen reseñas.

---

## 📊 ANÁLISIS DE LEGACY

### Servicio Legacy Identificado

**Ubicación:** `Codigo Fuente Mi Gente/MiGente_Front/Services/CalificacionesService.cs`

**Métodos Legacy (3):**

```csharp
// 1. Obtener todas las calificaciones (público)
List<VCalificaciones> getTodas()

// 2. Obtener calificaciones por ID de contratista/empleado (con filtro)
List<VCalificaciones> getById(string id, string userID = null)

// 3. Obtener calificación específica
Calificaciones getCalificacionByID(int calificacionID)
```

### Estado en Clean Architecture

**✅ LO QUE YA EXISTE:**
- Entidad `Calificacion` en Domain Layer
- DbSet `Calificaciones` en DbContext
- View `VCalificaciones` (Read Model)

**❌ LO QUE FALTA:**
- Carpeta `Features/Calificaciones/`
- Commands (Create, Update, Delete)
- Queries (GetByContratista, GetByEmpleado, GetById, GetPromedio)
- DTOs (CalificacionDto, PromedioCalificacionDto)
- Controller (`CalificacionesController` con 7 endpoints REST)

---

## 📁 ESTRUCTURA A CREAR

```
src/Core/MiGenteEnLinea.Application/Features/Calificaciones/
├── Commands/
│   ├── CreateCalificacion/
│   │   ├── CreateCalificacionCommand.cs
│   │   ├── CreateCalificacionCommandHandler.cs
│   │   └── CreateCalificacionCommandValidator.cs
│   │
│   ├── UpdateCalificacion/
│   │   ├── UpdateCalificacionCommand.cs
│   │   ├── UpdateCalificacionCommandHandler.cs
│   │   └── UpdateCalificacionCommandValidator.cs
│   │
│   └── DeleteCalificacion/
│       ├── DeleteCalificacionCommand.cs
│       ├── DeleteCalificacionCommandHandler.cs
│       └── DeleteCalificacionCommandValidator.cs
│
├── Queries/
│   ├── GetCalificacionesByContratista/
│   │   ├── GetCalificacionesByContratistaQuery.cs
│   │   └── GetCalificacionesByContratistaQueryHandler.cs
│   │
│   ├── GetCalificacionesByEmpleado/
│   │   ├── GetCalificacionesByEmpleadoQuery.cs
│   │   └── GetCalificacionesByEmpleadoQueryHandler.cs
│   │
│   ├── GetCalificacionById/
│   │   ├── GetCalificacionByIdQuery.cs
│   │   └── GetCalificacionByIdQueryHandler.cs
│   │
│   └── GetPromedioCalificacion/
│       ├── GetPromedioCalificacionQuery.cs
│       └── GetPromedioCalificacionQueryHandler.cs
│
└── DTOs/
    ├── CalificacionDto.cs
    └── PromedioCalificacionDto.cs

src/Presentation/MiGenteEnLinea.API/Controllers/
└── CalificacionesController.cs

tests/MiGenteEnLinea.Application.Tests/Features/Calificaciones/
├── Commands/
│   ├── CreateCalificacionCommandTests.cs
│   ├── UpdateCalificacionCommandTests.cs
│   └── DeleteCalificacionCommandTests.cs
└── Queries/
    ├── GetCalificacionesByContratistaQueryTests.cs
    └── GetPromedioCalificacionQueryTests.cs
```

**Total Archivos:** 20 archivos (~1,200 líneas de código)

---

## 📋 PLAN DE IMPLEMENTACIÓN

### ⏱️ FASE 1: Commands (3 Commands × 3 archivos = 9 archivos, ~600 líneas) - 6 horas

#### Paso 1.1: CreateCalificacionCommand (2 horas)

**1.1.1: Command Class**

**Ubicación:** `Application/Features/Calificaciones/Commands/CreateCalificacion/CreateCalificacionCommand.cs`

```csharp
using MediatR;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.CreateCalificacion;

/// <summary>
/// Comando: Crear nueva calificación/review
/// </summary>
public record CreateCalificacionCommand : IRequest<int>
{
    /// <summary>
    /// ID del usuario que califica (Empleador)
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// ID del contratista calificado (opcional - mutuamente exclusivo con EmpleadoId)
    /// </summary>
    public int? ContratistaId { get; init; }

    /// <summary>
    /// ID del empleado calificado (opcional - mutuamente exclusivo con ContratistaId)
    /// </summary>
    public int? EmpleadoId { get; init; }

    /// <summary>
    /// Calificación (1-5 estrellas)
    /// </summary>
    public int Rating { get; init; }

    /// <summary>
    /// Comentario opcional de la calificación
    /// </summary>
    public string? Comentario { get; init; }

    /// <summary>
    /// Título breve de la calificación (opcional)
    /// </summary>
    public string? Titulo { get; init; }
}
```

**1.1.2: Command Handler**

**Ubicación:** `Application/Features/Calificaciones/Commands/CreateCalificacion/CreateCalificacionCommandHandler.cs`

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Calificaciones;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.CreateCalificacion;

public class CreateCalificacionCommandHandler : IRequestHandler<CreateCalificacionCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CreateCalificacionCommandHandler> _logger;

    public CreateCalificacionCommandHandler(
        IApplicationDbContext context,
        ILogger<CreateCalificacionCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<int> Handle(CreateCalificacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creando calificación: UserId={UserId}, Rating={Rating}, ContratistaId={ContratistaId}, EmpleadoId={EmpleadoId}",
            request.UserId,
            request.Rating,
            request.ContratistaId,
            request.EmpleadoId);

        // VALIDACIÓN: Solo puede calificar a uno (contratista O empleado, no ambos)
        if (request.ContratistaId.HasValue && request.EmpleadoId.HasValue)
        {
            throw new InvalidOperationException(
                "No se puede calificar a contratista y empleado simultáneamente");
        }

        if (!request.ContratistaId.HasValue && !request.EmpleadoId.HasValue)
        {
            throw new InvalidOperationException(
                "Debe especificar ContratistaId o EmpleadoId");
        }

        // VALIDACIÓN: Verificar que no exista calificación duplicada
        var existeCalificacion = await _context.Calificaciones
            .AnyAsync(c => 
                c.UserId == request.UserId &&
                ((request.ContratistaId.HasValue && c.ContratistaId == request.ContratistaId) ||
                 (request.EmpleadoId.HasValue && c.EmpleadoId == request.EmpleadoId)),
                cancellationToken);

        if (existeCalificacion)
        {
            throw new InvalidOperationException(
                "Ya existe una calificación de este usuario para este contratista/empleado");
        }

        // CREAR ENTIDAD
        var calificacion = Calificacion.Create(
            userId: request.UserId,
            rating: request.Rating,
            comentario: request.Comentario,
            titulo: request.Titulo,
            contratistaId: request.ContratistaId,
            empleadoId: request.EmpleadoId);

        _context.Calificaciones.Add(calificacion);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Calificación creada exitosamente. CalificacionId={CalificacionId}",
            calificacion.CalificacionId);

        return calificacion.CalificacionId;
    }
}
```

**1.1.3: Command Validator**

**Ubicación:** `Application/Features/Calificaciones/Commands/CreateCalificacion/CreateCalificacionCommandValidator.cs`

```csharp
using FluentValidation;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.CreateCalificacion;

public class CreateCalificacionCommandValidator : AbstractValidator<CreateCalificacionCommand>
{
    public CreateCalificacionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId es requerido")
            .Matches(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")
            .WithMessage("UserId debe ser un GUID válido");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("Rating debe estar entre 1 y 5 estrellas");

        RuleFor(x => x.Comentario)
            .MaximumLength(500)
            .WithMessage("Comentario no puede exceder 500 caracteres");

        RuleFor(x => x.Titulo)
            .MaximumLength(100)
            .WithMessage("Título no puede exceder 100 caracteres");

        // VALIDACIÓN: Al menos uno debe estar presente
        RuleFor(x => x)
            .Must(x => x.ContratistaId.HasValue || x.EmpleadoId.HasValue)
            .WithMessage("Debe especificar ContratistaId o EmpleadoId");

        // VALIDACIÓN: No pueden estar ambos presentes
        RuleFor(x => x)
            .Must(x => !(x.ContratistaId.HasValue && x.EmpleadoId.HasValue))
            .WithMessage("No puede especificar ContratistaId y EmpleadoId simultáneamente");

        When(x => x.ContratistaId.HasValue, () =>
        {
            RuleFor(x => x.ContratistaId!.Value)
                .GreaterThan(0)
                .WithMessage("ContratistaId debe ser mayor a 0");
        });

        When(x => x.EmpleadoId.HasValue, () =>
        {
            RuleFor(x => x.EmpleadoId!.Value)
                .GreaterThan(0)
                .WithMessage("EmpleadoId debe ser mayor a 0");
        });
    }
}
```

---

#### Paso 1.2: UpdateCalificacionCommand (1.5 horas)

**1.2.1: Command Class**

```csharp
using MediatR;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.UpdateCalificacion;

/// <summary>
/// Comando: Actualizar calificación existente (solo el autor puede editar)
/// </summary>
public record UpdateCalificacionCommand : IRequest<Unit>
{
    /// <summary>
    /// ID de la calificación a actualizar
    /// </summary>
    public int CalificacionId { get; init; }

    /// <summary>
    /// ID del usuario (debe ser el autor original)
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Nuevo rating (1-5 estrellas)
    /// </summary>
    public int Rating { get; init; }

    /// <summary>
    /// Nuevo comentario
    /// </summary>
    public string? Comentario { get; init; }

    /// <summary>
    /// Nuevo título
    /// </summary>
    public string? Titulo { get; init; }
}
```

**1.2.2: Command Handler**

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.UpdateCalificacion;

public class UpdateCalificacionCommandHandler : IRequestHandler<UpdateCalificacionCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<UpdateCalificacionCommandHandler> _logger;

    public UpdateCalificacionCommandHandler(
        IApplicationDbContext context,
        ILogger<UpdateCalificacionCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateCalificacionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Actualizando calificación: CalificacionId={CalificacionId}, UserId={UserId}",
            request.CalificacionId,
            request.UserId);

        // BUSCAR CALIFICACIÓN
        var calificacion = await _context.Calificaciones
            .FirstOrDefaultAsync(c => c.CalificacionId == request.CalificacionId, cancellationToken);

        if (calificacion == null)
        {
            throw new KeyNotFoundException(
                $"Calificación con ID {request.CalificacionId} no encontrada");
        }

        // VALIDACIÓN: Solo el autor puede editar
        if (calificacion.UserId != request.UserId)
        {
            throw new UnauthorizedAccessException(
                "Solo el autor puede editar esta calificación");
        }

        // ACTUALIZAR
        calificacion.ActualizarCalificacion(
            rating: request.Rating,
            comentario: request.Comentario,
            titulo: request.Titulo);

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Calificación actualizada exitosamente. CalificacionId={CalificacionId}",
            request.CalificacionId);

        return Unit.Value;
    }
}
```

**1.2.3: Command Validator** (similar a Create, omitido por brevedad)

---

#### Paso 1.3: DeleteCalificacionCommand (1.5 horas)

**1.3.1: Command Class**

```csharp
using MediatR;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Commands.DeleteCalificacion;

/// <summary>
/// Comando: Eliminar calificación (soft delete)
/// </summary>
public record DeleteCalificacionCommand : IRequest<Unit>
{
    public int CalificacionId { get; init; }
    public string UserId { get; init; } = string.Empty; // Solo autor puede eliminar
}
```

**1.3.2: Command Handler** (implementación similar a Update)

---

### ⏱️ FASE 2: Queries (4 Queries × 2 archivos = 8 archivos, ~400 líneas) - 5 horas

#### Paso 2.1: GetCalificacionesByContratistaQuery (1.5 horas)

**2.1.1: Query Class**

```csharp
using MediatR;
using MiGenteEnLinea.Application.Common.Models;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificacionesByContratista;

/// <summary>
/// Query: Obtener calificaciones de un contratista (con paginación)
/// </summary>
public record GetCalificacionesByContratistaQuery : IRequest<PaginatedList<CalificacionDto>>
{
    public int ContratistaId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// Filtrar por rating mínimo (opcional)
    /// </summary>
    public int? MinRating { get; init; }

    /// <summary>
    /// Ordenar por fecha (true = más recientes primero)
    /// </summary>
    public bool OrderByDateDesc { get; init; } = true;
}
```

**2.1.2: Query Handler**

```csharp
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Common.Models;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificacionesByContratista;

public class GetCalificacionesByContratistaQueryHandler 
    : IRequestHandler<GetCalificacionesByContratistaQuery, PaginatedList<CalificacionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCalificacionesByContratistaQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<CalificacionDto>> Handle(
        GetCalificacionesByContratistaQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Calificaciones
            .AsNoTracking()
            .Where(c => c.ContratistaId == request.ContratistaId);

        // FILTRO: Rating mínimo
        if (request.MinRating.HasValue)
        {
            query = query.Where(c => c.Rating >= request.MinRating.Value);
        }

        // ORDENAR
        query = request.OrderByDateDesc
            ? query.OrderByDescending(c => c.FechaCreacion)
            : query.OrderBy(c => c.FechaCreacion);

        // PROYECTAR A DTO y PAGINAR
        return await query
            .ProjectTo<CalificacionDto>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
```

---

#### Paso 2.2: GetCalificacionesByEmpleadoQuery (1 hora)

Similar a GetCalificacionesByContratista, pero filtra por `EmpleadoId`.

---

#### Paso 2.3: GetCalificacionByIdQuery (1 hora)

Query simple para obtener una calificación específica por ID.

---

#### Paso 2.4: GetPromedioCalificacionQuery (1.5 horas)

**2.4.1: Query Class**

```csharp
using MediatR;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetPromedioCalificacion;

/// <summary>
/// Query: Calcular promedio y estadísticas de calificaciones
/// </summary>
public record GetPromedioCalificacionQuery : IRequest<PromedioCalificacionDto>
{
    public int? ContratistaId { get; init; }
    public int? EmpleadoId { get; init; }
}
```

**2.4.2: Query Handler**

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetPromedioCalificacion;

public class GetPromedioCalificacionQueryHandler 
    : IRequestHandler<GetPromedioCalificacionQuery, PromedioCalificacionDto>
{
    private readonly IApplicationDbContext _context;

    public GetPromedioCalificacionQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PromedioCalificacionDto> Handle(
        GetPromedioCalificacionQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Calificaciones.AsNoTracking();

        // FILTRAR por contratista o empleado
        if (request.ContratistaId.HasValue)
            query = query.Where(c => c.ContratistaId == request.ContratistaId.Value);
        else if (request.EmpleadoId.HasValue)
            query = query.Where(c => c.EmpleadoId == request.EmpleadoId.Value);
        else
            throw new ArgumentException("Debe especificar ContratistaId o EmpleadoId");

        var calificaciones = await query.ToListAsync(cancellationToken);

        if (!calificaciones.Any())
        {
            return new PromedioCalificacionDto
            {
                PromedioGeneral = 0,
                TotalCalificaciones = 0,
                DistribucionEstrellas = new Dictionary<int, int>
                {
                    { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }
                }
            };
        }

        var promedio = calificaciones.Average(c => c.Rating);
        var distribucion = calificaciones
            .GroupBy(c => c.Rating)
            .ToDictionary(g => g.Key, g => g.Count());

        // Rellenar estrellas faltantes con 0
        for (int i = 1; i <= 5; i++)
        {
            if (!distribucion.ContainsKey(i))
                distribucion[i] = 0;
        }

        return new PromedioCalificacionDto
        {
            PromedioGeneral = Math.Round(promedio, 2),
            TotalCalificaciones = calificaciones.Count,
            DistribucionEstrellas = distribucion,
            ContratistaId = request.ContratistaId,
            EmpleadoId = request.EmpleadoId
        };
    }
}
```

---

### ⏱️ FASE 3: DTOs y Controller (3 archivos, ~200 líneas) - 3 horas

#### Paso 3.1: DTOs (1 hora)

**3.1.1: CalificacionDto**

```csharp
using System;

namespace MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

/// <summary>
/// DTO: Calificación con propiedades calculadas
/// </summary>
public class CalificacionDto
{
    public int CalificacionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int? ContratistaId { get; set; }
    public int? EmpleadoId { get; set; }
    public int Rating { get; set; }
    public string? Comentario { get; set; }
    public string? Titulo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }

    // PROPIEDADES CALCULADAS

    /// <summary>
    /// Tiempo transcurrido desde la calificación (ej: "hace 2 días")
    /// </summary>
    public string TiempoTranscurrido => CalcularTiempoTranscurrido();

    /// <summary>
    /// Indica si la calificación es reciente (menos de 7 días)
    /// </summary>
    public bool EsReciente => (DateTime.Now - FechaCreacion).TotalDays <= 7;

    /// <summary>
    /// Indica si la calificación ha sido editada
    /// </summary>
    public bool FueEditada => FechaModificacion.HasValue;

    private string CalcularTiempoTranscurrido()
    {
        var diferencia = DateTime.Now - FechaCreacion;

        if (diferencia.TotalMinutes < 60)
            return $"hace {(int)diferencia.TotalMinutes} minuto(s)";
        if (diferencia.TotalHours < 24)
            return $"hace {(int)diferencia.TotalHours} hora(s)";
        if (diferencia.TotalDays < 30)
            return $"hace {(int)diferencia.TotalDays} día(s)";
        if (diferencia.TotalDays < 365)
            return $"hace {(int)(diferencia.TotalDays / 30)} mes(es)";
        
        return $"hace {(int)(diferencia.TotalDays / 365)} año(s)";
    }
}
```

**3.1.2: PromedioCalificacionDto**

```csharp
using System.Collections.Generic;

namespace MiGenteEnLinea.Application.Features.Calificaciones.DTOs;

/// <summary>
/// DTO: Estadísticas de calificaciones
/// </summary>
public class PromedioCalificacionDto
{
    public double PromedioGeneral { get; set; }
    public int TotalCalificaciones { get; set; }

    /// <summary>
    /// Distribución de calificaciones por estrellas
    /// Key: Número de estrellas (1-5)
    /// Value: Cantidad de calificaciones
    /// </summary>
    public Dictionary<int, int> DistribucionEstrellas { get; set; } = new();

    public int? ContratistaId { get; set; }
    public int? EmpleadoId { get; set; }

    // PROPIEDADES CALCULADAS

    /// <summary>
    /// Porcentaje de calificaciones 5 estrellas
    /// </summary>
    public double PorcentajeExcelente =>
        TotalCalificaciones > 0
            ? Math.Round((double)DistribucionEstrellas[5] / TotalCalificaciones * 100, 1)
            : 0;

    /// <summary>
    /// Indica si tiene buena reputación (promedio >= 4.0)
    /// </summary>
    public bool TieneBuenaReputacion => PromedioGeneral >= 4.0;
}
```

---

#### Paso 3.2: CalificacionesController (2 horas)

```csharp
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Models;
using MiGenteEnLinea.Application.Features.Calificaciones.Commands.CreateCalificacion;
using MiGenteEnLinea.Application.Features.Calificaciones.Commands.UpdateCalificacion;
using MiGenteEnLinea.Application.Features.Calificaciones.Commands.DeleteCalificacion;
using MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificacionesByContratista;
using MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificacionesByEmpleado;
using MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetCalificacionById;
using MiGenteEnLinea.Application.Features.Calificaciones.Queries.GetPromedioCalificacion;
using MiGenteEnLinea.Application.Features.Calificaciones.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiGenteEnLinea.API.Controllers;

/// <summary>
/// Controller REST API para gestión de calificaciones/reviews
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CalificacionesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CalificacionesController> _logger;

    public CalificacionesController(IMediator mediator, ILogger<CalificacionesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Crear nueva calificación
    /// </summary>
    /// <param name="command">Datos de la calificación</param>
    /// <returns>ID de la calificación creada</returns>
    /// <response code="201">Calificación creada exitosamente</response>
    /// <response code="400">Datos inválidos o calificación duplicada</response>
    /// <response code="401">No autenticado</response>
    [HttpPost]
    [Authorize] // Solo usuarios autenticados pueden calificar
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<int>> CreateCalificacion([FromBody] CreateCalificacionCommand command)
    {
        _logger.LogInformation(
            "POST /api/calificaciones - UserId: {UserId}, Rating: {Rating}",
            command.UserId,
            command.Rating);

        // Asegurar que UserId es el usuario autenticado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        command = command with { UserId = userId ?? command.UserId };

        var calificacionId = await _mediator.Send(command);

        return CreatedAtAction(
            nameof(GetCalificacionById),
            new { id = calificacionId },
            new { calificacionId });
    }

    /// <summary>
    /// Obtener calificación por ID
    /// </summary>
    /// <param name="id">ID de la calificación</param>
    /// <returns>Detalles de la calificación</returns>
    /// <response code="200">Calificación encontrada</response>
    /// <response code="404">Calificación no encontrada</response>
    [HttpGet("{id}")]
    [AllowAnonymous] // Público
    [ProducesResponseType(typeof(CalificacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CalificacionDto>> GetCalificacionById(int id)
    {
        var query = new GetCalificacionByIdQuery { CalificacionId = id };
        var calificacion = await _mediator.Send(query);

        if (calificacion == null)
            return NotFound(new { message = $"Calificación con ID {id} no encontrada" });

        return Ok(calificacion);
    }

    /// <summary>
    /// Obtener calificaciones de un contratista (paginado)
    /// </summary>
    /// <param name="contratistaId">ID del contratista</param>
    /// <param name="pageNumber">Número de página (default: 1)</param>
    /// <param name="pageSize">Tamaño de página (default: 10, max: 50)</param>
    /// <param name="minRating">Filtrar por rating mínimo (opcional)</param>
    /// <returns>Lista paginada de calificaciones</returns>
    /// <response code="200">Calificaciones encontradas</response>
    [HttpGet("contratista/{contratistaId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedList<CalificacionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<CalificacionDto>>> GetCalificacionesByContratista(
        int contratistaId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? minRating = null)
    {
        var query = new GetCalificacionesByContratistaQuery
        {
            ContratistaId = contratistaId,
            PageNumber = pageNumber,
            PageSize = Math.Min(pageSize, 50), // Max 50 por página
            MinRating = minRating
        };

        var calificaciones = await _mediator.Send(query);
        return Ok(calificaciones);
    }

    /// <summary>
    /// Obtener calificaciones de un empleado (paginado)
    /// </summary>
    [HttpGet("empleado/{empleadoId}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PaginatedList<CalificacionDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PaginatedList<CalificacionDto>>> GetCalificacionesByEmpleado(
        int empleadoId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetCalificacionesByEmpleadoQuery
        {
            EmpleadoId = empleadoId,
            PageNumber = pageNumber,
            PageSize = Math.Min(pageSize, 50)
        };

        var calificaciones = await _mediator.Send(query);
        return Ok(calificaciones);
    }

    /// <summary>
    /// Obtener promedio y estadísticas de calificaciones
    /// </summary>
    /// <param name="contratistaId">ID del contratista (mutuamente exclusivo con empleadoId)</param>
    /// <param name="empleadoId">ID del empleado (mutuamente exclusivo con contratistaId)</param>
    /// <returns>Estadísticas de calificaciones</returns>
    /// <response code="200">Estadísticas calculadas</response>
    /// <response code="400">Parámetros inválidos</response>
    [HttpGet("promedio")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PromedioCalificacionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PromedioCalificacionDto>> GetPromedioCalificacion(
        [FromQuery] int? contratistaId = null,
        [FromQuery] int? empleadoId = null)
    {
        if (!contratistaId.HasValue && !empleadoId.HasValue)
            return BadRequest(new { message = "Debe especificar contratistaId o empleadoId" });

        if (contratistaId.HasValue && empleadoId.HasValue)
            return BadRequest(new { message = "No puede especificar ambos IDs simultáneamente" });

        var query = new GetPromedioCalificacionQuery
        {
            ContratistaId = contratistaId,
            EmpleadoId = empleadoId
        };

        var promedio = await _mediator.Send(query);
        return Ok(promedio);
    }

    /// <summary>
    /// Actualizar calificación existente (solo el autor)
    /// </summary>
    /// <param name="id">ID de la calificación</param>
    /// <param name="command">Nuevos datos</param>
    /// <returns>Sin contenido si exitoso</returns>
    /// <response code="204">Actualizado exitosamente</response>
    /// <response code="401">No autorizado</response>
    /// <response code="404">Calificación no encontrada</response>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCalificacion(int id, [FromBody] UpdateCalificacionCommand command)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        command = command with { CalificacionId = id, UserId = userId ?? command.UserId };

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Eliminar calificación (solo el autor)
    /// </summary>
    /// <param name="id">ID de la calificación</param>
    /// <returns>Sin contenido si exitoso</returns>
    /// <response code="204">Eliminado exitosamente</response>
    /// <response code="401">No autorizado</response>
    /// <response code="404">Calificación no encontrada</response>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCalificacion(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var command = new DeleteCalificacionCommand
        {
            CalificacionId = id,
            UserId = userId ?? string.Empty
        };

        await _mediator.Send(command);
        return NoContent();
    }
}
```

---

### ⏱️ FASE 4: Testing (4 horas)

#### Paso 4.1: Unit Tests para Commands (2 horas)

Crear tests para:
- CreateCalificacionCommand (validación, duplicados, etc.)
- UpdateCalificacionCommand (autorización, etc.)
- DeleteCalificacionCommand

#### Paso 4.2: Unit Tests para Queries (1 hora)

Crear tests para:
- GetPromedioCalificacionQuery (cálculos correctos)
- Paginación funciona correctamente

#### Paso 4.3: Integration Tests con Swagger UI (1 hora)

Testing manual de todos los 7 endpoints REST.

---

## ✅ CHECKLIST DE COMPLETADO

### Fase 1: Commands (6 horas)

- [ ] CreateCalificacionCommand + Handler + Validator (3 archivos)
- [ ] UpdateCalificacionCommand + Handler + Validator (3 archivos)
- [ ] DeleteCalificacionCommand + Handler + Validator (3 archivos)
- [ ] Compilación sin errores

### Fase 2: Queries (5 horas)

- [ ] GetCalificacionesByContratistaQuery + Handler (2 archivos)
- [ ] GetCalificacionesByEmpleadoQuery + Handler (2 archivos)
- [ ] GetCalificacionByIdQuery + Handler (2 archivos)
- [ ] GetPromedioCalificacionQuery + Handler (2 archivos)
- [ ] Compilación sin errores

### Fase 3: DTOs y Controller (3 horas)

- [ ] CalificacionDto con propiedades calculadas
- [ ] PromedioCalificacionDto con estadísticas
- [ ] CalificacionesController (7 endpoints REST)
- [ ] Compilación sin errores

### Fase 4: Testing (4 horas)

- [ ] Unit tests para Commands
- [ ] Unit tests para Queries
- [ ] Integration tests con Swagger UI
- [ ] Testing end-to-end (crear → listar → actualizar → eliminar)

---

## 📊 ENDPOINTS REST FINALES (7 ENDPOINTS)

| Método | Endpoint | Autenticación | Descripción |
|--------|----------|---------------|-------------|
| POST | `/api/calificaciones` | ✅ Required | Crear calificación |
| GET | `/api/calificaciones/{id}` | ❌ Público | Obtener calificación por ID |
| GET | `/api/calificaciones/contratista/{id}` | ❌ Público | Listar calificaciones de contratista |
| GET | `/api/calificaciones/empleado/{id}` | ❌ Público | Listar calificaciones de empleado |
| GET | `/api/calificaciones/promedio` | ❌ Público | Obtener promedio y estadísticas |
| PUT | `/api/calificaciones/{id}` | ✅ Required | Actualizar calificación (solo autor) |
| DELETE | `/api/calificaciones/{id}` | ✅ Required | Eliminar calificación (solo autor) |

---

## 📈 MÉTRICAS DE ÉXITO

| Métrica | Objetivo | Cómo Verificar |
|---------|----------|----------------|
| **Compilación** | 0 errores | `dotnet build` exitoso |
| **Unit Tests** | 80%+ coverage | `dotnet test` todos pasan |
| **Performance** | < 200ms (queries) | Swagger UI timing |
| **API Coverage** | 7/7 endpoints | Swagger UI |
| **Calificaciones creadas** | Funciona end-to-end | Testing manual |

---

## 📁 ARCHIVOS CREADOS (RESUMEN)

```
Total Archivos Nuevos: 20 archivos (~1,200 líneas)

Commands: 9 archivos (~600 líneas)
Queries: 8 archivos (~400 líneas)
DTOs: 2 archivos (~100 líneas)
Controller: 1 archivo (~100 líneas)
```

---

## 🚀 PRÓXIMOS PASOS (POST-IMPLEMENTACIÓN)

1. ✅ **Implementar JWT Tokens** (PLAN 3)
2. ⏳ Agregar notificaciones cuando reciben calificación
3. ⏳ Implementar moderación de comentarios inapropiados
4. ⏳ Agregar soporte para respuestas a calificaciones
5. ⏳ Implementar sistema de "calificación verificada"

---

**Elaborado por:** GitHub Copilot AI Agent  
**Fecha:** 2025-01-13  
**Versión:** 1.0  
**Estado:** ⏳ PENDIENTE DE EJECUCIÓN
