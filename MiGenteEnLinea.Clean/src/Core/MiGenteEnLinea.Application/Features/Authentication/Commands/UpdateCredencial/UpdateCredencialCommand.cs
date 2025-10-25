using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateCredencial;

/// <summary>
/// Command para actualizar credencial completa (password + activo + email)
/// Réplica de SuscripcionesService.actualizarCredenciales() del Legacy
/// GAP-012: Update full credential en una sola operación
/// </summary>
public sealed record UpdateCredencialCommand : IRequest<bool>
{
    /// <summary>
    /// ID del usuario (GUID)
    /// </summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>
    /// Email nuevo o actual (requerido)
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Password nueva (sin hashear - se hasheará en el handler)
    /// Si es null o vacío, no se actualiza el password
    /// </summary>
    public string? Password { get; init; }

    /// <summary>
    /// Estado de activación de la cuenta
    /// </summary>
    public bool Activo { get; init; }
}
