using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ActivateAccount;

/// <summary>
/// Command para activar una cuenta de usuario
/// </summary>
/// <remarks>
/// Réplica de Activar.aspx.cs + SuscripcionesService.guardarCredenciales() del Legacy
/// Activa una cuenta que fue creada pero aún no activada (activo=false)
/// </remarks>
public sealed record ActivateAccountCommand : IRequest<bool>
{
    /// <summary>
    /// ID del usuario (GUID)
    /// </summary>
    public required string UserId { get; init; }
    
    /// <summary>
    /// Email del usuario (para validación adicional)
    /// </summary>
    public required string Email { get; init; }
}
