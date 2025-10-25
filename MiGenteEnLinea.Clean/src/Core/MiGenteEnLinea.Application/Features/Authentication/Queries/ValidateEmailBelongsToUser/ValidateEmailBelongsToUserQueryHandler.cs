using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.ValidateEmailBelongsToUser;

/// <summary>
/// Handler para validar si un correo electrónico pertenece a un usuario específico.
/// </summary>
/// <remarks>
/// Mapeo desde Legacy: SuscripcionesService.validarCorreoCuentaActual()
/// 
/// LÓGICA LEGACY:
/// ```csharp
/// var result = db.Cuentas.Where(x => x.Email == correo && x.userID == userID)
///                        .Include(a => a.perfilesInfo)
///                        .FirstOrDefault();
/// return result != null; // true si existe
/// ```
/// 
/// CASO DE USO REAL (MiPerfilEmpleador.aspx.cs línea 250):
/// - Usuario intenta crear nueva credencial en su suscripción
/// - Sistema valida si el email YA EXISTE en esa suscripción (userID)
/// - Si existe → Error: "Este Correo ya Existe en esta Suscripcion"
/// - Si no existe → Permite crear credencial
/// 
/// NOTA: El nombre del método Legacy es confuso ("cuentaActual" sugiere exclusión), 
/// pero la implementación real valida INCLUSIÓN (email pertenece a userID).
/// </remarks>
public sealed class ValidateEmailBelongsToUserQueryHandler 
    : IRequestHandler<ValidateEmailBelongsToUserQuery, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ValidateEmailBelongsToUserQueryHandler> _logger;

    public ValidateEmailBelongsToUserQueryHandler(
        IApplicationDbContext context,
        ILogger<ValidateEmailBelongsToUserQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Valida si el email pertenece al userID especificado.
    /// </summary>
    /// <returns>
    /// <c>true</c> si el email está registrado para ese userID (ya existe en la suscripción).
    /// <c>false</c> si el email NO pertenece a ese userID (disponible para crear credencial).
    /// </returns>
    public async Task<bool> Handle(
        ValidateEmailBelongsToUserQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Validando si email {Email} pertenece a userID {UserID}",
            request.Email,
            request.UserID);

        try
        {
            // LÓGICA LEGACY: WHERE Email == correo && userID == userID
            // En Legacy se consulta Cuentas, pero en Clean Architecture usamos Credenciales
            // que tiene Email (normalizado) y UserId (GUID string)
            var credencial = await _context.Credenciales
                .Where(c => c.Email.Value == request.Email && c.UserId == request.UserID)
                .FirstOrDefaultAsync(cancellationToken);

            var existe = credencial != null;

            _logger.LogInformation(
                "Validación completada: Email {Email} {Estado} en suscripción de userID {UserID}",
                request.Email,
                existe ? "YA EXISTE" : "NO EXISTE",
                request.UserID);

            return existe;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error al validar email {Email} para userID {UserID}",
                request.Email,
                request.UserID);
            throw;
        }
    }
}
