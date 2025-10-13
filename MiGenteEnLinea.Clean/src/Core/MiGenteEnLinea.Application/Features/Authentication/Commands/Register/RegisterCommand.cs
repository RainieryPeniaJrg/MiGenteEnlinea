using MediatR;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Register;

/// <summary>
/// Command para registrar un nuevo usuario en el sistema
/// </summary>
/// <remarks>
/// Réplica de SuscripcionesService.GuardarPerfil() del Legacy
/// Crea: Cuenta, Credencial, Contratista (si tipo=2), Suscripción inicial
/// </remarks>
public sealed record RegisterCommand : IRequest<RegisterResult>
{
    /// <summary>
    /// Email del usuario (único en el sistema)
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Contraseña sin hashear (se hasheará con BCrypt en el handler)
    /// </summary>
    public required string Password { get; init; }
    
    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public required string Nombre { get; init; }
    
    /// <summary>
    /// Apellido del usuario
    /// </summary>
    public required string Apellido { get; init; }
    
    /// <summary>
    /// Tipo de usuario: 1 = Empleador, 2 = Contratista
    /// </summary>
    public required int Tipo { get; init; }
    
    /// <summary>
    /// Teléfono 1 (opcional)
    /// </summary>
    public string? Telefono1 { get; init; }
    
    /// <summary>
    /// Teléfono 2 (opcional)
    /// </summary>
    public string? Telefono2 { get; init; }
    
    /// <summary>
    /// URL del host para generar el link de activación
    /// Ejemplo: "https://migenteenlinea.com"
    /// </summary>
    public required string Host { get; init; }
}
