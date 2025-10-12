namespace MiGenteEnLinea.Domain.Common;

/// <summary>
/// Entidad base que proporciona campos de auditoría automática.
/// Todas las entidades que hereden de esta clase tendrán tracking de creación y modificación.
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// Momento en que se creó la entidad (UTC)
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Usuario que creó la entidad
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Última fecha de modificación (UTC)
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Usuario que realizó la última modificación
    /// </summary>
    public string? UpdatedBy { get; set; }
}
