namespace MiGenteEnLinea.Application.Features.Empleadores.DTOs;

/// <summary>
/// DTO: Representa un Empleador completo
/// </summary>
/// <remarks>
/// Se usa para:
/// - Respuesta de queries (GetEmpleadorByUserId, GetEmpleadorById, SearchEmpleadores)
/// - NO incluye byte[] Foto (muy grande), se obtiene en endpoint separado
/// </remarks>
public sealed class EmpleadorDto
{
    /// <summary>
    /// ID del empleador (ofertanteID en legacy)
    /// </summary>
    public int EmpleadorId { get; init; }

    /// <summary>
    /// ID del usuario (FK a Credencial.UserId)
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Fecha de publicación/creación del perfil
    /// </summary>
    public DateTime? FechaPublicacion { get; init; }

    /// <summary>
    /// Habilidades de la empresa empleadora (max 200 caracteres)
    /// </summary>
    public string? Habilidades { get; init; }

    /// <summary>
    /// Experiencia o trayectoria de la empresa (max 200 caracteres)
    /// </summary>
    public string? Experiencia { get; init; }

    /// <summary>
    /// Descripción general del empleador/empresa (max 500 caracteres)
    /// </summary>
    public string? Descripcion { get; init; }

    /// <summary>
    /// Indica si el empleador tiene foto/logo cargado
    /// </summary>
    public bool TieneFoto { get; init; }

    /// <summary>
    /// Fecha de creación (auditoría)
    /// </summary>
    public DateTime? CreatedAt { get; init; }

    /// <summary>
    /// Fecha de última modificación (auditoría)
    /// </summary>
    public DateTime? UpdatedAt { get; init; }
}
