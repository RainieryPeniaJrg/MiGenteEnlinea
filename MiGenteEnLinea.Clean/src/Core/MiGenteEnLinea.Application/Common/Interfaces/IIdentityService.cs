using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio de identidad y autenticación usando ASP.NET Core Identity
/// </summary>
/// <remarks>
/// Esta interfaz abstrae UserManager y permite a Application layer
/// trabajar con autenticación sin depender de Infrastructure
/// </remarks>
public interface IIdentityService
{
    /// <summary>
    /// Autentica un usuario con email y contraseña, genera tokens JWT
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <param name="password">Contraseña</param>
    /// <param name="ipAddress">IP del cliente (para refresh token tracking)</param>
    /// <returns>Resultado de autenticación con tokens y datos del usuario</returns>
    Task<AuthenticationResultDto> LoginAsync(string email, string password, string ipAddress);

    /// <summary>
    /// Renueva un access token usando un refresh token válido
    /// </summary>
    /// <param name="refreshToken">Refresh token activo</param>
    /// <param name="ipAddress">IP del cliente</param>
    /// <returns>Nuevo access token y refresh token rotado</returns>
    Task<AuthenticationResultDto> RefreshTokenAsync(string refreshToken, string ipAddress);

    /// <summary>
    /// Revoca un refresh token (logout)
    /// </summary>
    /// <param name="refreshToken">Refresh token a revocar</param>
    /// <param name="ipAddress">IP del cliente que revoca</param>
    /// <param name="reason">Razón de revocación (ej: "User logout")</param>
    Task RevokeTokenAsync(string refreshToken, string ipAddress, string? reason = null);

    /// <summary>
    /// Registra un nuevo usuario en el sistema
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <param name="password">Contraseña</param>
    /// <param name="nombreCompleto">Nombre completo</param>
    /// <param name="tipo">Tipo de usuario ("1" Empleador, "2" Contratista)</param>
    /// <returns>ID del usuario creado</returns>
    Task<string> RegisterAsync(string email, string password, string nombreCompleto, string tipo);

    /// <summary>
    /// Verifica si un usuario existe por email
    /// </summary>
    Task<bool> UserExistsAsync(string email);

    /// <summary>
    /// Confirma el email de un usuario (activación de cuenta)
    /// </summary>
    Task<bool> ConfirmEmailAsync(string userId, string token);

    /// <summary>
    /// Genera token para reset de contraseña
    /// </summary>
    Task<string> GeneratePasswordResetTokenAsync(string email);

    /// <summary>
    /// Resetea la contraseña de un usuario
    /// </summary>
    Task<bool> ResetPasswordAsync(string email, string token, string newPassword);
}
