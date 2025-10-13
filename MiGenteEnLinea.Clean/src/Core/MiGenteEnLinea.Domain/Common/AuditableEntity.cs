namespace MiGenteEnLinea.Domain.Common;

/// <summary>
/// Entidad base que proporciona campos de auditoría automática.
/// Todas las entidades que hereden de esta clase tendrán tracking de creación y modificación.
/// 
/// NOTA: CreatedAt es nullable para permitir migración gradual desde tablas legacy
/// que no tienen estas columnas. Los interceptores establecerán el valor automáticamente.
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// Momento en que se creó la entidad (UTC)
    /// Nullable para compatibilidad con migración desde legacy (se poblará automáticamente)
    /// </summary>
    public DateTime? CreatedAt { get; set; }

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
