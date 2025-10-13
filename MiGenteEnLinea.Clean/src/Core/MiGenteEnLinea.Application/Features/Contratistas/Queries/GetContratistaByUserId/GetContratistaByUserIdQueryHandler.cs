using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Contratistas.Common;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.GetContratistaByUserId;

/// <summary>
/// Handler: Obtiene un contratista por su userId
/// </summary>
public class GetContratistaByUserIdQueryHandler : IRequestHandler<GetContratistaByUserIdQuery, ContratistaDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetContratistaByUserIdQueryHandler> _logger;

    public GetContratistaByUserIdQueryHandler(
        IApplicationDbContext context,
        ILogger<GetContratistaByUserIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ContratistaDto?> Handle(GetContratistaByUserIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando contratista por userId: {UserId}", request.UserId);

        var contratista = await _context.Contratistas
            .AsNoTracking()
            .Where(c => c.UserId == request.UserId)
            .Select(c => new ContratistaDto
            {
                ContratistaId = c.Id,
                UserId = c.UserId,
                FechaIngreso = c.FechaIngreso,
                Titulo = c.Titulo,
                Tipo = c.Tipo,
                Identificacion = c.Identificacion,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                NombreCompleto = $"{c.Nombre} {c.Apellido}",
                Sector = c.Sector,
                Experiencia = c.Experiencia,
                Presentacion = c.Presentacion,
                Telefono1 = c.Telefono1,
                Whatsapp1 = c.Whatsapp1,
                Telefono2 = c.Telefono2,
                Whatsapp2 = c.Whatsapp2,
                Email = c.Email != null ? c.Email.Value : null,
                Activo = c.Activo,
                Provincia = c.Provincia,
                NivelNacional = c.NivelNacional,
                ImagenUrl = c.ImagenUrl,
                // Campos calculados
                TieneWhatsApp = (c.Telefono1 != null && c.Whatsapp1) || (c.Telefono2 != null && c.Whatsapp2),
                PerfilCompleto = !string.IsNullOrWhiteSpace(c.UserId) &&
                                 !string.IsNullOrWhiteSpace(c.Nombre) &&
                                 !string.IsNullOrWhiteSpace(c.Apellido) &&
                                 !string.IsNullOrWhiteSpace(c.Titulo) &&
                                 !string.IsNullOrWhiteSpace(c.Presentacion) &&
                                 !string.IsNullOrWhiteSpace(c.Telefono1) &&
                                 !string.IsNullOrWhiteSpace(c.Provincia),
                PuedeRecibirTrabajos = c.Activo &&
                                       !string.IsNullOrWhiteSpace(c.Telefono1) &&
                                       (!string.IsNullOrWhiteSpace(c.Presentacion) || !string.IsNullOrWhiteSpace(c.Titulo))
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (contratista == null)
        {
            _logger.LogInformation("Contratista no encontrado para userId: {UserId}", request.UserId);
        }
        else
        {
            _logger.LogInformation("Contratista encontrado. ContratistaId: {ContratistaId}", contratista.ContratistaId);
        }

        return contratista;
    }
}
