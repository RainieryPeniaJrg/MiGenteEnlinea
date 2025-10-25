using MediatR;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.GetCedulaByUserId;

/// <summary>
/// Query para obtener cédula/identificación de contratista por userId
/// Réplica de SuscripcionesService.obtenerCedula() del Legacy
/// GAP-013: Endpoint simple para obtener identificación
/// </summary>
public sealed record GetCedulaByUserIdQuery : IRequest<string?>
{
    /// <summary>
    /// ID del usuario (GUID)
    /// </summary>
    public string UserId { get; init; } = string.Empty;
}
