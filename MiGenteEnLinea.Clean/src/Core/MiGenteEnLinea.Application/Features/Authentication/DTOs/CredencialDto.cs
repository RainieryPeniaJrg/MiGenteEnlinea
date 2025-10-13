namespace MiGenteEnLinea.Application.Features.Authentication.DTOs;

/// <summary>
/// DTO para credenciales de usuario
/// </summary>
public class CredencialDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public DateTime? UltimoAcceso { get; set; }
}
