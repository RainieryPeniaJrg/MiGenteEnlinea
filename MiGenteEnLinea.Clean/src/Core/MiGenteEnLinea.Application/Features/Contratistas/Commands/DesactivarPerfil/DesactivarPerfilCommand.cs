using MediatR;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.DesactivarPerfil;

/// <summary>
/// Command: Desactiva el perfil de un contratista (lo oculta del público)
/// </summary>
/// <remarks>
/// LÓGICA LEGACY: ContratistasService.DesactivarPerfil(userID)
/// REPLICACIÓN: btnEstatus_Click() en index_contratista.aspx.cs
/// COMPORTAMIENTO: Cambia el campo Activo de true a false
/// NOTA: Contratistas inactivos NO aparecen en búsquedas ni pueden recibir trabajos
/// </remarks>
/// <param name="UserId">ID del usuario (identifica al contratista)</param>
public record DesactivarPerfilCommand(string UserId) : IRequest;
