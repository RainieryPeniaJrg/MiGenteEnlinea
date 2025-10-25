using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfile;

/// <summary>
/// Command para actualizar el perfil de un usuario
/// </summary>
/// <remarks>
/// Réplica simplificada de LoginService.actualizarPerfil() del Legacy
/// </remarks>
public sealed record UpdateProfileCommand : IRequest<bool>
{
    /// <summary>
    /// ID del usuario (GUID)
    /// </summary>
    public required string UserID { get; init; }
    
    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public required string Nombre { get; init; }
    
    /// <summary>
    /// Apellido del usuario
    /// </summary>
    public required string Apellido { get; init; }
    
    /// <summary>
    /// Email del usuario (opcional)
    /// </summary>
    public string? Email { get; init; }
}
