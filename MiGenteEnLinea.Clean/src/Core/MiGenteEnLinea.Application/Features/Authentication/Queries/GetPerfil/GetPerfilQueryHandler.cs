using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetPerfil;

/// <summary>
/// Handler para obtener el perfil de un usuario
/// </summary>
/// <remarks>
/// LÓGICA COPIADA DE: LoginService.asmx.cs -> obtenerPerfil(string userID)
/// Legacy: db.VPerfiles.Where(a => a.userID == userID).FirstOrDefault()
/// </remarks>
public class GetPerfilQueryHandler : IRequestHandler<GetPerfilQuery, PerfilDto?>
{
    private readonly IApplicationDbContext _context;

    public GetPerfilQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PerfilDto?> Handle(GetPerfilQuery request, CancellationToken cancellationToken)
    {
        // LÓGICA EXACTA DEL LEGACY
        var vPerfil = await _context.VPerfiles
            .Where(x => x.UserId == request.UserId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (vPerfil == null)
            return null;

        // Mapear solo propiedades que existen en VistaPerfil
        return new PerfilDto
        {
            UserId = vPerfil.UserId,
            Nombre = vPerfil.Nombre,
            Apellido = vPerfil.Apellido,
            Tipo = vPerfil.Tipo,
            Telefono1 = vPerfil.Telefono1,
            Telefono2 = vPerfil.Telefono2,
            FechaCreacion = vPerfil.FechaCreacion,
            Email = vPerfil.Email,
            PerfilId = vPerfil.PerfilId
        };
    }
}
