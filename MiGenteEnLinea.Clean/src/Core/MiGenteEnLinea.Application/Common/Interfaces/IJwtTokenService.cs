using System.Security.Claims;

namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para generar y validar tokens JWT con ASP.NET Core Identity
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Genera un access token JWT para el usuario autenticado
    /// </summary>
    /// <param name="userId">ID del usuario (AspNetUsers.Id)</param>
    /// <param name="email">Email del usuario</param>
    /// <param name="tipo">Tipo de usuario ("1" Empleador, "2" Contratista)</param>
    /// <param name="nombreCompleto">Nombre completo del usuario</param>
    /// <param name="planId">ID del plan de suscripción</param>
    /// <param name="roles">Roles del usuario (de Identity)</param>
    /// <returns>Access token JWT firmado (duración: 15 minutos)</returns>
    string GenerateAccessToken(
        string userId,
        string email,
        string tipo,
        string nombreCompleto,
        int planId,
        IEnumerable<string>? roles = null);

    /// <summary>
    /// Genera un refresh token criptográficamente seguro
    /// </summary>
    /// <param name="ipAddress">IP del cliente que solicita el token</param>
    /// <returns>Datos del refresh token (token, expiración, IP)</returns>
    RefreshTokenData GenerateRefreshToken(string ipAddress);

    /// <summary>
    /// Valida un access token y extrae los claims (ignora expiración)
    /// Útil para refresh token flow cuando el access token ya expiró
    /// </summary>
    /// <param name="token">Access token a validar</param>
    /// <returns>ClaimsPrincipal con los claims del token, o null si es inválido</returns>
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    /// <summary>
    /// Valida un access token (incluye verificación de expiración)
    /// </summary>
    /// <param name="token">Token a validar</param>
    /// <returns>True si el token es válido y no ha expirado</returns>
    bool ValidateToken(string token);

    /// <summary>
    /// Obtiene el userId desde un token JWT válido
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>UserID extraído del token, o null si el token es inválido</returns>
    string? GetUserIdFromToken(string token);
}

/// <summary>
/// Datos del refresh token generado
/// </summary>
/// <param name="Token">El refresh token (base64, 64 bytes)</param>
/// <param name="Expires">Fecha de expiración (7 días desde creación)</param>
/// <param name="CreatedByIp">IP del cliente que solicitó el token</param>
public record RefreshTokenData(
    string Token,
    DateTime Expires,
    string CreatedByIp);
