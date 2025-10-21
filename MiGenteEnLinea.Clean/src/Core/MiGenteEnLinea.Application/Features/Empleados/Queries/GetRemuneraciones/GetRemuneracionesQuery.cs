using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetRemuneraciones;

/// <summary>
/// Query para obtener remuneraciones de un empleado
/// Migrado desde: EmpleadosService.obtenerRemuneraciones(string userID, int empleadoID)
/// 
/// LEGACY BEHAVIOR:
/// - return db.Remuneraciones.Where(x => x.userID == userID && x.empleadoID == empleadoID).ToList();
/// - Retorna todas las remuneraciones (otras remuneraciones adicionales al salario base)
/// 
/// CLEAN ARCHITECTURE:
/// - Query con validación de ownership (userID)
/// - Retorna lista de RemuneracionDto
/// </summary>
public record GetRemuneracionesQuery(
    string UserId,
    int EmpleadoId
) : IRequest<List<RemuneracionDto>>;
