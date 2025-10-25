using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ChangePasswordById;

/// <summary>
/// Command para cambiar password usando credential ID
/// Réplica de SuscripcionesService.actualizarPassByID() del Legacy
/// GAP-014: Change password by credential ID (no por userID)
/// </summary>
public sealed record ChangePasswordByIdCommand : IRequest<bool>
{
    /// <summary>
    /// ID de la credencial (PK de tabla Credenciales)
    /// </summary>
    public int CredencialId { get; init; }

    /// <summary>
    /// Nueva contraseña (sin hashear - se hasheará en el handler)
    /// </summary>
    public string NewPassword { get; init; } = string.Empty;
}
