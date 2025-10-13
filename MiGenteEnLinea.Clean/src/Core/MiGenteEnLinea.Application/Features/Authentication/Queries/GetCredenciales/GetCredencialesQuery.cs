using MediatR;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetCredenciales;

/// <summary>
/// Query para obtener las credenciales de un usuario
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.asmx.cs -> obtenerCredenciales(string userID)
/// </remarks>
public record GetCredencialesQuery(string UserId) : IRequest<List<CredencialDto>>;
