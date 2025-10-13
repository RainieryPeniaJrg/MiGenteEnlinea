using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetPerfilByEmail;

/// <summary>
/// Handler para obtener perfil por email
/// </summary>
/// <remarks>
/// LÓGICA COPIADA DE: LoginService.asmx.cs -> obtenerPerfilByEmail(string email)
/// Legacy: db.VPerfiles.Where(a => a.emailUsuario == email).FirstOrDefault()
/// </remarks>
public class GetPerfilByEmailQueryHandler : IRequestHandler<GetPerfilByEmailQuery, PerfilDto?>
{
    private readonly IApplicationDbContext _context;

    public GetPerfilByEmailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PerfilDto?> Handle(GetPerfilByEmailQuery request, CancellationToken cancellationToken)
    {
        // LÓGICA EXACTA DEL LEGACY
        var perfil = await _context.VPerfiles
            .Where(v => v.Email == request.Email) // Cambio: EmailUsuario → Email
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (perfil == null)
            return null;

        // Mapear solo las propiedades que existen en VistaPerfil
        return new PerfilDto
        {
            UserId = perfil.UserId,
            Nombre = perfil.Nombre,
            Apellido = perfil.Apellido,
            Tipo = perfil.Tipo,
            Telefono1 = perfil.Telefono1,
            Telefono2 = perfil.Telefono2,
            FechaCreacion = perfil.FechaCreacion,
            Email = perfil.Email,
            PerfilId = perfil.PerfilId
        };
    }
}
