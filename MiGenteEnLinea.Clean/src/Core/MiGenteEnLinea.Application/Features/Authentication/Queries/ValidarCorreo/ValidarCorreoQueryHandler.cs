using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.ValidarCorreo;

/// <summary>
/// Handler para validar si un correo existe
/// LOTE 1: Refactorizado para usar ICredencialRepository
/// </summary>
/// <remarks>
/// LÓGICA COPIADA DE: LoginService.asmx.cs -> validarCorreo(string correo)
/// Legacy: db.Cuentas.Where(x => x.Email == correo).FirstOrDefault()
/// Retorna true si existe, false si está disponible
/// </remarks>
public class ValidarCorreoQueryHandler : IRequestHandler<ValidarCorreoQuery, bool>
{
    private readonly ICredencialRepository _credencialRepository;

    public ValidarCorreoQueryHandler(ICredencialRepository credencialRepository)
    {
        _credencialRepository = credencialRepository;
    }

    public async Task<bool> Handle(ValidarCorreoQuery request, CancellationToken cancellationToken)
    {
        // LOTE 1: Usando repositorio optimizado con case-insensitive
        // Validar si el email ya existe en Credenciales
        var exists = await _credencialRepository.ExistsByEmailAsync(request.Email, cancellationToken);

        // Retorna true si existe (NO disponible), false si disponible
        return exists;
    }
}
