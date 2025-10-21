namespace MiGenteEnLinea.Infrastructure.Identity;

/// <summary>
/// Configuración de JWT desde appsettings.json
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Sección de configuración en appsettings.json
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// Clave secreta para firmar tokens (min 32 caracteres)
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Emisor del token (quien lo genera)
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audiencia del token (para quién es)
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Duración del access token en minutos (default: 15 minutos)
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; } = 15;

    /// <summary>
    /// Duración del refresh token en días (default: 7 días)
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
