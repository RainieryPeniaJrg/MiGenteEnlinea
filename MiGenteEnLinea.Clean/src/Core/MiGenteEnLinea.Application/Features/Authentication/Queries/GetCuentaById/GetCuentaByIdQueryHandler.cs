using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Authentication.DTOs;

namespace MiGenteEnLinea.Application.Features.Authentication.Queries.GetCuentaById;

/// <summary>
/// Handler para obtener un perfil (Cuenta) por su ID
/// </summary>
public class GetCuentaByIdQueryHandler : IRequestHandler<GetCuentaByIdQuery, PerfilDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCuentaByIdQueryHandler> _logger;

    public GetCuentaByIdQueryHandler(
        IApplicationDbContext context,
        IMapper mapper,
        ILogger<GetCuentaByIdQueryHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PerfilDto?> Handle(GetCuentaByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Obteniendo perfil por CuentaID: {CuentaId}",
            request.CuentaId);

        // Réplica exacta de Legacy: db.Cuentas.Where(x => x.cuentaID == cuentaID).FirstOrDefault()
        var perfil = await _context.Perfiles
            .Where(p => p.PerfilId == request.CuentaId)
            .FirstOrDefaultAsync(cancellationToken);

        if (perfil == null)
        {
            _logger.LogWarning(
                "Perfil no encontrado para CuentaID: {CuentaId}",
                request.CuentaId);
            return null;
        }

        var resultado = _mapper.Map<PerfilDto>(perfil);

        _logger.LogInformation(
            "Perfil encontrado - CuentaID: {CuentaId}, UserId: {UserId}, Email: {Email}",
            request.CuentaId,
            perfil.UserId,
            perfil.Email);

        return resultado;
    }
}
