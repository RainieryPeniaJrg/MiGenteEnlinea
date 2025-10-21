using MediatR;
using MiGenteEnLinea.Application.Features.Contratistas.Common;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.GetContratistaById;

/// <summary>
/// Query: Obtiene un contratista por su ID
/// </summary>
/// <remarks>
/// USO: Obtener el perfil público de un contratista específico
/// DIFERENCIA con GetContratistaByUserId: Este busca por ID interno (int), no por userId (GUID)
/// </remarks>
/// <param name="ContratistaId">ID del contratista</param>
public record GetContratistaByIdQuery(int ContratistaId) : IRequest<ContratistaDto?>;
