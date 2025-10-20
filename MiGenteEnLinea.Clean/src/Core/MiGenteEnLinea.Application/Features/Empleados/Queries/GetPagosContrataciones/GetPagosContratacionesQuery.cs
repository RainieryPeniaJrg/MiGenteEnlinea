using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetPagosContrataciones;

/// <summary>
/// Query to get payment records for a specific contratacion and detalle
/// Migrated from: EmpleadosService.GetEmpleador_RecibosContratacionesByID(int contratacionID, int detalleID) - line 360
/// </summary>
public record GetPagosContratacionesQuery(int ContratacionId, int DetalleId) : IRequest<List<PagoContratacionDto>>;
