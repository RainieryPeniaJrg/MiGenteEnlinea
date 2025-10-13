using MediatR;
using MiGenteEnLinea.Application.Features.Contratistas.Common;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.GetContratistaByUserId;

/// <summary>
/// Query: Obtiene un contratista por su userId
/// </summary>
/// <remarks>
/// LÓGICA LEGACY: ContratistasService.getMiPerfil(userID)
/// USO: Obtener el perfil del contratista autenticado
/// </remarks>
/// <param name="UserId">ID del usuario (GUID de Credenciales)</param>
public record GetContratistaByUserIdQuery(string UserId) : IRequest<ContratistaDto?>;
