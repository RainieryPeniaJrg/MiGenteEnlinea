namespace MiGenteEnLinea.Domain.ReadModels;

/// <summary>
/// Vista de solo lectura que combina información de perfiles con su información extendida
/// </summary>
/// <remarks>
/// Esta vista une la tabla Perfiles con perfilesInfo para proporcionar una vista
/// completa del perfil incluyendo datos básicos, identificación, foto y datos de gerente
/// (para empresas). Optimizada para consultas que requieren toda la información del perfil.
/// </remarks>
public sealed class VistaPerfil
{
    /// <summary>
    /// ID del perfil
    /// </summary>
    public int PerfilId { get; init; }

    /// <summary>
    /// Fecha de creación del perfil
    /// </summary>
    public DateTime? FechaCreacion { get; init; }

    /// <summary>
    /// ID del usuario credencial
    /// </summary>
    public string? UserId { get; init; }

    /// <summary>
    /// Tipo de perfil (1=Empleador, 2=Contratista)
    /// </summary>
    public int? Tipo { get; init; }

    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public string? Nombre { get; init; }

    /// <summary>
    /// Apellido del usuario
    /// </summary>
    public string? Apellido { get; init; }

    /// <summary>
    /// Email del usuario
    /// </summary>
    public string? Email { get; init; }

    /// <summary>
    /// Teléfono principal
    /// </summary>
    public string? Telefono1 { get; init; }

    /// <summary>
    /// Teléfono secundario
    /// </summary>
    public string? Telefono2 { get; init; }

    /// <summary>
    /// Usuario/alias
    /// </summary>
    public string? Usuario { get; init; }

    /// <summary>
    /// ID de la información extendida (perfilesInfo)
    /// </summary>
    public int? Id { get; init; }

    /// <summary>
    /// Tipo de identificación (1=Cédula, 2=Pasaporte, 3=RNC)
    /// </summary>
    public int? TipoIdentificacion { get; init; }

    /// <summary>
    /// Número de identificación
    /// </summary>
    public string? Identificacion { get; init; }

    /// <summary>
    /// Dirección física
    /// </summary>
    public string? Direccion { get; init; }

    /// <summary>
    /// Foto de perfil (binario)
    /// </summary>
    public byte[]? FotoPerfil { get; init; }

    /// <summary>
    /// Presentación o biografía del usuario
    /// </summary>
    public string? Presentacion { get; init; }

    /// <summary>
    /// Nombre comercial (para empresas)
    /// </summary>
    public string? NombreComercial { get; init; }

    /// <summary>
    /// Cédula del gerente (para empresas)
    /// </summary>
    public string? CedulaGerente { get; init; }

    /// <summary>
    /// Nombre del gerente (para empresas)
    /// </summary>
    public string? NombreGerente { get; init; }

    /// <summary>
    /// Apellido del gerente (para empresas)
    /// </summary>
    public string? ApellidoGerente { get; init; }

    /// <summary>
    /// Dirección del gerente (para empresas)
    /// </summary>
    public string? DireccionGerente { get; init; }
}
