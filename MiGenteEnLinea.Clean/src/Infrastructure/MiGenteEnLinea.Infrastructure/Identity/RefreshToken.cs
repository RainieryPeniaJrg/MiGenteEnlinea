namespace MiGenteEnLinea.Infrastructure.Identity;

/// <summary>
/// RefreshToken: Almacena tokens de renovación para JWT
/// 
/// SEGURIDAD:
/// - Tokens generados con RNGCryptoServiceProvider (cryptographically secure)
/// - Tienen fecha de expiración (default: 7 días)
/// - Se invalidan al revocar (Revoked = true)
/// - Un usuario puede tener múltiples refresh tokens activos (múltiples dispositivos)
/// 
/// FLUJO:
/// 1. Login exitoso → Genera Access Token (15 min) + Refresh Token (7 días)
/// 2. Access Token expira → Cliente usa Refresh Token para obtener nuevo Access Token
/// 3. Refresh Token se regenera en cada renovación (rotating refresh tokens)
/// 4. Logout → Revoca el Refresh Token específico
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// ID del refresh token
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// ID del usuario propietario del token
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Token de renovación (base64, 64 bytes)
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de creación del token
    /// </summary>
    public DateTime Created { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de expiración del token
    /// Default: 7 días desde creación
    /// </summary>
    public DateTime Expires { get; set; }

    /// <summary>
    /// Fecha de revocación (si fue revocado)
    /// null = Token activo
    /// </summary>
    public DateTime? Revoked { get; set; }

    /// <summary>
    /// Token que reemplazó a este (en caso de rotating refresh tokens)
    /// </summary>
    public string? ReplacedByToken { get; set; }

    /// <summary>
    /// Razón de revocación (logout, security breach, etc.)
    /// </summary>
    public string? ReasonRevoked { get; set; }

    /// <summary>
    /// IP del cliente que creó el token
    /// </summary>
    public string? CreatedByIp { get; set; }

    /// <summary>
    /// IP del cliente que revocó el token
    /// </summary>
    public string? RevokedByIp { get; set; }

    /// <summary>
    /// Navigation property: Usuario propietario
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// Verifica si el token está activo (no expirado ni revocado)
    /// </summary>
    public bool IsActive => Revoked == null && !IsExpired;

    /// <summary>
    /// Verifica si el token expiró
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= Expires;
}
