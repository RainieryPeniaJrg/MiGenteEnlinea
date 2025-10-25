using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ForgotPassword;

/// <summary>
/// Command para solicitar recuperación de contraseña
/// </summary>
/// <remarks>
/// NUEVA FUNCIONALIDAD (no existe en Legacy)
/// Security best practice: password reset tokens with expiration
/// </remarks>
public sealed record ForgotPasswordCommand : IRequest<bool>
{
    /// <summary>
    /// Email del usuario que solicita recuperación
    /// </summary>
    public required string Email { get; init; }
}
