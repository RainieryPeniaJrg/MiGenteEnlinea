namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetProfileById;

/// <summary>
/// DTO con la información completa del perfil de usuario
/// </summary>
public sealed record PerfilDto
{
    public int PerfilId { get; init; }
    public DateTime? FechaCreacion { get; init; }
    public string? UserId { get; init; }
    public int? Tipo { get; init; }
    public string? Nombre { get; init; }
    public string? Apellido { get; init; }
    public string? Email { get; init; }
    public string? Telefono1 { get; init; }
    public string? Telefono2 { get; init; }
    public string? Usuario { get; init; }
    
    // Información extendida (perfilesInfo)
    public int? PerfilInfoId { get; init; }
    public int? TipoIdentificacion { get; init; }
    public string? Identificacion { get; init; }
    public string? Direccion { get; init; }
    public byte[]? FotoPerfil { get; init; }
    public string? Presentacion { get; init; }
    
    // Información empresa (si aplica)
    public string? NombreComercial { get; init; }
    public string? CedulaGerente { get; init; }
    public string? NombreGerente { get; init; }
    public string? ApellidoGerente { get; init; }
    public string? DireccionGerente { get; init; }
}
