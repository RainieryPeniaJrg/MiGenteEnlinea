namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Interfaz para el servicio de tokens JWT
/// </summary>
/// <remarks>
/// Se implementará en Infrastructure con autenticación JWT
/// </remarks>
public interface IJwtTokenService
{
    /// <summary>
    /// Genera un token JWT para el usuario
    /// </summary>
    /// <param name="userId">ID del usuario (GUID)</param>
    /// <param name="email">Email del usuario</param>
    /// <param name="tipo">Tipo de usuario (1=Empleador, 2=Contratista)</param>
    /// <param name="planId">ID del plan activo</param>
    /// <returns>Token JWT</returns>
    string GenerateToken(string userId, string email, int tipo, int? planId);

    /// <summary>
    /// Genera un refresh token para el usuario
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <returns>Refresh token</returns>
    string GenerateRefreshToken(string userId);

    /// <summary>
    /// Valida un token JWT
    /// </summary>
    /// <param name="token">Token a validar</param>
    /// <returns>True si el token es válido</returns>
    bool ValidateToken(string token);

    /// <summary>
    /// Obtiene el userId desde un token JWT
    /// </summary>
    /// <param name="token">Token JWT</param>
    /// <returns>UserID extraído del token</returns>
    string? GetUserIdFromToken(string token);
}
