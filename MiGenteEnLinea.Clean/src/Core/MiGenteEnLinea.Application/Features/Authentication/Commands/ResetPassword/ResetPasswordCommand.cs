using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ResetPassword;

/// <summary>
/// Command para resetear contraseña usando token de recuperación
/// </summary>
/// <remarks>
/// NUEVA FUNCIONALIDAD (no existe en Legacy)
/// Security best practice: token validation before password reset
/// </remarks>
public sealed record ResetPasswordCommand : IRequest<bool>
{
    /// <summary>
    /// Email del usuario
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Token de recuperación (6 dígitos enviado por email)
    /// </summary>
    public required string Token { get; init; }

    /// <summary>
    /// Nueva contraseña
    /// </summary>
    public required string NewPassword { get; init; }
}
