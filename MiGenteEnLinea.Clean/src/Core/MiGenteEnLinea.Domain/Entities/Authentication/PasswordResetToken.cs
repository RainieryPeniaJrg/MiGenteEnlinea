using MiGenteEnLinea.Domain.Common;

namespace MiGenteEnLinea.Domain.Entities.Authentication;

/// <summary>
/// Entidad para tokens de recuperación de contraseña
/// </summary>
/// <remarks>
/// Security best practice: Tokens con expiración para password reset
/// Implementado para LOTE 2 - ForgotPassword/ResetPassword
/// </remarks>
public sealed class PasswordResetToken : AuditableEntity
{
    /// <summary>
    /// ID del token (PK)
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// UserId de la credencial (FK)
    /// </summary>
    public string UserId { get; private set; } = null!;

    /// <summary>
    /// Email del usuario (para validación adicional)
    /// </summary>
    public string Email { get; private set; } = null!;

    /// <summary>
    /// Token de 6 dígitos
    /// </summary>
    public string Token { get; private set; } = null!;

    /// <summary>
    /// Fecha de expiración del token (15 minutos desde creación)
    /// </summary>
    public DateTime ExpiresAt { get; private set; }

    /// <summary>
    /// Fecha en que se usó el token (null si no se ha usado)
    /// </summary>
    public DateTime? UsedAt { get; private set; }

    /// <summary>
    /// Indica si el token ha sido usado
    /// </summary>
    public bool IsUsed => UsedAt.HasValue;

    /// <summary>
    /// Indica si el token ha expirado
    /// </summary>
    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    /// <summary>
    /// Indica si el token es válido (no usado y no expirado)
    /// </summary>
    public bool IsValid => !IsUsed && !IsExpired;

    // Constructor privado para EF Core
    private PasswordResetToken() { }

    /// <summary>
    /// Crea un nuevo token de recuperación de contraseña
    /// </summary>
    public static PasswordResetToken Create(string userId, string email, string token, int expirationMinutes = 15)
    {
        return new PasswordResetToken
        {
            UserId = userId,
            Email = email,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System"
        };
    }

    /// <summary>
    /// Marca el token como usado
    /// </summary>
    public void MarkAsUsed()
    {
        if (IsUsed)
        {
            throw new InvalidOperationException("Token ya ha sido usado");
        }

        if (IsExpired)
        {
            throw new InvalidOperationException("Token expirado");
        }

        UsedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = "System";
    }

    /// <summary>
    /// Valida el token contra un token proporcionado
    /// </summary>
    public bool ValidateToken(string providedToken)
    {
        return IsValid && Token == providedToken;
    }
}
