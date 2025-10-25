using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetProfileById;

/// <summary>
/// Query para obtener el perfil completo de un usuario por su ID
/// </summary>
/// <remarks>
/// Réplica de LoginService.obtenerPerfil(string userID) del Legacy
/// Utiliza VPerfiles (vista) para obtener datos combinados de Credenciales, Perfiles y PerfilesInfo
/// </remarks>
public sealed record GetProfileByIdQuery : IRequest<PerfilDto?>
{
    /// <summary>
    /// ID del usuario (GUID string)
    /// </summary>
    public required string UserId { get; init; }
}
