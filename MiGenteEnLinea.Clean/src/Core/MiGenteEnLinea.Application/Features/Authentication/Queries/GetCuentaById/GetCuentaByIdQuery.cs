using MediatR;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetCuentaById;

/// <summary>
/// Query para obtener un perfil (Cuenta) por su ID
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.getPerfilByID(int cuentaID) (línea 179)
/// 
/// LEGACY BEHAVIOR:
/// - Simple query: db.Cuentas.Where(x => x.cuentaID == cuentaID).FirstOrDefault()
/// - Retorna null si no encuentra el perfil
/// 
/// CLEAN ARCHITECTURE:
/// - Query CQRS con AutoMapper
/// - Retorna PerfilDto (consistente con otros endpoints)
/// - Mantiene paridad 100% con Legacy (retorna null si no existe)
/// </remarks>
public record GetCuentaByIdQuery(int CuentaId) : IRequest<PerfilDto?>;
