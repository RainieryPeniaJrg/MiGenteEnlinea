using MediatR;

namespace MiGenteEnLinea.Application.Features.Contratistas.Commands.ActivarPerfil;

/// <summary>
/// Command: Activa el perfil de un contratista (lo hace visible públicamente)
/// </summary>
/// <remarks>
/// LÓGICA LEGACY: ContratistasService.ActivarPerfil(userID)
/// REPLICACIÓN: btnEstatus_Click() en index_contratista.aspx.cs
/// COMPORTAMIENTO: Cambia el campo Activo de false a true
/// NOTA: Solo contratistas activos aparecen en búsquedas y pueden recibir trabajos
/// </remarks>
/// <param name="UserId">ID del usuario (identifica al contratista)</param>
public record ActivarPerfilCommand(string UserId) : IRequest;
