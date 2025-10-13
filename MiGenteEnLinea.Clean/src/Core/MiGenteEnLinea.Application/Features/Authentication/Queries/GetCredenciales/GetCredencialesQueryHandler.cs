using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetCredenciales;

/// <summary>
/// Handler para obtener credenciales de un usuario
/// </summary>
/// <remarks>
/// LÓGICA COPIADA DE: LoginService.asmx.cs -> obtenerCredenciales(string userID)
/// Legacy: db.Credenciales.Where(a => a.userID == userID).ToList()
/// </remarks>
public class GetCredencialesQueryHandler : IRequestHandler<GetCredencialesQuery, List<CredencialDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCredencialesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CredencialDto>> Handle(GetCredencialesQuery request, CancellationToken cancellationToken)
    {
        // LÓGICA EXACTA DEL LEGACY
        var credenciales = await _context.Credenciales
            .Where(x => x.UserId == request.UserId)
            .AsNoTracking()
            .Select(c => new CredencialDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Email = c.Email,
                Activo = c.Activo,
                FechaCreacion = c.FechaActivacion, // Cambio: FechaCreacion → FechaActivacion
                UltimoAcceso = c.UltimoAcceso
            })
            .ToListAsync(cancellationToken);

        return credenciales ?? new List<CredencialDto>();
    }
}
