using MediatR;

namespace MiGenteEnLinea.Application.Features.Seguridad.Credenciales.Commands.DeleteUser;

/// <summary>
/// Command para eliminar un usuario del sistema.
/// Implementa borrarUsuario() del Legacy (LoginService.asmx.cs).
/// </summary>
/// <remarks>
/// LÓGICA LEGACY EXACTA:
/// 1. Buscar Credencial por userID + credencialID
/// 2. Hard delete (db.Credenciales.Remove)
/// 3. Cascada automática por FK constraints de DB
/// 
/// GAP-001: DELETE /api/auth/users/{userID}
/// </remarks>
public record DeleteUserCommand(string UserID, int CredencialID) : IRequest<Unit>;
