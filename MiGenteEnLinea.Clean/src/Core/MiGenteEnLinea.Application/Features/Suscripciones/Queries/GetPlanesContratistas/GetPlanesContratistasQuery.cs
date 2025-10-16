using MediatR;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetPlanesContratistas;

/// <summary>
/// Query para obtener todos los planes disponibles para contratistas.
/// </summary>
/// <remarks>
/// Legacy: SuscripcionesService.obtenerPlanesContratistas()
/// Uso: Página de selección de planes para contratistas, checkout.
/// </remarks>
public record GetPlanesContratistasQuery : IRequest<List<PlanContratista>>
{
    /// <summary>
    /// Si es true, solo retorna planes activos. Si es false, retorna todos.
    /// </summary>
    public bool SoloActivos { get; init; } = true;
}
