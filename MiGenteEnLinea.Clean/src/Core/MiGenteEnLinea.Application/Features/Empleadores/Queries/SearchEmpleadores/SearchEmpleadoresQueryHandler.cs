using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MiGenteEnLinea.Application.Common.Interfaces;
using MiGenteEnLinea.Application.Features.Empleadores.DTOs;

namespace MiGenteEnLinea.Application.Features.Empleadores.Queries.SearchEmpleadores;

/// <summary>
/// Handler: Busca empleadores con paginación y filtros
/// </summary>
public sealed class SearchEmpleadoresQueryHandler : IRequestHandler<SearchEmpleadoresQuery, SearchEmpleadoresResult>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<SearchEmpleadoresQueryHandler> _logger;

    public SearchEmpleadoresQueryHandler(
        IApplicationDbContext context,
        ILogger<SearchEmpleadoresQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<SearchEmpleadoresResult> Handle(SearchEmpleadoresQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Búsqueda de empleadores. SearchTerm: {SearchTerm}, PageIndex: {PageIndex}, PageSize: {PageSize}",
            request.SearchTerm ?? "N/A", request.PageIndex, request.PageSize);

        // ============================================
        // PASO 1: Query base
        // ============================================
        var query = _context.Empleadores.AsNoTracking();

        // ============================================
        // PASO 2: Filtro por término de búsqueda
        // ============================================
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchLower = request.SearchTerm.ToLower();

            query = query.Where(e =>
                (e.Habilidades != null && e.Habilidades.ToLower().Contains(searchLower)) ||
                (e.Experiencia != null && e.Experiencia.ToLower().Contains(searchLower)) ||
                (e.Descripcion != null && e.Descripcion.ToLower().Contains(searchLower))
            );
        }

        // ============================================
        // PASO 3: Contar total de registros
        // ============================================
        var totalRecords = await query.CountAsync(cancellationToken);

        // ============================================
        // PASO 4: Aplicar paginación
        // ============================================
        var empleadores = await query
            .OrderByDescending(e => e.FechaPublicacion ?? e.CreatedAt)
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EmpleadorDto
            {
                EmpleadorId = e.Id,
                UserId = e.UserId,
                FechaPublicacion = e.FechaPublicacion,
                Habilidades = e.Habilidades,
                Experiencia = e.Experiencia,
                Descripcion = e.Descripcion,
                TieneFoto = e.Foto != null && e.Foto.Length > 0,
                CreatedAt = e.CreatedAt,
                UpdatedAt = e.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        _logger.LogInformation(
            "Búsqueda completada. Registros encontrados: {TotalRecords}, Página actual: {PageIndex}/{TotalPages}",
            totalRecords, request.PageIndex, (int)Math.Ceiling((double)totalRecords / request.PageSize));

        return new SearchEmpleadoresResult
        {
            Empleadores = empleadores,
            TotalRecords = totalRecords,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };
    }
}
