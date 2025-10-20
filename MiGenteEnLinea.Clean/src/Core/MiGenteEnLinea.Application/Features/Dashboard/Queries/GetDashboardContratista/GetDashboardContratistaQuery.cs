using MediatR;

namespace MiGenteEnLinea.Application.Features.Dashboard.Queries.GetDashboardContratista;

/// <summary>
/// Query para obtener el dashboard completo del Contratista con métricas, gráficos y actividad reciente.
/// </summary>
public record GetDashboardContratistaQuery : IRequest<DashboardContratistaDto>
{
    /// <summary>
    /// ID del usuario (UserId de Credencial)
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Fecha de referencia para cálculos (opcional, default: hoy)
    /// </summary>
    public DateTime? FechaReferencia { get; init; }
}

/// <summary>
/// DTO principal del dashboard del contratista con todas las métricas y gráficos.
/// </summary>
public class DashboardContratistaDto
{
    // ========================================
    // SECCIÓN 1: PERFIL Y SERVICIOS
    // ========================================

    /// <summary>
    /// Nombre completo del contratista
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Título o especialidad principal
    /// </summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>
    /// URL de la imagen de perfil
    /// </summary>
    public string? ImagenUrl { get; set; }

    /// <summary>
    /// Cantidad total de servicios ofrecidos
    /// </summary>
    public int TotalServicios { get; set; }

    /// <summary>
    /// Lista de servicios activos
    /// </summary>
    public List<string> ServiciosActivos { get; set; } = new();

    // ========================================
    // SECCIÓN 2: CALIFICACIONES
    // ========================================

    /// <summary>
    /// Promedio de calificaciones (0-5 estrellas)
    /// </summary>
    public decimal PromedioCalificacion { get; set; }

    /// <summary>
    /// Total de calificaciones recibidas
    /// </summary>
    public int TotalCalificaciones { get; set; }

    /// <summary>
    /// Cantidad de calificaciones 5 estrellas
    /// </summary>
    public int Calificaciones5Estrellas { get; set; }

    /// <summary>
    /// Cantidad de calificaciones 4 estrellas
    /// </summary>
    public int Calificaciones4Estrellas { get; set; }

    /// <summary>
    /// Cantidad de calificaciones 3 estrellas
    /// </summary>
    public int Calificaciones3Estrellas { get; set; }

    /// <summary>
    /// Cantidad de calificaciones 2 estrellas o menos
    /// </summary>
    public int CalificacionesBajas { get; set; }

    /// <summary>
    /// Porcentaje de recomendación (calificaciones >= 4)
    /// </summary>
    public decimal PorcentajeRecomendacion { get; set; }

    // ========================================
    // SECCIÓN 3: CONTRATACIONES
    // ========================================

    /// <summary>
    /// Contrataciones pendientes de aceptación/rechazo
    /// </summary>
    public int ContratacionesPendientes { get; set; }

    /// <summary>
    /// Contrataciones aceptadas pero no iniciadas
    /// </summary>
    public int ContratacionesAceptadas { get; set; }

    /// <summary>
    /// Contrataciones actualmente en progreso
    /// </summary>
    public int ContratacionesEnProgreso { get; set; }

    /// <summary>
    /// Total de contrataciones completadas históricamente
    /// </summary>
    public int ContratacionesCompletadas { get; set; }

    /// <summary>
    /// Contrataciones completadas este mes
    /// </summary>
    public int ContratacionesCompletadasEsteMes { get; set; }

    /// <summary>
    /// Contrataciones completadas este año
    /// </summary>
    public int ContratacionesCompletadasEsteAno { get; set; }

    /// <summary>
    /// Total de contrataciones canceladas/rechazadas
    /// </summary>
    public int ContratacionesCanceladas { get; set; }

    /// <summary>
    /// Tasa de éxito (completadas / total)
    /// </summary>
    public decimal TasaExito { get; set; }

    // ========================================
    // SECCIÓN 4: INGRESOS
    // ========================================

    /// <summary>
    /// Ingresos totales del mes actual
    /// </summary>
    public decimal IngresosMesActual { get; set; }

    /// <summary>
    /// Ingresos totales del año actual
    /// </summary>
    public decimal IngresosAnoActual { get; set; }

    /// <summary>
    /// Total histórico de ingresos
    /// </summary>
    public decimal IngresosHistoricos { get; set; }

    /// <summary>
    /// Promedio de ingreso por contratación
    /// </summary>
    public decimal IngresoPromedioContratacion { get; set; }

    /// <summary>
    /// Ingresos pendientes por cobrar (contrataciones en progreso)
    /// </summary>
    public decimal IngresosPendientes { get; set; }

    // ========================================
    // SECCIÓN 5: SUSCRIPCIÓN
    // ========================================

