namespace MiGenteEnLinea.Infrastructure.Services;

/// <summary>
/// Configuración para el servicio del Padrón Nacional.
/// Leer desde appsettings.json sección "PadronAPI".
/// </summary>
public class PadronSettings
{
    /// <summary>
    /// URL base del API del Padrón Nacional.
    /// Ejemplo: "https://abcportal.online/Sigeinfo/public/api/"
    /// </summary>
    public string BaseUrl { get; set; } = null!;

    /// <summary>
    /// Usuario para autenticación en el API del Padrón.
    /// IMPORTANTE: Almacenar en User Secrets en desarrollo, Azure Key Vault en producción.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Contraseña para autenticación en el API del Padrón.
    /// IMPORTANTE: Almacenar en User Secrets en desarrollo, Azure Key Vault en producción.
    /// </summary>
    public string Password { get; set; } = null!;
}
