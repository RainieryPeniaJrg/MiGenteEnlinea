using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.RevokeToken;

/// <summary>
/// Command para revocar un refresh token (logout)
/// </summary>
/// <remarks>
/// FLUJO:
/// 1. Cliente envía refresh token a revocar + IP address
/// 2. IIdentityService busca el token en base de datos
/// 3. Si existe y está activo, lo revoca (Revoked = DateTime.UtcNow)
/// 4. Si ya está revocado, la operación es idempotente (no falla)
/// 
/// CASOS DE USO:
/// - Logout de usuario (revoca el refresh token actual)
/// - Cambio de contraseña (revocar todos los tokens del usuario)
/// - Revocación por admin (seguridad)
/// - Detección de actividad sospechosa
/// 
/// SEGURIDAD:
/// - El token revocado NO puede volver a usarse
/// - Se registra IP de quien revoca (audit trail)
/// - Se registra razón de revocación
/// - El access token sigue siendo válido hasta que expire (15 min max)
/// </remarks>
/// <param name="RefreshToken">Refresh token a revocar</param>
/// <param name="IpAddress">IP del cliente que revoca (audit)</param>
/// <param name="Reason">Razón de revocación (opcional, default: "User logout")</param>
public record RevokeTokenCommand(
    string RefreshToken,
    string IpAddress,
    string? Reason = null
) : IRequest<Unit>;
