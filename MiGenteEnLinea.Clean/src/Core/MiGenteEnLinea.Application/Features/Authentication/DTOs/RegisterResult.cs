namespace MiGenteEnLinea.Application.Features.Authentication.DTOs;

/// <summary>
/// Resultado de la operación de registro
/// </summary>
public class RegisterResult
{
    /// <summary>
    /// Indica si el registro fue exitoso
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensaje de resultado
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// ID del usuario creado (GUID)
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Email del usuario creado
    /// </summary>
    public string? Email { get; set; }
}
