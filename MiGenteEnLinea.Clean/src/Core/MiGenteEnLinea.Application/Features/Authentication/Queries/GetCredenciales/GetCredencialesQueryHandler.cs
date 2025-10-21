using MediatR;
using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Authentication;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetCredenciales;

/// <summary>
/// Handler para obtener credenciales de un usuario
/// LOTE 1: Refactorizado para usar ICredencialRepository
/// </summary>
/// <remarks>
/// LÓGICA COPIADA DE: LoginService.asmx.cs -> obtenerCredenciales(string userID)
/// Legacy: db.Credenciales.Where(a => a.userID == userID).ToList()
/// Nota: En práctica, solo debería haber 1 credencial por userId, pero Legacy retorna List
/// </remarks>
public class GetCredencialesQueryHandler : IRequestHandler<GetCredencialesQuery, List<CredencialDto>>
{
    private readonly ICredencialRepository _credencialRepository;

    public GetCredencialesQueryHandler(ICredencialRepository credencialRepository)
    {
        _credencialRepository = credencialRepository;
    }

    public async Task<List<CredencialDto>> Handle(GetCredencialesQuery request, CancellationToken cancellationToken)
    {
        // LOTE 1: Usar repositorio - optimizado para traer solo 1 credencial por userId
        var credencial = await _credencialRepository.GetByUserIdAsync(request.UserId, cancellationToken);

        if (credencial == null)
            return new List<CredencialDto>();

        // Mapear a DTO (Legacy retornaba List, mantenemos compatibilidad)
        var dto = new CredencialDto
        {
            Id = credencial.Id,
            UserId = credencial.UserId,
            Email = credencial.Email.Value, // Value Object
            Activo = credencial.Activo,
            FechaCreacion = credencial.FechaActivacion,
            UltimoAcceso = credencial.UltimoAcceso
        };

        return new List<CredencialDto> { dto };
    }
}
