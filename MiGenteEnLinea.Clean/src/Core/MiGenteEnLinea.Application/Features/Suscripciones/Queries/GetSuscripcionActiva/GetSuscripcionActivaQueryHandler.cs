using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Domain.Entities.Suscripciones;

namespace MiGenteEnLinea.Application.Features.Suscripciones.Queries.GetSuscripcionActiva;

/// <summary>
/// Handler para GetSuscripcionActivaQuery.
/// </summary>
/// <remarks>
/// LÓGICA LEGACY:
/// - Comunity1.Master.cs: Valida si usuario tiene suscripción activa antes de mostrar páginas
/// - ContratistaM.Master.cs: Mismo comportamiento para contratistas
/// - Múltiples *.aspx.cs: Verifican plan activo antes de permitir acciones
/// 
/// CRITERIOS DE SUSCRIPCIÓN ACTIVA:
/// - No cancelada (Cancelada = false)
/// - No vencida (Vencimiento >= hoy)
/// </remarks>
public class GetSuscripcionActivaQueryHandler : IRequestHandler<GetSuscripcionActivaQuery, Suscripcion?>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetSuscripcionActivaQueryHandler> _logger;

    public GetSuscripcionActivaQueryHandler(
        IApplicationDbContext context,
        ILogger<GetSuscripcionActivaQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Suscripcion?> Handle(GetSuscripcionActivaQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Buscando suscripción activa para usuario {UserId}",
            request.UserId);

        // Buscar suscripción no cancelada
        // El método EstaActiva() del dominio valida que no esté vencida
        var suscripcion = await _context.Suscripciones
            .Where(s => s.UserId == request.UserId && !s.Cancelada)
            .OrderByDescending(s => s.FechaInicio)
            .FirstOrDefaultAsync(cancellationToken);

        if (suscripcion == null)
        {
            _logger.LogInformation(
                "No se encontró suscripción activa para usuario {UserId}",
                request.UserId);
            return null;
        }

        // Verificar si está activa (no vencida)
        var estaActiva = suscripcion.EstaActiva();

        _logger.LogInformation(
            "Suscripción {SuscripcionId} encontrada. Activa: {Activa}, Vencimiento: {Vencimiento}",
            suscripcion.Id,
            estaActiva,
            suscripcion.Vencimiento);

        // Retornar la suscripción (el llamador puede verificar EstaActiva() si necesita)
        return suscripcion;
    }
}
