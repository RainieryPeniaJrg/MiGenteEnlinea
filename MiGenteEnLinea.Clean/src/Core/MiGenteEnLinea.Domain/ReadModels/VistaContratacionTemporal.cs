namespace MiGenteEnLinea.Domain.ReadModels;

/// <summary>
/// Vista de solo lectura que combina información de contrataciones temporales con detalles completos
/// </summary>
/// <remarks>
/// Esta vista es generada por la base de datos y proporciona una consulta optimizada
/// para mostrar contrataciones con información del contratista, detalles del proyecto y calificaciones.
/// Usada típicamente para reportes y listados de contrataciones activas.
/// </remarks>
public sealed class VistaContratacionTemporal
{
    /// <summary>
    /// ID de la contratación temporal
    /// </summary>
    public int ContratacionId { get; init; }

    /// <summary>
    /// ID del usuario empleador
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// Fecha de registro de la contratación
    /// </summary>
    public DateTime? FechaRegistro { get; init; }

    /// <summary>
    /// Tipo de contratación
    /// </summary>
    public int? Tipo { get; init; }

    /// <summary>
    /// Nombre comercial del contratista (si es empresa)
    /// </summary>
    public string? NombreComercial { get; init; }

    /// <summary>
    /// RNC del contratista (si es empresa)
    /// </summary>
    public string? Rnc { get; init; }

    /// <summary>
    /// Identificación (cédula) del contratista
    /// </summary>
    public string? Identificacion { get; init; }

    /// <summary>
    /// Nombre del contratista
    /// </summary>
    public string? Nombre { get; init; }

    /// <summary>
    /// Apellido del contratista
    /// </summary>
    public string? Apellido { get; init; }

    /// <summary>
    /// Alias/apodo del contratista
    /// </summary>
    public string? Alias { get; init; }

    /// <summary>
    /// Dirección del contratista
    /// </summary>
    public string? Direccion { get; init; }

    /// <summary>
    /// Provincia del contratista
    /// </summary>
    public string? Provincia { get; init; }

    /// <summary>
    /// Municipio del contratista
    /// </summary>
    public string? Municipio { get; init; }

    /// <summary>
    /// Teléfono principal
    /// </summary>
    public string? Telefono1 { get; init; }

    /// <summary>
    /// Teléfono secundario
    /// </summary>
    public string? Telefono2 { get; init; }

    /// <summary>
    /// ID del detalle de contratación
    /// </summary>
    public int DetalleId { get; init; }

    /// <summary>
    /// Campo expresión 1 (JOIN auxiliar)
    /// </summary>
    public int? Expr1 { get; init; }

    /// <summary>
    /// Descripción corta del proyecto/servicio
    /// </summary>
    public string? DescripcionCorta { get; init; }

    /// <summary>
    /// Descripción ampliada del proyecto/servicio
    /// </summary>
    public string? DescripcionAmpliada { get; init; }

    /// <summary>
    /// Fecha de inicio del proyecto
    /// </summary>
    public DateOnly? FechaInicio { get; init; }

    /// <summary>
    /// Fecha final del proyecto
    /// </summary>
    public DateOnly? FechaFinal { get; init; }

    /// <summary>
    /// Monto acordado para el proyecto
    /// </summary>
    public decimal? MontoAcordado { get; init; }

    /// <summary>
    /// Esquema de pagos acordado
    /// </summary>
    public string? EsquemaPagos { get; init; }

    /// <summary>
    /// Estatus de la contratación
    /// </summary>
    public int? Estatus { get; init; }

    /// <summary>
    /// Composición del nombre completo
    /// </summary>
    public string? ComposicionNombre { get; init; }

    /// <summary>
    /// Composición del ID
    /// </summary>
    public string? ComposicionId { get; init; }

    /// <summary>
    /// Calificación de conocimientos
    /// </summary>
    public int? Conocimientos { get; init; }

    /// <summary>
    /// Calificación de puntualidad
    /// </summary>
    public int? Puntualidad { get; init; }

    /// <summary>
    /// Calificación de recomendación
    /// </summary>
    public int? Recomendacion { get; init; }

    /// <summary>
    /// Calificación de cumplimiento
    /// </summary>
    public int? Cumplimiento { get; init; }
}
