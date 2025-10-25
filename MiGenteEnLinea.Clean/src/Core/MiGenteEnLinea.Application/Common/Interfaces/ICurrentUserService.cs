namespace MiGenteEnLinea.Application.Common.Interfaces;

/// <summary>
/// Servicio para obtener información del usuario actual autenticado.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// ID del usuario actual (GUID en string).
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Email del usuario actual.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Verifica si el usuario actual tiene un rol específico.
    /// </summary>
    bool IsInRole(string role);

    /// <summary>
    /// Verifica si el usuario está autenticado.
    /// </summary>
    bool IsAuthenticated { get; }
}
