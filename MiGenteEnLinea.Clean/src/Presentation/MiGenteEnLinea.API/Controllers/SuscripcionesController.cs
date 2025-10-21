using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Features.Suscripciones.Commands.CancelarSuscripcion;
using MiGenteEnLinea.Application.Features.Suscripciones.Commands.CreateSuscripcion;
using MiGenteEnLinea.Application.Features.Suscripciones.Commands.RenovarSuscripcion;
using MiGenteEnLinea.Application.Features.Suscripciones.Commands.UpdateSuscripcion;
using MiGenteEnLinea.Application.Features.Suscripciones.DTOs;
using MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetPlanesContratistas;
using MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetPlanesEmpleadores;
using MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetSuscripcionActiva;
using MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetVentasByUserId;

namespace MiGenteEnLinea.API.Controllers;

/// <summary>
/// Controlador para gestión de suscripciones y planes.
/// </summary>
/// <remarks>
/// Legacy: SuscripcionesService.cs, Comunity1.Master.cs, ContratistaM.Master.cs
/// Endpoints para crear, actualizar, renovar, cancelar suscripciones,
/// consultar planes disponibles y historial de ventas.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuscripcionesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<SuscripcionesController> _logger;

    public SuscripcionesController(
        IMediator mediator,
        IMapper mapper,
        ILogger<SuscripcionesController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la suscripción activa de un usuario.
    /// </summary>
    /// <param name="userId">ID del usuario (Credencial.Id).</param>
    /// <returns>Suscripción activa o null si no tiene.</returns>
    /// <response code="200">Suscripción encontrada (puede estar expirada).</response>
    /// <response code="404">Usuario no tiene suscripción.</response>
    /// <response code="401">No autorizado.</response>
    [HttpGet("activa/{userId}")]
    [ProducesResponseType(typeof(SuscripcionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SuscripcionDto>> GetSuscripcionActiva(string userId)
    {
        _logger.LogInformation("GET /api/suscripciones/activa/{UserId} - Request received", userId);

        var query = new GetSuscripcionActivaQuery { UserId = userId };
        var suscripcion = await _mediator.Send(query);

        if (suscripcion == null)
        {
            _logger.LogInformation("No se encontró suscripción para usuario {UserId}", userId);
            return NotFound(new { message = "Usuario no tiene suscripción activa" });
        }

        var dto = _mapper.Map<SuscripcionDto>(suscripcion);
        return Ok(dto);
    }

    /// <summary>
    /// Crea una nueva suscripción para un usuario.
    /// </summary>
    /// <param name="command">Datos de la suscripción a crear.</param>
    /// <returns>ID de la suscripción creada.</returns>
    /// <response code="201">Suscripción creada exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="401">No autorizado.</response>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> CreateSuscripcion([FromBody] CreateSuscripcionCommand command)
    {
        _logger.LogInformation(
            "POST /api/suscripciones - Creating subscription for user {UserId}, plan {PlanId}",
            command.UserId, command.PlanId);

        var suscripcionId = await _mediator.Send(command);

        _logger.LogInformation(
            "Suscripción {SuscripcionId} creada exitosamente para usuario {UserId}",
            suscripcionId, command.UserId);

        return CreatedAtAction(
            nameof(GetSuscripcionActiva),
            new { userId = command.UserId },
            new { suscripcionId });
    }

    /// <summary>
    /// Actualiza una suscripción existente (cambio de plan y/o extensión de vencimiento).
    /// </summary>
    /// <param name="command">Datos de actualización.</param>
    /// <returns>Sin contenido si fue exitoso.</returns>
    /// <response code="204">Suscripción actualizada exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="404">Suscripción no encontrada.</response>
    /// <response code="401">No autorizado.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSuscripcion([FromBody] UpdateSuscripcionCommand command)
    {
        _logger.LogInformation(
            "PUT /api/suscripciones - Updating subscription for user {UserId}, new plan {PlanId}",
            command.UserId, command.NuevoPlanId);

        await _mediator.Send(command);

        _logger.LogInformation(
            "Suscripción actualizada exitosamente para usuario {UserId}",
            command.UserId);

        return NoContent();
    }

    /// <summary>
    /// Renueva una suscripción existente (extiende el vencimiento).
    /// </summary>
    /// <param name="command">Datos de renovación.</param>
    /// <returns>Sin contenido si fue exitoso.</returns>
    /// <response code="204">Suscripción renovada exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="404">Suscripción no encontrada.</response>
    /// <response code="401">No autorizado.</response>
    [HttpPost("renovar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RenovarSuscripcion([FromBody] RenovarSuscripcionCommand command)
    {
        _logger.LogInformation(
            "POST /api/suscripciones/renovar - Renewing subscription for user {UserId}, {Meses} months",
            command.UserId, command.MesesExtension);

        await _mediator.Send(command);

        _logger.LogInformation(
            "Suscripción renovada exitosamente para usuario {UserId} por {Meses} meses",
            command.UserId, command.MesesExtension);

        return NoContent();
    }

    /// <summary>
    /// Cancela la suscripción activa de un usuario.
    /// </summary>
    /// <param name="userId">ID del usuario.</param>
    /// <param name="command">Datos de cancelación (motivo).</param>
    /// <returns>Sin contenido si fue exitoso.</returns>
    /// <response code="204">Suscripción cancelada exitosamente.</response>
    /// <response code="400">Datos inválidos.</response>
    /// <response code="404">Suscripción no encontrada.</response>
    /// <response code="401">No autorizado.</response>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelarSuscripcion(
        string userId,
        [FromBody] CancelarSuscripcionCommand command)
    {
        _logger.LogInformation(
            "DELETE /api/suscripciones/{UserId} - Cancelling subscription",
            userId);

        // Ensure userId from route matches command
        if (command.UserId != userId)
        {
            return BadRequest(new { message = "UserId en la ruta no coincide con el comando" });
        }

        await _mediator.Send(command);

        _logger.LogInformation(
            "Suscripción cancelada exitosamente para usuario {UserId}",
            userId);

        return NoContent();
    }

    /// <summary>
    /// Obtiene todos los planes disponibles para empleadores.
    /// </summary>
    /// <param name="soloActivos">Si es true, solo retorna planes activos. Default: true.</param>
    /// <returns>Lista de planes de empleadores.</returns>
    /// <response code="200">Lista de planes obtenida exitosamente.</response>
    /// <remarks>
    /// Endpoint público (no requiere autenticación) para permitir consulta antes del registro.
    /// Legacy: SuscripcionesService.obtenerPlanes()
    /// </remarks>
    [HttpGet("planes/empleadores")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<PlanDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PlanDto>>> GetPlanesEmpleadores(
        [FromQuery] bool soloActivos = true)
    {
        _logger.LogInformation(
            "GET /api/suscripciones/planes/empleadores?soloActivos={SoloActivos}",
            soloActivos);

        var query = new GetPlanesEmpleadoresQuery { SoloActivos = soloActivos };
        var planes = await _mediator.Send(query);

        var dtos = _mapper.Map<List<PlanDto>>(planes);

        _logger.LogInformation(
            "Retornando {Count} planes de empleadores",
            dtos.Count);

        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene todos los planes disponibles para contratistas.
    /// </summary>
    /// <param name="soloActivos">Si es true, solo retorna planes activos. Default: true.</param>
    /// <returns>Lista de planes de contratistas.</returns>
    /// <response code="200">Lista de planes obtenida exitosamente.</response>
    /// <remarks>
    /// Endpoint público (no requiere autenticación) para permitir consulta antes del registro.
    /// Legacy: SuscripcionesService.obtenerPlanesContratistas()
    /// </remarks>
    [HttpGet("planes/contratistas")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(List<PlanDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PlanDto>>> GetPlanesContratistas(
        [FromQuery] bool soloActivos = true)
    {
        _logger.LogInformation(
            "GET /api/suscripciones/planes/contratistas?soloActivos={SoloActivos}",
            soloActivos);

        var query = new GetPlanesContratistasQuery { SoloActivos = soloActivos };
        var planes = await _mediator.Send(query);

        var dtos = _mapper.Map<List<PlanDto>>(planes);

        _logger.LogInformation(
            "Retornando {Count} planes de contratistas",
            dtos.Count);

        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene el historial de ventas/pagos de un usuario (paginado).
    /// </summary>
    /// <param name="userId">ID del usuario.</param>
    /// <param name="pageNumber">Número de página (1-based). Default: 1.</param>
    /// <param name="pageSize">Tamaño de página. Default: 10. Máximo: 100.</param>
    /// <param name="soloAprobadas">Si es true, solo retorna ventas aprobadas. Default: false.</param>
    /// <returns>Lista paginada de ventas del usuario.</returns>
    /// <response code="200">Historial obtenido exitosamente.</response>
    /// <response code="401">No autorizado.</response>
    /// <remarks>
    /// NUEVA FUNCIONALIDAD (no existe en Legacy).
    /// Retorna historial de compras/pagos ordenado por fecha descendente.
    /// </remarks>
    [HttpGet("ventas/{userId}")]
    [ProducesResponseType(typeof(List<VentaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<VentaDto>>> GetVentasByUserId(
        string userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool soloAprobadas = false)
    {
        _logger.LogInformation(
            "GET /api/suscripciones/ventas/{UserId}?pageNumber={PageNumber}&pageSize={PageSize}&soloAprobadas={SoloAprobadas}",
            userId, pageNumber, pageSize, soloAprobadas);

        var query = new GetVentasByUserIdQuery
        {
            UserId = userId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            SoloAprobadas = soloAprobadas
        };

        var ventas = await _mediator.Send(query);
        var dtos = _mapper.Map<List<VentaDto>>(ventas);

        _logger.LogInformation(
            "Retornando {Count} ventas para usuario {UserId} (página {PageNumber})",
            dtos.Count, userId, pageNumber);

        return Ok(dtos);
    }
}
