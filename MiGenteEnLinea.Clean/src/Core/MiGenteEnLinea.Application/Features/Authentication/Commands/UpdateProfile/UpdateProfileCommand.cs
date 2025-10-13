using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfile;

/// <summary>
/// Command para actualizar el perfil de un usuario
/// </summary>
/// <remarks>
/// Réplica de LoginService.actualizarPerfil() del Legacy
/// </remarks>
public sealed record UpdateProfileCommand : IRequest<bool>
{
    /// <summary>
    /// ID del usuario (GUID)
    /// </summary>
    public required string UserId { get; init; }
    
    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public required string Nombre { get; init; }
    
    /// <summary>
    /// Apellido del usuario
    /// </summary>
    public required string Apellido { get; init; }
    
    /// <summary>
    /// Email del usuario
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Teléfono 1 (opcional)
    /// </summary>
    public string? Telefono1 { get; init; }
    
    /// <summary>
    /// Teléfono 2 (opcional)
    /// </summary>
    public string? Telefono2 { get; init; }
    
    /// <summary>
    /// Nombre de usuario para login (opcional)
    /// </summary>
    public string? Usuario { get; init; }
}
