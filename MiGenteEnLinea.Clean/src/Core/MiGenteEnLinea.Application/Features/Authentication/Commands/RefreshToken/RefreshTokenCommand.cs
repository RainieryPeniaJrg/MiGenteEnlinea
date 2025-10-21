using MediatR;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.RefreshToken;

/// <summary>
/// Command para renovar el access token usando un refresh token válido
/// </summary>
/// <remarks>
/// FLUJO:
/// 1. Cliente envía refresh token + IP address
/// 2. IIdentityService valida el refresh token (no expirado, no revocado)
/// 3. Si es válido, genera nuevo access token + nuevo refresh token
/// 4. Revoca el refresh token viejo (TOKEN ROTATION - seguridad)
/// 5. Retorna nuevos tokens
/// 
/// SEGURIDAD (Token Rotation):
/// - Cada refresh token solo puede usarse UNA VEZ
/// - Al usarse, se revoca y se reemplaza por uno nuevo
/// - Previene replay attacks
/// - Historial completo de rotación (ReplacedByToken chain)
/// 
/// USO:
/// - Cuando el access token expira (15 min)
/// - Cliente NO necesita pedir credenciales nuevamente
/// - Experiencia de usuario fluida (seamless authentication)
/// </remarks>
/// <param name="RefreshToken">Refresh token actual (64 bytes base64)</param>
/// <param name="IpAddress">IP del cliente (para audit tracking)</param>
public record RefreshTokenCommand(
    string RefreshToken,
    string IpAddress
) : IRequest<AuthenticationResultDto>;
