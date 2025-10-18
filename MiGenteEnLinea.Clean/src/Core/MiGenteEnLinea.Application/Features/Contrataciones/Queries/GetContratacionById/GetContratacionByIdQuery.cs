using MediatR;
using MiGenteEnLinea.Application.Features.Contrataciones.DTOs;

namespace MiGenteEnLinea.Application.Features.Contrataciones.Queries.GetContratacionById;

/// <summary>
/// Query para obtener una contratación específica por su ID.
/// 
/// CONTEXTO:
/// - Retorna todos los detalles de la contratación
/// - Incluye información de estado, fechas, montos
/// - Usado para vista de detalle de contratación
/// </summary>
public record GetContratacionByIdQuery : IRequest<ContratacionDetalleDto?>
{
    public int DetalleId { get; init; }
}
