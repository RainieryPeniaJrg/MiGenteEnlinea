namespace MiGenteEnLinea.Application.Features.Authentication.DTOs;

/// <summary>
/// Resultado de la operación de cambio de contraseña
/// </summary>
public class ChangePasswordResult
{
    /// <summary>
    /// Indica si el cambio fue exitoso
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Mensaje de resultado
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
