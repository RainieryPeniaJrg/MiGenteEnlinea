namespace MiGenteEnLinea.Domain.Interfaces.Repositories.Configuracion;

/// <summary>
/// Repositorio para la entidad ConfigCorreo.
/// Proporciona operaciones para gestionar la configuración del servidor SMTP.
/// Nota: Solo debe existir una configuración activa en el sistema.
/// </summary>
public interface IConfigCorreoRepository : IRepository<Entities.Configuracion.ConfigCorreo>
{
    /// <summary>
    /// Obtiene la configuración activa del servidor SMTP.
    /// Solo debe existir una configuración en el sistema.
    /// </summary>
    Task<Entities.Configuracion.ConfigCorreo?> GetConfiguracionActivaAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe una configuración de correo
    /// </summary>
    Task<bool> ExisteConfiguracionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene la configuración por email del remitente
    /// </summary>
    Task<Entities.Configuracion.ConfigCorreo?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
