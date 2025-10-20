using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiGenteEnLinea.Application.Features.Dashboard.Queries.GetDashboardEmpleador;
using System.Security.Claims;

namespace MiGenteEnLinea.API.Controllers;

/// <summary>
/// Controller para endpoints de Dashboard (métricas y estadísticas).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        IMediator mediator,
        ILogger<DashboardController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las métricas del dashboard para un Empleador.
    /// </summary>
    /// <param name="fechaReferencia">Fecha de referencia para calcular métricas (opcional, default: hoy)</param>
    /// <returns>Dashboard completo con métricas, gráficos e historial</returns>
    /// <response code="200">Dashboard obtenido exitosamente</response>
    /// <response code="401">No autenticado</response>
    /// <response code="500">Error interno del servidor</response>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /api/dashboard/empleador
    ///     GET /api/dashboard/empleador?fechaReferencia=2025-10-15
    ///     
    /// Sample response:
    /// 
    ///     {
    ///       "totalEmpleados": 25,
    ///       "empleadosActivos": 22,
    ///       "empleadosInactivos": 3,
    ///       "nominaMesActual": 450000.00,
    ///       "nominaAnoActual": 5400000.00,
    ///       "proximaNominaFecha": "2025-11-01T00:00:00",
    ///       "proximaNominaMonto": 450000.00,
    ///       "totalPagosHistoricos": 12500000.00,
    ///       "suscripcionPlan": "Plan Premium",
    ///       "suscripcionVencimiento": "2026-01-15T00:00:00",
    ///       "suscripcionActiva": true,
    ///       "diasRestantesSuscripcion": 87,
    ///       "recibosGeneradosEsteMes": 22,
    ///       "contratacionesTemporalesActivas": 0,
    ///       "contratacionesTemporalesCompletadas": 0,
    ///       "calificacionesPendientes": 0,
    ///       "calificacionesCompletadas": 15,
    ///       "ultimosPagos": [
    ///         {
    ///           "reciboId": 1234,
    ///           "fecha": "2025-10-15T00:00:00",
    ///           "monto": 18500.00,
    ///           "empleadoNombre": "Juan Pérez",
    ///           "concepto": "Salario Quincenal - Octubre 2025",
    ///           "estado": "Completado"
    ///         }
    ///       ],
    ///       "evolucionNomina": [
    ///         {
    ///           "mes": "May 2025",
    ///           "ano": 2025,
    ///           "numeroMes": 5,
    ///           "totalNomina": 420000.00,
    ///           "cantidadRecibos": 21
    ///         }
    ///       ],
    ///       "topDeducciones": [
    ///         {
    ///           "descripcion": "AFP",
    ///           "total": 85000.00,
    ///           "frecuencia": 150,
    ///           "porcentaje": 35.5
    ///         }
    ///       ],
    ///       "distribucionEmpleados": [
    ///         {
    ///           "posicion": "Operario",
    ///           "cantidad": 15,
    ///           "porcentaje": 68.2,
    ///           "salarioPromedio": 18000.00
    ///         }
    ///       ]
    ///     }
    /// 
    /// **Notas:**
    /// - Requiere autenticación (token JWT)
    /// - UserId se obtiene automáticamente del token
    /// - ContratacionesTemporales* = 0 (funcionalidad pendiente de migración)
    /// - Se recomienda cachear el resultado (TTL: 5-15 min)
    /// - Evolución nómina muestra últimos 6 meses
    /// - Top deducciones muestra top 5
    /// - Distribución empleados solo incluye activos
    /// </remarks>
    [HttpGet("empleador")]
    [ProducesResponseType(typeof(DashboardEmpleadorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DashboardEmpleadorDto>> GetEmpleadorDashboard(
        [FromQuery] DateTime? fechaReferencia = null)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("GetEmpleadorDashboard called without valid UserId");
            return Unauthorized(new { message = "Usuario no autenticado" });
        }

        _logger.LogInformation("Fetching dashboard for Empleador UserId: {UserId}", userId);

        try
        {
            var query = new GetDashboardEmpleadorQuery
            {
                UserId = userId,
                FechaReferencia = fechaReferencia
            };

            var dashboard = await _mediator.Send(query);

            _logger.LogInformation(
                "Dashboard fetched successfully - Empleados: {Empleados}, Nómina Mes: {NominaMes:C}",
                dashboard.TotalEmpleados,
                dashboard.NominaMesActual);

            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching dashboard for UserId: {UserId}", userId);
            return StatusCode(500, new { message = "Error al obtener el dashboard" });
        }
    }

    /// <summary>
    /// Health check endpoint para el DashboardController.
    /// </summary>
    /// <returns>Estado del servicio</returns>
    [HttpGet("health")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Health()
    {
        return Ok(new
        {
            service = "Dashboard API",
            version = "1.0.0",
            timestamp = DateTime.UtcNow,
            features = new[]
            {
                "Empleador Dashboard (Metrics + Charts)",
                "Real-time Statistics",
                "6-month Payroll Evolution",
                "Top 5 Deductions Analysis",
                "Employee Distribution by Position",
                "Payment History (Last 10)",
                "Subscription Status Tracking"
            },
            endpoints = new[]
            {
                "GET /api/dashboard/empleador",
                "GET /api/dashboard/health"
            },
            status = "Healthy"
        });
    }
}
