using MediatR;
using MiGenteEnLinea.Application.Features.Empleados.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleados.Queries.GetDeduccionesTss;

/// <summary>
/// Query para obtener catálogo de deducciones TSS (Tesorería de la Seguridad Social).
/// Migrado de: EmpleadosService.deducciones() - Line 680
/// </summary>
public record GetDeduccionesTssQuery : IRequest<List<DeduccionTssDto>>;
