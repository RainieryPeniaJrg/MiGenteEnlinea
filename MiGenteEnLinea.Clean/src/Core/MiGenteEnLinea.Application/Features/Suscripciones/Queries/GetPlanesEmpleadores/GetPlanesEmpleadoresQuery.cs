using MediatR;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetPlanesEmpleadores;

/// <summary>
/// Query para obtener todos los planes disponibles para empleadores.
/// </summary>
/// <remarks>
/// Legacy: SuscripcionesService.obtenerPlanes()
/// Uso: Página de selección de planes, checkout, información de precios.
/// </remarks>
public record GetPlanesEmpleadoresQuery : IRequest<List<PlanEmpleador>>
{
    /// <summary>
    /// Si es true, solo retorna planes activos. Si es false, retorna todos.
    /// </summary>
    public bool SoloActivos { get; init; } = true;
}
