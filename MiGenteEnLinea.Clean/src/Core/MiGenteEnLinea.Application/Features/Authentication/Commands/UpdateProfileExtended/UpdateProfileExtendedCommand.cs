using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfileExtended;

/// <summary>
/// Command para actualizar perfil completo (Cuenta + PerfilesInfo)
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.actualizarPerfil(perfilesInfo info, Cuentas cuenta) (línea 136)
/// 
/// LEGACY BEHAVIOR (2 DbContexts):
/// - Actualiza perfilesInfo: db.Entry(info).State = Modified; db.SaveChanges()
/// - Actualiza Cuentas: db1.Entry(cuenta).State = Modified; db1.SaveChanges()
/// - Usa 2 DbContexts separados (db y db1)
/// - Retorna true siempre
/// 
/// CLEAN ARCHITECTURE:
/// - Usa Domain methods para actualizar (garantiza invariantes)
/// - 1 solo DbContext con UnitOfWork pattern
/// - Actualiza Perfile (antes Cuentas) y PerfilesInfo en una transacción
/// - Retorna bool indicando éxito
/// </remarks>
public record UpdateProfileExtendedCommand : IRequest<bool>
{
    // ====================================
    // DATOS DE PERFILE (antes Cuentas)
    // ====================================
    
    /// <summary>
    /// ID del usuario (GUID) - para buscar el perfil
    /// </summary>
    public required string UserId { get; init; }
    
    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public required string Nombre { get; init; }
    
    /// <summary>
    /// Apellido del usuario
    /// </summary>
    public required string Apellido { get; init; }
    
    /// <summary>
    /// Email del usuario
    /// </summary>
    public required string Email { get; init; }
    
    /// <summary>
    /// Teléfono 1 (opcional)
    /// </summary>
    public string? Telefono1 { get; init; }
    
    /// <summary>
    /// Teléfono 2 (opcional)
    /// </summary>
    public string? Telefono2 { get; init; }
    
    /// <summary>
    /// Nombre de usuario para login (opcional)
    /// </summary>
    public string? Usuario { get; init; }

    // ====================================
    // DATOS DE PERFILESINFO (opcional)
    // ====================================
    
    /// <summary>
    /// Identificación (cédula, RNC, pasaporte)
    /// </summary>
    public string? Identificacion { get; init; }
    
    /// <summary>
    /// Tipo de identificación: 1=Cédula, 2=Pasaporte, 3=RNC
    /// </summary>
    public int? TipoIdentificacion { get; init; }
    
    /// <summary>
    /// Nombre comercial (solo empresas)
    /// </summary>
    public string? NombreComercial { get; init; }
    
    /// <summary>
    /// Dirección
    /// </summary>
    public string? Direccion { get; init; }
    
    /// <summary>
    /// Presentación / Biografía
    /// </summary>
    public string? Presentacion { get; init; }
    
    /// <summary>
    /// Foto de perfil (byte array)
    /// </summary>
    public byte[]? FotoPerfil { get; init; }
    
    /// <summary>
    /// Cédula del gerente (empresas)
    /// </summary>
    public string? CedulaGerente { get; init; }
    
    /// <summary>
    /// Nombre del gerente (empresas)
    /// </summary>
    public string? NombreGerente { get; init; }
    
    /// <summary>
    /// Apellido del gerente (empresas)
    /// </summary>
    public string? ApellidoGerente { get; init; }
    
    /// <summary>
    /// Dirección del gerente (empresas)
    /// </summary>
    public string? DireccionGerente { get; init; }
}
