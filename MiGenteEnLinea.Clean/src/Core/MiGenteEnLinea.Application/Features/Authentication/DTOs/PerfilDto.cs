namespace MiGenteEnLinea.Application.Features.Authentication.DTOs;

/// <summary>
/// DTO para el perfil de usuario (basado en VPerfiles view)
/// </summary>
public class PerfilDto
{
    public string? UserId { get; set; }
    public string? EmailUsuario { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public int? Tipo { get; set; }
    public string? Telefono1 { get; set; }
    public string? Telefono2 { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public string? Email { get; set; }
    public int? CuentaId { get; set; }
    public int? PerfilId { get; set; }
    public string? Sexo { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string? EstadoCivil { get; set; }
    public int? ProvinciaId { get; set; }
    public string? ProvinciaStr { get; set; }
    public string? Sector { get; set; }
    public string? Calle { get; set; }
    public string? NumeroCasa { get; set; }
}
