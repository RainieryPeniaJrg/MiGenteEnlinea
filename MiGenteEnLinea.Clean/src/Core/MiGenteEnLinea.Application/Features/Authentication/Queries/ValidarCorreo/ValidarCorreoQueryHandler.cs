using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.ValidarCorreo;

/// <summary>
/// Handler para validar si un correo existe
/// </summary>
/// <remarks>
/// LÓGICA COPIADA DE: LoginService.asmx.cs -> validarCorreo(string correo)
/// Legacy: db.Cuentas.Where(x => x.Email == correo).FirstOrDefault()
/// Retorna true si existe, false si está disponible
/// </remarks>
public class ValidarCorreoQueryHandler : IRequestHandler<ValidarCorreoQuery, bool>
{
    private readonly IApplicationDbContext _context;

    public ValidarCorreoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ValidarCorreoQuery request, CancellationToken cancellationToken)
    {
        // LÓGICA EXACTA DEL LEGACY
        // Validar si el email ya existe en Credenciales
        var exists = await _context.Credenciales
            .AnyAsync(x => x.Email == request.Email, cancellationToken);

        // Retorna true si existe (NO disponible), false si disponible
        return exists;
    }
}
