using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.DeleteUserCredential;

/// <summary>
/// Command para eliminar una credencial específica de un usuario
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.borrarUsuario(string userID, int credencialID)
/// 
/// Reglas de negocio Legacy:
/// - Usuario puede tener múltiples credenciales (emails diferentes)
/// - Al eliminar, se hace DELETE directo en EF6
/// - No hay validación de última credencial en Legacy (MEJORADO en Clean)
/// 
/// Mejoras en Clean Architecture:
/// - Validación: Usuario debe tener al menos 1 credencial activa
/// - Authorization: Solo el propio usuario o admin puede eliminar
/// - Logging: Se registra la eliminación para auditoría
/// </remarks>
public record DeleteUserCredentialCommand(
    string UserId,
    int CredentialId
) : IRequest<Unit>;
