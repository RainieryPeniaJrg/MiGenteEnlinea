namespace MiGenteEnLinea.Domain.ReadModels;

/// <summary>
/// Vista de solo lectura optimizada para búsqueda y listado de contratistas
/// </summary>
/// <remarks>
/// Esta vista combina información de contratistas con sus calificaciones promedio,
/// servicios, fotos y disponibilidad geográfica. Es la vista principal para el
/// directorio público de contratistas disponibles.
/// </remarks>
public sealed class VistaContratista
{
    /// <summary>
    /// ID del contratista
    /// </summary>
    public int ContratistaId { get; init; }

    /// <summary>
    /// Fecha de ingreso al sistema
    /// </summary>
    public DateTime? FechaIngreso { get; init; }

    /// <summary>
    /// ID del usuario credencial
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// Título profesional o especialidad principal
    /// </summary>
    public string? Titulo { get; init; }

    /// <summary>
    /// Tipo de contratista (1=Persona física, 2=Empresa)
    /// </summary>
    public int? Tipo { get; init; }

    /// <summary>
    /// Identificación (cédula o RNC)
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
    /// Sector o industria en la que trabaja
    /// </summary>
    public string? Sector { get; init; }

    /// <summary>
    /// Años de experiencia
    /// </summary>
    public int? Experiencia { get; init; }

    /// <summary>
    /// Presentación o biografía del contratista
    /// </summary>
    public string? Presentacion { get; init; }

    /// <summary>
    /// Teléfono principal
    /// </summary>
    public string? Telefono1 { get; init; }

    /// <summary>
    /// Indica si el teléfono 1 es WhatsApp
    /// </summary>
    public bool? Whatsapp1 { get; init; }

    /// <summary>
    /// Indica si el teléfono 2 es WhatsApp
    /// </summary>
    public bool? Whatsapp2 { get; init; }

    /// <summary>
    /// Email de contacto
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Indica si el perfil está activo
    /// </summary>
    public bool? Activo { get; init; }

    /// <summary>
    /// Provincia donde opera
    /// </summary>
    public string? Provincia { get; init; }

    /// <summary>
    /// Indica si trabaja a nivel nacional
    /// </summary>
    public bool? NivelNacional { get; init; }

    /// <summary>
    /// Calificación promedio (calculada)
    /// </summary>
    public decimal Calificacion { get; init; }

    /// <summary>
    /// Total de calificaciones recibidas
    /// </summary>
    public int TotalRegistros { get; init; }

    /// <summary>
    /// Teléfono secundario
    /// </summary>
    public string? Telefono2 { get; init; }

    /// <summary>
    /// URL de la imagen de perfil
    /// </summary>
    public string? ImagenUrl { get; init; }
}
