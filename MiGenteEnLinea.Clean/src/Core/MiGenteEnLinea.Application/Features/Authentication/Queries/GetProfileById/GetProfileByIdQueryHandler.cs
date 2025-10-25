using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetProfileById;

/// <summary>
/// Handler para obtener el perfil completo de un usuario
/// </summary>
public sealed class GetProfileByIdQueryHandler : IRequestHandler<GetProfileByIdQuery, PerfilDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetProfileByIdQueryHandler> _logger;

    public GetProfileByIdQueryHandler(
        IApplicationDbContext context,
        ILogger<GetProfileByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PerfilDto?> Handle(GetProfileByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetProfileById: UserId={UserId}", request.UserId);

        // Query VPerfiles (vista) - réplica exacta de Legacy
        // Legacy: return db.VPerfiles.Where(a => a.userID == userID).FirstOrDefault();
        var vPerfil = await _context.VPerfiles
            .Where(a => a.UserId == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (vPerfil == null)
        {
            _logger.LogWarning("GetProfileById: Perfil no encontrado para UserId={UserId}", request.UserId);
            return null;
        }

        // Mapeo manual (AutoMapper se puede agregar después)
        var dto = new PerfilDto
        {
            PerfilId = vPerfil.PerfilId,
            FechaCreacion = vPerfil.FechaCreacion,
            UserId = vPerfil.UserId,
            Tipo = vPerfil.Tipo,
            Nombre = vPerfil.Nombre,
            Apellido = vPerfil.Apellido,
            Email = vPerfil.Email,
            Telefono1 = vPerfil.Telefono1,
            Telefono2 = vPerfil.Telefono2,
            Usuario = vPerfil.Usuario,
            
            // Información extendida
            PerfilInfoId = vPerfil.Id,
            TipoIdentificacion = vPerfil.TipoIdentificacion,
            Identificacion = vPerfil.Identificacion,
            Direccion = vPerfil.Direccion,
            FotoPerfil = vPerfil.FotoPerfil,
            Presentacion = vPerfil.Presentacion,
            
            // Información empresa
            NombreComercial = vPerfil.NombreComercial,
            CedulaGerente = vPerfil.CedulaGerente,
            NombreGerente = vPerfil.NombreGerente,
            ApellidoGerente = vPerfil.ApellidoGerente,
            DireccionGerente = vPerfil.DireccionGerente
        };

        _logger.LogInformation("GetProfileById: Perfil encontrado - PerfilId={PerfilId}, Nombre={Nombre}",
            dto.PerfilId, dto.Nombre);

        return dto;
    }
}
