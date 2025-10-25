using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.ResendActivationEmail;

/// <summary>
/// Command para reenviar email de activación
/// Réplica de SuscripcionesService.enviarCorreoActivacion() y Registrar.aspx.cs EnviarCorreoActivacion()
/// GAP-011: Soporte para enviar con userID o email
/// </summary>
public sealed record ResendActivationEmailCommand : IRequest<bool>
{
    /// <summary>
    /// ID del usuario (opcional si se pasa Email)
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// Email del usuario (requerido)
    /// </summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>
    /// Host base para construir URL de activación (ej: "https://migente.com")
    /// </summary>
    public string Host { get; init; } = string.Empty;
}
