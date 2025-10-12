namespace MiGenteEnLinea.Domain.ReadModels;

/// <summary>
/// Vista de solo lectura que combina información de calificaciones con datos del perfil evaluado
/// </summary>
/// <remarks>
/// Esta vista es generada por la base de datos y proporciona una consulta optimizada
/// para mostrar calificaciones con información completa del perfil (empleador o contratista).
/// No tiene métodos de dominio ni eventos, solo propiedades inmutables.
/// </remarks>
public sealed class VistaCalificacion
{
    /// <summary>
    /// ID de la calificación
    /// </summary>
    public int CalificacionId { get; init; }

    /// <summary>
    /// Fecha en que se realizó la calificación
    /// </summary>
    public DateTime? Fecha { get; init; }

    /// <summary>
    /// ID del usuario que fue calificado
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// Tipo de usuario ("Empleador" o "Contratista")
    /// </summary>
    public string? Tipo { get; init; }

    /// <summary>
    /// Identificación (cédula/RNC) del calificado
    /// </summary>
    public string? Identificacion { get; init; }

    /// <summary>
    /// Nombre del calificado
    /// </summary>
    public string? Nombre { get; init; }

    /// <summary>
    /// Puntuación de puntualidad (1-5)
    /// </summary>
    public int? Puntualidad { get; init; }

    /// <summary>
    /// Puntuación de cumplimiento (1-5)
    /// </summary>
    public int? Cumplimiento { get; init; }

    /// <summary>
    /// Puntuación de conocimientos (1-5)
    /// </summary>
    public int? Conocimientos { get; init; }

    /// <summary>
    /// Puntuación de recomendación (1-5)
    /// </summary>
    public int? Recomendacion { get; init; }

    /// <summary>
    /// ID del perfil asociado
    /// </summary>
    public int? PerfilId { get; init; }

    /// <summary>
    /// Fecha de creación del perfil
    /// </summary>
    public DateTime? FechaCreacion { get; init; }

    /// <summary>
    /// Campos adicionales de la vista (JOIN con Perfiles)
    /// </summary>
    public string? Expr1 { get; init; }
    public int? Expr2 { get; init; }
    public string? Expr3 { get; init; }

    /// <summary>
    /// Apellido del calificado
    /// </summary>
    public string? Apellido { get; init; }

    /// <summary>
    /// Email del calificado
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Teléfono principal del calificado
    /// </summary>
    public string? Telefono1 { get; init; }

    /// <summary>
    /// Teléfono secundario del calificado
    /// </summary>
    public string? Telefono2 { get; init; }

    /// <summary>
    /// Usuario/alias del calificado
    /// </summary>
    public string? Usuario { get; init; }
}
