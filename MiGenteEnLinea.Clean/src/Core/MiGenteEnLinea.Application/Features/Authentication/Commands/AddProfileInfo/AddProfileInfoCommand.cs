using MediatR;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.AddProfileInfo;

/// <summary>
/// Command para agregar información extendida de perfil de usuario
/// </summary>
/// <remarks>
/// Migrado desde: LoginService.agregarPerfilInfo(perfilesInfo info)
/// 
/// LÓGICA LEGACY:
/// - Recibe objeto perfilesInfo completo
/// - Lo inserta en BD usando db.perfilesInfo.Add(info)
/// - Retorna bool (true=success)
/// 
/// IMPORTANTE:
/// - Legacy usaba entidad con propiedades lowercase (cuentaID, userID, etc.)
/// - Clean Architecture usa Domain entity con PascalCase
/// - Legacy NO validaba si ya existe perfil info para ese usuario
/// </remarks>
public record AddProfileInfoCommand(
    string UserId,
    string Identificacion,
    int? TipoIdentificacion = null,
    string? NombreComercial = null,
    string? Direccion = null,
    string? Presentacion = null,
    byte[]? FotoPerfil = null,
    string? CedulaGerente = null,
    string? NombreGerente = null,
    string? ApellidoGerente = null,
    string? DireccionGerente = null
) : IRequest<int>; // Returns the ID of the created PerfilInfo
