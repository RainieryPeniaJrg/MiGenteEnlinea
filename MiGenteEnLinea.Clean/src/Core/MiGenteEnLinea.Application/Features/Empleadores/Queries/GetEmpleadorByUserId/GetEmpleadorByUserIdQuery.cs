using MediatR;
using MiGenteEnLinea.Application.Features.Empleadores.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleadores.Queries.GetEmpleadorByUserId;

/// <summary>
/// Query: Obtener Empleador por UserId
/// </summary>
/// <remarks>
/// RÉPLICA DE: LoginService.getPerfilInfo(userID) pero específico para Empleador
/// LEGACY: Usa VPerfiles (view) que combina múltiples tablas
/// 
/// CLEAN: Consulta directa a tabla Empleadores (Ofertantes)
/// 
/// LÓGICA DE NEGOCIO:
/// - Buscar empleador por userId
/// - Retornar null si no existe
/// - Mapear a EmpleadorDto (sin incluir byte[] foto)
/// </remarks>
public record GetEmpleadorByUserIdQuery(string UserId) : IRequest<EmpleadorDto?>;
