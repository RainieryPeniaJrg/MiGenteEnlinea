using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Commands.UpdateProfileExtended;

/// <summary>
/// Handler para actualizar perfil completo (Perfile + PerfilesInfo)
/// </summary>
public class UpdateProfileExtendedCommandHandler : IRequestHandler<UpdateProfileExtendedCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<UpdateProfileExtendedCommandHandler> _logger;

    public UpdateProfileExtendedCommandHandler(
        IApplicationDbContext context,
        ILogger<UpdateProfileExtendedCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateProfileExtendedCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Actualizando perfil extendido - UserId: {UserId}, Email: {Email}",
            request.UserId,
            request.Email);

        // ================================================================================
        // PASO 1: ACTUALIZAR PERFILE (antes Cuentas)
        // ================================================================================
        // Legacy: db1.Entry(cuenta).State = EntityState.Modified; db1.SaveChanges()
        
        var perfil = await _context.Perfiles
            .Where(p => p.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (perfil == null)
        {
            _logger.LogWarning("Perfil no encontrado para UserId: {UserId}", request.UserId);
            return false;
        }

        // Usar domain method para actualizar (garantiza invariantes)
        perfil.ActualizarInformacionBasica(
            request.Nombre,
            request.Apellido,
            request.Email,
            request.Telefono1,
            request.Telefono2,
            request.Usuario);

        // ================================================================================
        // PASO 2: ACTUALIZAR PERFILESINFO (si existen datos)
        // ================================================================================
        // Legacy: db.Entry(info).State = EntityState.Modified; db.SaveChanges()
        
        bool tieneInfoAdicional = 
            request.Identificacion != null ||
            request.TipoIdentificacion.HasValue ||
            request.NombreComercial != null ||
            request.Direccion != null ||
            request.Presentacion != null ||
            request.FotoPerfil != null ||
            request.CedulaGerente != null ||
            request.NombreGerente != null ||
            request.ApellidoGerente != null ||
            request.DireccionGerente != null;

        if (tieneInfoAdicional)
        {
            var perfilInfo = await _context.PerfilesInfos
                .Where(pi => pi.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (perfilInfo != null)
            {
                if (request.Identificacion != null)
                {
                    perfilInfo.ActualizarIdentificacion(
                        request.Identificacion,
                        request.TipoIdentificacion);
                }

                if (request.NombreComercial != null)
                {
                    perfilInfo.ActualizarNombreComercial(request.NombreComercial);
                }

                if (request.Direccion != null)
                {
                    perfilInfo.ActualizarDireccion(request.Direccion);
                }

                if (request.Presentacion != null)
                {
                    perfilInfo.ActualizarPresentacion(request.Presentacion);
                }

                // Actualizar foto de perfil si se provee
                if (request.FotoPerfil != null && request.FotoPerfil.Length > 0)
                {
                    perfilInfo.ActualizarFotoPerfil(request.FotoPerfil);
                }

                // Actualizar información del gerente si se provee algún dato
                if (request.CedulaGerente != null ||
                    request.NombreGerente != null ||
                    request.ApellidoGerente != null ||
                    request.DireccionGerente != null)
                {
                    perfilInfo.ActualizarInformacionGerente(
                        request.CedulaGerente,
                        request.NombreGerente,
                        request.ApellidoGerente,
                        request.DireccionGerente);
                }

                _logger.LogInformation(
                    "PerfilesInfo actualizado - UserId: {UserId}, PerfilInfoId: {PerfilInfoId}",
                    request.UserId,
                    perfilInfo.Id);
            }
            else
            {
                _logger.LogWarning(
                    "PerfilesInfo no encontrado para UserId: {UserId} - No se actualizó información adicional",
                    request.UserId);
            }
        }

        // ================================================================================
        // PASO 3: GUARDAR CAMBIOS (UnitOfWork pattern - 1 transacción)
        // ================================================================================
        // Legacy usa 2 DbContexts (db y db1), Clean usa 1 solo con transacción implícita
        
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Perfil extendido actualizado exitosamente - UserId: {UserId}, Email: {Email}",
            request.UserId,
            request.Email);

        return true;
    }
}
