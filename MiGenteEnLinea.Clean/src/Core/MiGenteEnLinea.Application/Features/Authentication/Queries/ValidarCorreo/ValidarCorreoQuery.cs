using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.ValidarCorreo;

/// <summary>
/// Query para validar si un correo ya existe en el sistema
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.asmx.cs -> validarCorreo(string correo)
/// Retorna true si el correo ya existe (NO disponible)
/// </remarks>
public record ValidarCorreoQuery(string Email) : IRequest<bool>;
