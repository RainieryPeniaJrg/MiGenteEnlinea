using Microsoft.AspNetCore.Identity;

namespace MiGenteEnLinea.Infrastructure.Identity;

/// <summary>
/// ApplicationUser: Extiende IdentityUser para agregar propiedades personalizadas
/// 
/// MAPEO CON LEGACY:
/// - IdentityUser.UserName → Credencial.UserID (identificador único)
/// - IdentityUser.Email → Credencial.Email
/// - IdentityUser.EmailConfirmed → Credencial.Activo
/// - IdentityUser.PasswordHash → Credencial.Password (migrado de Crypt.Encrypt a BCrypt)
/// 
/// PROPIEDADES ADICIONALES:
/// - Tipo: "1" (Empleador) o "2" (Contratista)
/// - NombreCompleto: Concatenación de Cuenta.Nombre + Cuenta.Apellido
/// - PlanID: ID del plan actual (0 si no tiene)
/// - VencimientoPlan: Fecha de vencimiento del plan
/// 
/// REFRESH TOKENS:
/// - Almacenados en tabla separada RefreshTokens
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Tipo de usuario: "1" (Empleador) o "2" (Contratista)
    /// Mapea a: Cuenta.Tipo
    /// </summary>
    public string Tipo { get; set; } = string.Empty;

    /// <summary>
    /// Nombre completo del usuario (para mostrar en UI)
    /// Calculado desde: Cuenta.Nombre + " " + Cuenta.Apellido
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// ID del plan de suscripción actual
    /// 0 = Sin plan (requiere compra)
    /// >0 = ID del plan activo
    /// Mapea a: Suscripcion.PlanID
    /// </summary>
    public int PlanID { get; set; }

    /// <summary>
    /// Fecha de vencimiento del plan actual
    /// null = Sin plan o plan vencido
    /// Mapea a: Suscripcion.Vencimiento
    /// </summary>
    public DateTime? VencimientoPlan { get; set; }

    /// <summary>
    /// Fecha de creación de la cuenta
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Último login exitoso
    /// </summary>
    public DateTime? UltimoLogin { get; set; }

    /// <summary>
    /// Navigation property: Refresh tokens del usuario
    /// </summary>
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
