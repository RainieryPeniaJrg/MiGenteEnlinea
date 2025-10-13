using MediatR;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.RemoveServicio;

/// <summary>
/// Command: Elimina un servicio del perfil de un contratista
/// </summary>
/// <remarks>
/// LÓGICA LEGACY: ContratistasService.removerServicio()
/// REPLICACIÓN: gridServicios_CustomButtonCallback (btnRemover) en index_contratista.aspx.cs
/// COMPORTAMIENTO: Eliminación física (hard delete) del registro
/// VALIDACIÓN: El servicio debe pertenecer al contratista especificado
/// </remarks>
/// <param name="ServicioId">ID del servicio a eliminar</param>
/// <param name="ContratistaId">ID del contratista (validación de pertenencia)</param>
public record RemoveServicioCommand(
    int ServicioId,
    int ContratistaId
) : IRequest;
