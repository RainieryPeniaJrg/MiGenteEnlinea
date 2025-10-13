using MediatR;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetPerfilByEmail;

/// <summary>
/// Query para obtener el perfil por email
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.asmx.cs -> obtenerPerfilByEmail(string email)
/// </remarks>
public record GetPerfilByEmailQuery(string Email) : IRequest<PerfilDto?>;
