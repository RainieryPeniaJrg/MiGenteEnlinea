using MediatR;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.Login;

/// <summary>
/// Command para autenticar un usuario
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.asmx.cs -> login(string email, string pass)
/// </remarks>
public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
