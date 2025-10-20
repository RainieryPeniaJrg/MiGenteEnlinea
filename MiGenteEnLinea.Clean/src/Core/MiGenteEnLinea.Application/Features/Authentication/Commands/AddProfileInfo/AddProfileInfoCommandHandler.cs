using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Seguridad;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.AddProfileInfo;

/// <summary>
/// Handler para agregar información extendida de perfil
/// </summary>
public class AddProfileInfoCommandHandler : IRequestHandler<AddProfileInfoCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<AddProfileInfoCommandHandler> _logger;

    public AddProfileInfoCommandHandler(
        IApplicationDbContext context,
        ILogger<AddProfileInfoCommandHandler> _logger)
    {
        _context = context;
        this._logger = _logger;
    }

    public async Task<int> Handle(AddProfileInfoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Agregando información de perfil para usuario {UserId}",
            request.UserId);

        // LÓGICA LEGACY EXACTA:
        // - El método agregarPerfilInfo() NO valida si ya existe un perfil info
        // - Simplemente hace db.perfilesInfo.Add(info) y db.SaveChanges()
        // - Por lo tanto, NO agregamos validación de existencia aquí (mantener comportamiento Legacy)

        // Crear entidad usando factory method según el tipo
        PerfilesInfo perfilInfo;

        if (!string.IsNullOrWhiteSpace(request.NombreComercial))
        {
            // Es una empresa (tiene nombre comercial)
            perfilInfo = PerfilesInfo.CrearPerfilEmpresa(
                request.UserId,
                request.Identificacion,
                request.NombreComercial,
                request.Direccion,
                request.Presentacion);
        }
        else
        {
            // Es persona física
            perfilInfo = PerfilesInfo.CrearPerfilPersonaFisica(
                request.UserId,
                request.Identificacion,
                request.Direccion,
                request.Presentacion);
        }

        // Configurar foto de perfil si viene
        if (request.FotoPerfil != null && request.FotoPerfil.Length > 0)
        {
            perfilInfo.ActualizarFotoPerfil(request.FotoPerfil);
        }

        // Configurar información del gerente si viene
        if (!string.IsNullOrWhiteSpace(request.CedulaGerente) ||
            !string.IsNullOrWhiteSpace(request.NombreGerente) ||
            !string.IsNullOrWhiteSpace(request.ApellidoGerente))
        {
            perfilInfo.ActualizarInformacionGerente(
                request.CedulaGerente,
                request.NombreGerente,
                request.ApellidoGerente,
                request.DireccionGerente);
        }

        // Agregar a contexto
        _context.PerfilesInfos.Add(perfilInfo);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Perfil info {PerfilInfoId} creado exitosamente para usuario {UserId}",
            perfilInfo.Id,
            request.UserId);

        return perfilInfo.Id;
    }
}
