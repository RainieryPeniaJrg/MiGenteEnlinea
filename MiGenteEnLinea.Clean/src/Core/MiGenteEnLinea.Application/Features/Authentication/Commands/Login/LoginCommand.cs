using MediatR;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

/// <summary>
/// Command para autenticar un usuario con email y contraseña
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.asmx.cs -> login(string email, string pass)
/// 
/// FLUJO NUEVO (JWT):
/// 1. Validar email y contraseña
/// 2. Buscar usuario en Identity (AspNetUsers)
/// 3. Verificar contraseña con UserManager
/// 4. Verificar si la cuenta está activa
/// 5. Generar access token (15 min) y refresh token (7 días)
/// 6. Guardar refresh token en base de datos
/// 7. Retornar tokens + información del usuario
/// 
/// CÓDIGOS LEGACY (referencia):
/// - 2: Login exitoso
/// - 0: Credenciales inválidas
/// - -1: Cuenta inactiva
/// </remarks>
public record LoginCommand : IRequest<AuthenticationResultDto>
{
    /// <summary>
    /// Email del usuario (único en el sistema)
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario
    /// </summary>
    public string Password { get; init; } = string.Empty;

    /// <summary>
    /// IP del cliente (para auditoría de refresh tokens)
    /// </summary>
    public string IpAddress { get; init; } = string.Empty;
}
