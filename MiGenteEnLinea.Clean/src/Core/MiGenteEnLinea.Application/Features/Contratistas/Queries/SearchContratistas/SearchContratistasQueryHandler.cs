using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Contratistas.Common;

namespace MiGenteEnLinea.Application.Features.Contratistas.Queries.SearchContratistas;

/// <summary>
/// Handler: Busca contratistas con filtros y paginación
/// </summary>
public class SearchContratistasQueryHandler : IRequestHandler<SearchContratistasQuery, SearchContratistasResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<SearchContratistasQueryHandler> _logger;

    public SearchContratistasQueryHandler(
        IApplicationDbContext context,
        ILogger<SearchContratistasQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SearchContratistasResult> Handle(SearchContratistasQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Buscando contratistas. SearchTerm: {SearchTerm}, Provincia: {Provincia}, PageIndex: {PageIndex}",
            request.SearchTerm, request.Provincia, request.PageIndex);

        // Validar y ajustar PageSize
        var pageSize = request.PageSize;
        if (pageSize > 100) pageSize = 100;
        if (pageSize < 1) pageSize = 10;

        // Validar y ajustar PageIndex
        var pageIndex = request.PageIndex;
        if (pageIndex < 1) pageIndex = 1;

        // 1. BASE QUERY
        var query = _context.Contratistas.AsNoTracking();

        // 2. FILTRO: Solo activos (si se especifica)
        if (request.SoloActivos)
        {
            query = query.Where(c => c.Activo);
        }

        // 3. FILTRO: Búsqueda por término (case-insensitive, busca en Titulo, Presentacion, Sector)
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTermLower = request.SearchTerm.ToLower();
            query = query.Where(c =>
                (c.Titulo != null && c.Titulo.ToLower().Contains(searchTermLower)) ||
                (c.Presentacion != null && c.Presentacion.ToLower().Contains(searchTermLower)) ||
                (c.Sector != null && c.Sector.ToLower().Contains(searchTermLower))
            );
        }

        // 4. FILTRO: Provincia (si no es "Cualquier Ubicacion")
        if (!string.IsNullOrWhiteSpace(request.Provincia) && 
            request.Provincia != "Cualquier Ubicacion")
        {
            var provinciaLower = request.Provincia.ToLower();
            query = query.Where(c => c.Provincia != null && c.Provincia.ToLower() == provinciaLower);
        }

        // 5. FILTRO: Sector
        if (!string.IsNullOrWhiteSpace(request.Sector))
        {
            var sectorLower = request.Sector.ToLower();
            query = query.Where(c => c.Sector != null && c.Sector.ToLower() == sectorLower);
        }

        // 6. FILTRO: Experiencia mínima
        if (request.ExperienciaMinima.HasValue)
        {
            query = query.Where(c => c.Experiencia >= request.ExperienciaMinima.Value);
        }

        // 7. CONTAR TOTAL DE REGISTROS (antes de paginación)
        var totalRecords = await query.CountAsync(cancellationToken);

        // 8. ORDENAR: Por fecha de ingreso descendente (más recientes primero)
        query = query.OrderByDescending(c => c.FechaIngreso ?? DateTime.MinValue);

        // 9. PAGINACIÓN: Skip y Take
        var contratistas = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
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
            .ToListAsync(cancellationToken);

        _logger.LogInformation(
            "Búsqueda completada. Total: {TotalRecords}, Página: {PageIndex}/{TotalPages}",
            totalRecords, pageIndex, (int)Math.Ceiling(totalRecords / (double)pageSize));

        return new SearchContratistasResult(contratistas, totalRecords, pageIndex, pageSize);
    }
}
