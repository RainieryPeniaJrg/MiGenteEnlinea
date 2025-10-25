using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUser;

/// <summary>
/// Command para eliminar (soft delete) una cuenta de usuario.
/// Marca el usuario como inactivo en lugar de eliminarlo físicamente.
/// </summary>
public record DeleteUserCommand : IRequest<bool>
{
    /// <summary>
    /// ID del usuario a eliminar (GUID en formato string).
    /// </summary>
    public string UserID { get; init; } = string.Empty;

    /// <summary>
    /// ID de la credencial a eliminar.
    /// </summary>
    public int CredencialID { get; init; }
}
