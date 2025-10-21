using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Features.Contrataciones.Commands.AcceptContratacion;
using MiGenteEnLinea.Application.Features.Contrataciones.Commands.CancelContratacion;
using MiGenteEnLinea.Application.Features.Contrataciones.Commands.CompleteContratacion;
using MiGenteEnLinea.Application.Features.Contrataciones.Commands.CreateContratacion;
using MiGenteEnLinea.Application.Features.Contrataciones.Commands.RejectContratacion;
using MiGenteEnLinea.Application.Features.Contrataciones.Commands.StartContratacion;
using MiGenteEnLinea.Application.Features.Contrataciones.Queries.GetContratacionById;
using MiGenteEnLinea.Application.Features.Contrataciones.Queries.GetContrataciones;

namespace MiGenteEnLinea.API.Controllers;

/// <summary>
/// Controller para gestión de contrataciones entre empleadores y contratistas.
/// 
/// WORKFLOW DE CONTRATACIÓN:
/// 1. Empleador crea propuesta (POST /api/contrataciones) → Estado: Pendiente
/// 2. Contratista acepta (PUT /api/contrataciones/{id}/accept) → Estado: Aceptada
///    O rechaza (PUT /api/contrataciones/{id}/reject) → Estado: Rechazada
/// 3. Trabajo inicia (PUT /api/contrataciones/{id}/start) → Estado: En Progreso
/// 4. Trabajo completa (PUT /api/contrataciones/{id}/complete) → Estado: Completada
/// 5. Empleador califica (POST /api/calificaciones) → Calificado = true
/// 
/// ESTADOS:
/// - 1 = Pendiente (propuesta enviada)
/// - 2 = Aceptada (contratista aceptó)
/// - 3 = En Progreso (trabajo iniciado)
/// - 4 = Completada (trabajo finalizado)
/// - 5 = Cancelada (cancelada por cualquier razón)
/// - 6 = Rechazada (contratista rechazó)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContratacionesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ContratacionesController> _logger;

    public ContratacionesController(
        IMediator mediator,
        ILogger<ContratacionesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Crea una nueva propuesta de contratación.
    /// </summary>
    /// <param name="command">Datos de la contratación</param>
    /// <returns>ID del detalle de contratación creado</returns>
    /// <response code="200">Contratación creada exitosamente</response>
    /// <response code="400">Datos inválidos</response>
    /// <response code="401">No autenticado</response>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Create([FromBody] CreateContratacionCommand command)
    {
        _logger.LogInformation("Creating new contratacion");

        try
        {
            var detalleId = await _mediator.Send(command);
            return Ok(detalleId);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating contratacion");
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene una contratación específica por ID.
    /// </summary>
    /// <param name="id">ID del detalle de contratación</param>
    /// <returns>Detalles de la contratación</returns>
    /// <response code="200">Contratación encontrada</response>
    /// <response code="404">Contratación no encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Getting contratacion {Id}", id);

        var query = new GetContratacionByIdQuery { DetalleId = id };
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new { message = $"Contratación con ID {id} no encontrada" });
        }

        return Ok(result);
    }

    /// <summary>
    /// Obtiene lista de contrataciones con filtros opcionales.
    /// </summary>
    /// <param name="query">Filtros de búsqueda</param>
    /// <returns>Lista de contrataciones</returns>
    /// <response code="200">Lista de contrataciones (puede estar vacía)</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetContratacionesQuery query)
    {
        _logger.LogInformation("Getting contrataciones with filters");

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Contratista acepta una propuesta de contratación.
    /// </summary>
    /// <param name="id">ID del detalle de contratación</param>
    /// <returns>Confirmación de aceptación</returns>
    /// <response code="200">Contratación aceptada exitosamente</response>
    /// <response code="400">No se puede aceptar (estado inválido)</response>
    /// <response code="404">Contratación no encontrada</response>
    [HttpPut("{id}/accept")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Accept(int id)
    {
        _logger.LogInformation("Accepting contratacion {Id}", id);

        try
        {
            var command = new AcceptContratacionCommand { DetalleId = id };
            await _mediator.Send(command);
            return Ok(new { message = "Contratación aceptada exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot accept contratacion {Id}", id);
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Contratista rechaza una propuesta de contratación.
    /// </summary>
    /// <param name="id">ID del detalle de contratación</param>
    /// <param name="command">Motivo del rechazo</param>
    /// <returns>Confirmación de rechazo</returns>
    /// <response code="200">Contratación rechazada exitosamente</response>
    /// <response code="400">No se puede rechazar (estado inválido o motivo vacío)</response>
    /// <response code="404">Contratación no encontrada</response>
    [HttpPut("{id}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(int id, [FromBody] RejectContratacionCommand command)
    {
        _logger.LogInformation("Rejecting contratacion {Id}", id);

        if (command.DetalleId != id)
        {
            return BadRequest(new { error = "ID mismatch" });
        }

        try
        {
            await _mediator.Send(command);
            return Ok(new { message = "Contratación rechazada exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot reject contratacion {Id}", id);
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Inicia el trabajo de una contratación aceptada.
    /// </summary>
    /// <param name="id">ID del detalle de contratación</param>
    /// <returns>Confirmación de inicio</returns>
    /// <response code="200">Trabajo iniciado exitosamente</response>
    /// <response code="400">No se puede iniciar (estado inválido)</response>
    /// <response code="404">Contratación no encontrada</response>
    [HttpPut("{id}/start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Start(int id)
    {
        _logger.LogInformation("Starting contratacion {Id}", id);

        try
        {
            var command = new StartContratacionCommand { DetalleId = id };
            await _mediator.Send(command);
            return Ok(new { message = "Trabajo iniciado exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot start contratacion {Id}", id);
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Marca una contratación como completada.
    /// </summary>
    /// <param name="id">ID del detalle de contratación</param>
    /// <returns>Confirmación de completado</returns>
    /// <response code="200">Trabajo completado exitosamente</response>
    /// <response code="400">No se puede completar (estado inválido)</response>
    /// <response code="404">Contratación no encontrada</response>
    [HttpPut("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Complete(int id)
    {
        _logger.LogInformation("Completing contratacion {Id}", id);

        try
        {
            var command = new CompleteContratacionCommand { DetalleId = id };
            await _mediator.Send(command);
            return Ok(new { message = "Trabajo completado exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot complete contratacion {Id}", id);
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Cancela una contratación.
    /// </summary>
    /// <param name="id">ID del detalle de contratación</param>
    /// <param name="command">Motivo de cancelación</param>
    /// <returns>Confirmación de cancelación</returns>
    /// <response code="200">Contratación cancelada exitosamente</response>
    /// <response code="400">No se puede cancelar (estado Completada o motivo vacío)</response>
    /// <response code="404">Contratación no encontrada</response>
    [HttpPut("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(int id, [FromBody] CancelContratacionCommand command)
    {
        _logger.LogInformation("Canceling contratacion {Id}", id);

        if (command.DetalleId != id)
        {
            return BadRequest(new { error = "ID mismatch" });
        }

        try
        {
            await _mediator.Send(command);
            return Ok(new { message = "Contratación cancelada exitosamente" });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Cannot cancel contratacion {Id}", id);
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene contrataciones pendientes (estado = Pendiente).
    /// </summary>
    /// <returns>Lista de contrataciones pendientes</returns>
    /// <response code="200">Lista de contrataciones pendientes</response>
    [HttpGet("pendientes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPendientes([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        _logger.LogInformation("Getting pendientes contrataciones");

        var query = new GetContratacionesQuery 
        { 
            SoloPendientes = true,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene contrataciones activas (estado = En Progreso).
    /// </summary>
    /// <returns>Lista de contrataciones activas</returns>
    /// <response code="200">Lista de contrataciones activas</response>
    [HttpGet("activas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivas([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        _logger.LogInformation("Getting activas contrataciones");

        var query = new GetContratacionesQuery 
        { 
            SoloActivas = true,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Obtiene contrataciones completadas sin calificar.
    /// </summary>
    /// <returns>Lista de contrataciones completadas sin calificar</returns>
    /// <response code="200">Lista de contrataciones sin calificar</response>
    [HttpGet("sin-calificar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSinCalificar([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        _logger.LogInformation("Getting contrataciones sin calificar");

        var query = new GetContratacionesQuery 
        { 
            SoloNoCalificadas = true,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
