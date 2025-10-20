using MediatR;

namespace MiGenteEnLinea.Application.Features.Dashboard.Queries.GetDashboardEmpleador;

/// <summary>
/// Query para obtener métricas completas del dashboard de un Empleador.
/// </summary>
/// <remarks>
/// Esta query recopila todas las métricas relevantes para el dashboard del empleador:
/// - Total de empleados (activos/inactivos)
/// - Nómina mensual y anual
/// - Estado de suscripción
/// - Contrataciones temporales
/// - Historial de pagos recientes
/// 
/// Los datos son agregados desde múltiples tablas (Empleados, RecibosHeader, Suscripciones, etc.)
/// y se recomienda cachear el resultado para optimizar performance.
/// </remarks>
public record GetDashboardEmpleadorQuery : IRequest<DashboardEmpleadorDto>
{
    /// <summary>
    /// UserId del empleador autenticado.
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Fecha de referencia para calcular métricas (default: DateTime.UtcNow).
    /// Útil para generar snapshots históricos del dashboard.
    /// </summary>
    public DateTime? FechaReferencia { get; init; }
}

/// <summary>
/// DTO con todas las métricas del dashboard empleador.
/// </summary>
/// <remarks>
/// Basado en el Dashboard.aspx del sistema legacy que muestra:
/// 1. Card "EMPLEADOS": Total empleados en nómina
/// 2. Card "PAGOS": Historial de pagos realizados
/// 3. Card "CALIFICACIONES": Calificaciones completadas
/// 4. Sección "Enlaces Rápidos": Acceso a módulos principales
/// 5. Tabla "Historial de Pagos": Últimos pagos
/// </remarks>
public class DashboardEmpleadorDto
{
    // ========================================
    // 📊 SECCIÓN: EMPLEADOS
    // ========================================
    
    /// <summary>
    /// Total de empleados registrados (activos + inactivos).
    /// </summary>
    public int TotalEmpleados { get; set; }
    
    /// <summary>
    /// Empleados activos actualmente (Activo = true).
    /// </summary>
    public int EmpleadosActivos { get; set; }
    
    /// <summary>
    /// Empleados inactivos o dados de baja (Activo = false).
    /// </summary>
    public int EmpleadosInactivos { get; set; }

    // ========================================
    // 💰 SECCIÓN: NÓMINA Y PAGOS
    // ========================================
    
    /// <summary>
    /// Total de nómina procesada en el mes actual (suma de NetoPagar).
    /// </summary>
    public decimal NominaMesActual { get; set; }
    
    /// <summary>
    /// Total de nómina procesada en el año actual.
    /// </summary>
    public decimal NominaAnoActual { get; set; }
    
    /// <summary>
    /// Próxima fecha estimada de procesamiento de nómina.
    /// Calculada basándose en el período de pago más común (Semanal/Quincenal/Mensual).
    /// </summary>
    public DateTime? ProximaNominaFecha { get; set; }
    
    /// <summary>
    /// Monto estimado de la próxima nómina basándose en salarios actuales.
    /// </summary>
    public decimal ProximaNominaMonto { get; set; }

    /// <summary>
    /// Total acumulado de pagos históricos realizados (todos los tiempos).
    /// Mostrado en Card "PAGOS" del dashboard legacy.
    /// </summary>
    public decimal TotalPagosHistoricos { get; set; }

    // ========================================
    // 📅 SECCIÓN: SUSCRIPCIÓN
    // ========================================
    
    /// <summary>
    /// Nombre del plan de suscripción actual (ej: "Plan Premium", "Plan Básico").
    /// </summary>
    public string SuscripcionPlan { get; set; } = string.Empty;
    
    /// <summary>
    /// Fecha de vencimiento de la suscripción.
    /// </summary>
    public DateTime? SuscripcionVencimiento { get; set; }
    
    /// <summary>
    /// Indica si la suscripción está activa (no vencida).
    /// </summary>
    public bool SuscripcionActiva { get; set; }

    /// <summary>
    /// Días restantes antes de que venza la suscripción.
    /// Valores negativos indican suscripción vencida.
    /// </summary>
    public int DiasRestantesSuscripcion { get; set; }

    // ========================================
    // 📊 SECCIÓN: ACTIVIDAD RECIENTE
    // ========================================
    
    /// <summary>
    /// Total de recibos de pago generados en el mes actual.
    /// </summary>
    public int RecibosGeneradosEsteMes { get; set; }
    
    /// <summary>
    /// Total de contrataciones temporales activas.
    /// </summary>
    public int ContratacionesTemporalesActivas { get; set; }

    /// <summary>
    /// Total de contrataciones temporales completadas.
    /// </summary>
    public int ContratacionesTemporalesCompletadas { get; set; }

    /// <summary>
    /// Total de calificaciones pendientes de realizar.
    /// Mostrado en Card "CALIFICACIONES" del dashboard legacy.
    /// </summary>
    public int CalificacionesPendientes { get; set; }

    /// <summary>
    /// Total de calificaciones completadas (histórico).
    /// </summary>
    public int CalificacionesCompletadas { get; set; }

    // ========================================
    // 📋 SECCIÓN: HISTORIAL DE PAGOS RECIENTES
    // ========================================

    /// <summary>
    /// Lista de los últimos 10 pagos realizados.
    /// Mostrado en sección "Historial de Pagos" del dashboard legacy.
    /// </summary>
    public List<PagoRecienteDto> UltimosPagos { get; set; } = new();
}

/// <summary>
/// DTO para representar un pago reciente en el historial del dashboard.
/// </summary>
public class PagoRecienteDto
{
    /// <summary>
    /// ID del recibo de pago.
    /// </summary>
    public int ReciboId { get; set; }

    /// <summary>
    /// Fecha en que se procesó el pago.
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Monto neto del pago.
    /// </summary>
    public decimal Monto { get; set; }

    /// <summary>
    /// Nombre del empleado que recibió el pago.
    /// </summary>
    public string EmpleadoNombre { get; set; } = string.Empty;

    /// <summary>
    /// Concepto del pago (ej: "Salario Quincenal - Diciembre 2024").
    /// </summary>
    public string Concepto { get; set; } = string.Empty;

    /// <summary>
    /// Estado del pago (ej: "Completado", "Pendiente", "Anulado").
    /// </summary>
    public string Estado { get; set; } = "Completado";
}
