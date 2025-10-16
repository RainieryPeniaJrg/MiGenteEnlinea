using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Features.Suscripciones.Commands.ProcesarVenta;
using MiGenteEnLinea.Application.Features.Suscripciones.Commands.ProcesarVentaSinPago;
using MiGenteEnLinea.Application.Features.Suscripciones.DTOs;
using MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetVentasByUserId;

namespace MiGenteEnLinea.API.Controllers;

/// <summary>
/// Controlador para procesamiento de pagos y consulta de historial.
/// </summary>
/// <remarks>
/// Legacy: PaymentService.cs, SuscripcionesService.cs
/// Endpoints para procesar pagos con Cardnet, procesar suscripciones gratuitas
/// y consultar historial de transacciones.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PagosController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<PagosController> _logger;

    public PagosController(
        IMediator mediator,
        IMapper mapper,
        ILogger<PagosController> logger)
    {
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Procesa un pago con tarjeta de crédito mediante Cardnet.
    /// </summary>
    /// <param name="command">Datos del pago (tarjeta, plan, usuario).</param>
    /// <returns>Detalles de la transacción procesada.</returns>
    /// <response code="200">Pago procesado exitosamente (aprobado).</response>
    /// <response code="400">Datos inválidos o pago rechazado.</response>
    /// <response code="401">No autorizado.</response>
    /// <response code="500">Error al comunicarse con Cardnet.</response>
    /// <remarks>
    /// Legacy: PaymentService.cs -> procesarPago()
    /// 
    /// Este endpoint realiza las siguientes operaciones:
    /// 1. Valida datos de la tarjeta (Luhn algorithm, CVV, expiration)
    /// 2. Genera clave de idempotencia (evita duplicados)
    /// 3. Procesa pago con Cardnet API
    /// 4. Crea registro de venta (estado: Aprobado/Rechazado)
    /// 5. Crea o renueva suscripción (solo si pago aprobado)
    /// 
    /// Códigos de respuesta Cardnet:
    /// - "00" = Aprobado
    /// - Otros = Rechazado (ver comentario en respuesta)
    /// 
    /// IMPORTANTE: Rate limiting aplicado (max 10 pagos por minuto por IP).
    /// </remarks>
    [HttpPost("procesar")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> ProcesarPago([FromBody] ProcesarVentaCommand command)
    {
        _logger.LogInformation(
            "POST /api/pagos/procesar - Processing payment for user {UserId}, plan {PlanId}",
            command.UserId, command.PlanId);

        try
        {
            var ventaId = await _mediator.Send(command);

            _logger.LogInformation(
                "Pago procesado exitosamente. VentaId: {VentaId}",
                ventaId);

            return Ok(new { ventaId, message = "Pago procesado exitosamente" });
        }
        catch (PaymentRejectedException ex)
        {
            _logger.LogWarning(
                ex,
                "Pago rechazado por Cardnet. UserId: {UserId}, ResponseCode: {ResponseCode}",
                command.UserId, ex.ResponseCode);

            return BadRequest(new
            {
                message = ex.Message,
                responseCode = ex.ResponseCode,
                tipo = "pago_rechazado"
            });
        }
        catch (PaymentException ex)
        {
            _logger.LogError(
                ex,
                "Error al procesar pago con Cardnet. UserId: {UserId}",
                command.UserId);

            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new
                {
                    message = "Error al comunicarse con el gateway de pago. Por favor intente nuevamente.",
                    tipo = "error_gateway"
                });
        }
    }

    /// <summary>
    /// Procesa una suscripción sin pago (plan gratuito o promocional).
    /// </summary>
    /// <param name="command">Datos de la suscripción gratuita.</param>
    /// <returns>Detalles de la transacción procesada.</returns>
    /// <response code="200">Suscripción gratuita procesada exitosamente.</response>
    /// <response code="400">Datos inválidos o plan no es gratuito.</response>
    /// <response code="401">No autorizado.</response>
    /// <remarks>
    /// Legacy: SuscripcionesService.cs -> guardarSuscripcion() (con precio = 0)
    /// 
    /// Este endpoint:
    /// 1. Valida que el precio del plan sea 0
    /// 2. Crea registro de venta con método "Sin Pago"
    /// 3. Crea o renueva suscripción automáticamente
    /// 
    /// Usado para:
    /// - Planes gratuitos de prueba
    /// - Promociones especiales
    /// - Activaciones manuales por soporte
    /// </remarks>
    [HttpPost("sin-pago")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> ProcesarSinPago([FromBody] ProcesarVentaSinPagoCommand command)
    {
        _logger.LogInformation(
            "POST /api/pagos/sin-pago - Processing free subscription for user {UserId}, plan {PlanId}",
            command.UserId, command.PlanId);

        var ventaId = await _mediator.Send(command);

        _logger.LogInformation(
            "Suscripción gratuita procesada exitosamente. VentaId: {VentaId}, UserId: {UserId}",
            ventaId, command.UserId);

        return Ok(new { ventaId, message = "Suscripción gratuita procesada exitosamente" });
    }

    /// <summary>
    /// Obtiene el historial de pagos/transacciones de un usuario (paginado).
    /// </summary>
    /// <param name="userId">ID del usuario.</param>
    /// <param name="pageNumber">Número de página (1-based). Default: 1.</param>
    /// <param name="pageSize">Tamaño de página. Default: 10. Máximo: 100.</param>
    /// <returns>Lista paginada de transacciones del usuario.</returns>
    /// <response code="200">Historial obtenido exitosamente.</response>
    /// <response code="401">No autorizado.</response>
    /// <remarks>
    /// NUEVA FUNCIONALIDAD (no existe en Legacy).
    /// 
    /// Retorna todas las transacciones (aprobadas, rechazadas, con error)
    /// ordenadas por fecha descendente (más reciente primero).
    /// 
    /// Para filtrar solo pagos exitosos, use:
    /// GET /api/suscripciones/ventas/{userId}?soloAprobadas=true
    /// </remarks>
    [HttpGet("historial/{userId}")]
    [ProducesResponseType(typeof(List<VentaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<VentaDto>>> GetHistorialPagos(
        string userId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        _logger.LogInformation(
            "GET /api/pagos/historial/{UserId}?pageNumber={PageNumber}&pageSize={PageSize}",
            userId, pageNumber, pageSize);

        var query = new GetVentasByUserIdQuery
        {
            UserId = userId,
            PageNumber = pageNumber,
            PageSize = pageSize,
            SoloAprobadas = false // Retorna todas (aprobadas, rechazadas, error)
        };

        var ventas = await _mediator.Send(query);
        var dtos = _mapper.Map<List<VentaDto>>(ventas);

        _logger.LogInformation(
            "Retornando {Count} transacciones para usuario {UserId} (página {PageNumber})",
            dtos.Count, userId, pageNumber);

        return Ok(dtos);
    }
}