    /// <summary>
    /// Nombre del plan de suscripción actual
    /// </summary>
    public string SuscripcionPlan { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de vencimiento de la suscripción
    /// </summary>
    public DateTime? SuscripcionVencimiento { get; set; }

    /// <summary>
    /// Indica si la suscripción está activa
    /// </summary>
    public bool SuscripcionActiva { get; set; }

    /// <summary>
    /// Días restantes de suscripción (negativo si está vencida)
    /// </summary>
    public int DiasRestantesSuscripcion { get; set; }

    // ========================================
    // SECCIÓN 6: ACTIVIDAD RECIENTE
    // ========================================

    /// <summary>
    /// Última fecha de login del contratista
    /// </summary>
    public DateTime? UltimoLogin { get; set; }

    /// <summary>
    /// Días desde la última contratación completada
    /// </summary>
    public int? DiasSinCompletarTrabajo { get; set; }

    /// <summary>
    /// Contrataciones próximas a vencer (fecha final < 7 días)
    /// </summary>
    public int ContratacionesProximasVencer { get; set; }

    /// <summary>
    /// Contrataciones atrasadas (fecha final pasada)
    /// </summary>
    public int ContratacionesAtrasadas { get; set; }

    // ========================================
    // SECCIÓN 7: HISTORIAL DE CONTRATACIONES
    // ========================================

    /// <summary>
    /// Últimas 10 contrataciones (todas las estados)
    /// </summary>
    public List<ContratacionRecienteDto> UltimasContrataciones { get; set; } = new();

    // ========================================
    // SECCIÓN 8: GRÁFICOS Y ESTADÍSTICAS
    // ========================================

    /// <summary>
    /// Evolución de ingresos por mes (últimos 6 meses)
    /// </summary>
    public List<IngresosEvolucionDto> EvolucionIngresos { get; set; } = new();

    /// <summary>
    /// Distribución de calificaciones
    /// </summary>
    public List<CalificacionDistribucionDto> DistribucionCalificaciones { get; set; } = new();

    /// <summary>
    /// Tipos de trabajos más frecuentes
    /// </summary>
    public List<ServicioFrecuenciaDto> ServiciosMasFrecuentes { get; set; } = new();

    /// <summary>
    /// Estadísticas de tiempo de respuesta
    /// </summary>
    public TiempoRespuestaDto TiempoRespuesta { get; set; } = new();
}

/// <summary>
/// DTO para mostrar contrataciones recientes en el historial
/// </summary>
public class ContratacionRecienteDto
{
    public int DetalleId { get; set; }
    public string DescripcionCorta { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFinal { get; set; }
    public decimal MontoAcordado { get; set; }
    public string Estado { get; set; } = string.Empty; // Pendiente, Aceptada, En Progreso, Completada, etc.
    public int PorcentajeAvance { get; set; }
    public bool Calificado { get; set; }
    public decimal? CalificacionRecibida { get; set; }
}

/// <summary>
/// DTO para gráfico de evolución de ingresos
/// </summary>
public class IngresosEvolucionDto
{
    public string Mes { get; set; } = string.Empty; // "Ene 2025"
    public int Ano { get; set; }
    public int NumeroMes { get; set; } // 1-12 para ordenamiento
    public decimal TotalIngresos { get; set; }
    public int CantidadContrataciones { get; set; }
}

/// <summary>
/// DTO para distribución de calificaciones (para gráfico de barras/pie)
/// </summary>
public class CalificacionDistribucionDto
{
    public int Estrellas { get; set; } // 1-5
    public int Cantidad { get; set; }
    public decimal Porcentaje { get; set; }
}

/// <summary>
/// DTO para servicios más frecuentes
/// </summary>
public class ServicioFrecuenciaDto
{
    public string Servicio { get; set; } = string.Empty;
    public int CantidadContrataciones { get; set; }
    public decimal IngresoTotal { get; set; }
    public decimal Porcentaje { get; set; }
}

/// <summary>
/// DTO para estadísticas de tiempo de respuesta
/// </summary>
public class TiempoRespuestaDto
{
    /// <summary>
    /// Promedio de días entre recibir propuesta y aceptarla/rechazarla
    /// </summary>
    public decimal PromedioRespuestaDias { get; set; }

    /// <summary>
    /// Tasa de aceptación (aceptadas / total propuestas)
    /// </summary>
    public decimal TasaAceptacion { get; set; }

    /// <summary>
    /// Cantidad de propuestas respondidas en menos de 24 horas
    /// </summary>
    public int RespuestasRapidas { get; set; }

    /// <summary>
    /// Cantidad de propuestas aún sin responder
    /// </summary>
    public int PropuestasSinResponder { get; set; }
}
