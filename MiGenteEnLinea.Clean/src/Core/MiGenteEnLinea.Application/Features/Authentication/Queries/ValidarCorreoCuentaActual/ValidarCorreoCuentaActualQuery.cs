using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.ValidarCorreoCuentaActual;

/// <summary>
/// Method #47: Query para validar si un correo pertenece a la cuenta actual del usuario
/// </summary>
/// <remarks>
/// Migrado desde: SuscripcionesService.validarCorreoCuentaActual(string correo, string userID) - línea 220
/// 
/// **Legacy Code:**
/// <code>
/// public Cuentas validarCorreoCuentaActual(string correo, string userID)
/// {
///     using (var db = new migenteEntities())
///     {
///         var result = db.Cuentas.Where(x => x.Email == correo && x.userID==userID).Include(a => a.perfilesInfo).FirstOrDefault();
///         if (result != null)
///         {
///             return result;
///         }
///     };
///     return null;
/// }
/// </code>
/// 
/// **Business Rules:**
/// - Valida que el correo exista Y pertenezca al userID específico
/// - Usado para verificar propiedad antes de cambios de email
/// - Previene conflictos cuando usuario intenta cambiar a email de otra cuenta
/// - Retorna null si no existe o no pertenece al usuario
/// 
/// **Use Cases:**
/// - Validación antes de actualizar email en perfil
/// - Verificación de propiedad de cuenta
/// - Prevención de duplicados en cambio de email
/// </remarks>
public record ValidarCorreoCuentaActualQuery(
    string Email,
    string UserId
) : IRequest<bool>;
