using MediatR;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetPerfil;

/// <summary>
/// Query para obtener el perfil de un usuario
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.asmx.cs -> obtenerPerfil(string userID)
/// </remarks>
public record GetPerfilQuery(string UserId) : IRequest<PerfilDto?>;
