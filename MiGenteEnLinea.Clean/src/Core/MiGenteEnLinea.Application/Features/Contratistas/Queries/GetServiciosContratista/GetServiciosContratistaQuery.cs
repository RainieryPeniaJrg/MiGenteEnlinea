using MediatR;
using MiGenteEnLinea.Application.Features.Contratistas.Common;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.GetServiciosContratista;

/// <summary>
/// Query: Obtiene todos los servicios de un contratista
/// </summary>
/// <remarks>
/// LÓGICA LEGACY: ContratistasService.getServicios(contratistaID)
/// REPLICACIÓN: obtenerServicios() en index_contratista.aspx.cs
/// USO: Mostrar lista de servicios en perfil del contratista
/// </remarks>
/// <param name="ContratistaId">ID del contratista</param>
public record GetServiciosContratistaQuery(int ContratistaId) : IRequest<List<ServicioContratistaDto>>;
