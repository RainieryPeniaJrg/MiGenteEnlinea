using Microsoft.EntityFrameworkCore;
using MiGenteEnLinea.Domain.Entities.Contratistas;
using MiGenteEnLinea.Domain.Interfaces.Repositories.Contratistas;
using MiGenteEnLinea.Infrastructure.Persistence.Contexts;
using System.Linq.Expressions;

namespace MiGenteEnLinea.Infrastructure.Persistence.Repositories.Contratistas;

/// <summary>
/// Implementación del repositorio Contratista con EF Core
/// </summary>
public class ContratistaRepository : Repository<Contratista>, IContratistaRepository
{
    public ContratistaRepository(MiGenteDbContext context) : base(context)
    {
    }

    // ============================================
    // BÚSQUEDAS BÁSICAS
    // ============================================

    public async Task<Contratista?> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);
    }

    public async Task<bool> ExistsByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await _dbSet
            .AnyAsync(c => c.UserId == userId, ct);
    }

    // ============================================
    // BÚSQUEDAS PAGINADAS CON FILTROS COMPLEJOS
    // ============================================

    public async Task<(IEnumerable<Contratista> Items, int TotalCount)> SearchAsync(
        string? searchTerm,
        string? provincia,
        string? sector,
        int? experienciaMinima,
        bool soloActivos,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = _dbSet.AsNoTracking();

        // Filtro: Solo activos
        if (soloActivos)
        {
            query = query.Where(c => c.Activo);
        }

        // Filtro: Búsqueda en Titulo, Presentacion, Sector
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(c =>
                (c.Titulo != null && c.Titulo.ToLower().Contains(searchLower)) ||
                (c.Presentacion != null && c.Presentacion.ToLower().Contains(searchLower)) ||
                (c.Sector != null && c.Sector.ToLower().Contains(searchLower))
            );
        }

        // Filtro: Provincia (excluye "Cualquier Ubicacion")
        if (!string.IsNullOrWhiteSpace(provincia) && provincia != "Cualquier Ubicacion")
        {
            var provinciaLower = provincia.ToLower();
            query = query.Where(c => c.Provincia != null && c.Provincia.ToLower() == provinciaLower);
        }

        // Filtro: Sector
        if (!string.IsNullOrWhiteSpace(sector))
        {
            var sectorLower = sector.ToLower();
            query = query.Where(c => c.Sector != null && c.Sector.ToLower() == sectorLower);
        }

        // Filtro: Experiencia mínima
        if (experienciaMinima.HasValue)
        {
            query = query.Where(c => c.Experiencia >= experienciaMinima.Value);
        }

        // Contar total
        var totalCount = await query.CountAsync(ct);

        // Paginación y ordenamiento (más recientes primero)
        var items = await query
            .OrderByDescending(c => c.FechaIngreso ?? DateTime.MinValue)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    // ============================================
    // PROYECCIONES DTO (OPTIMIZACIÓN)
    // ============================================

    public async Task<TResult?> GetByIdProjectedAsync<TResult>(
        int id,
        Expression<Func<Contratista, TResult>> selector,
        CancellationToken ct = default) where TResult : class
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(selector)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<TResult?> GetByUserIdProjectedAsync<TResult>(
        string userId,
        Expression<Func<Contratista, TResult>> selector,
        CancellationToken ct = default) where TResult : class
    {
        return await _dbSet
            .AsNoTracking()
            .Where(c => c.UserId == userId)
            .Select(selector)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<(IEnumerable<TResult> Items, int TotalCount)> SearchProjectedAsync<TResult>(
        string? searchTerm,
        string? provincia,
        string? sector,
        int? experienciaMinima,
        bool soloActivos,
        int pageNumber,
        int pageSize,
        Expression<Func<Contratista, TResult>> selector,
        CancellationToken ct = default) where TResult : class
    {
        var query = _dbSet.AsNoTracking();

        // Aplicar mismos filtros que SearchAsync
        if (soloActivos)
        {
            query = query.Where(c => c.Activo);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchLower = searchTerm.ToLower();
            query = query.Where(c =>
                (c.Titulo != null && c.Titulo.ToLower().Contains(searchLower)) ||
                (c.Presentacion != null && c.Presentacion.ToLower().Contains(searchLower)) ||
                (c.Sector != null && c.Sector.ToLower().Contains(searchLower))
            );
        }

        if (!string.IsNullOrWhiteSpace(provincia) && provincia != "Cualquier Ubicacion")
        {
            var provinciaLower = provincia.ToLower();
            query = query.Where(c => c.Provincia != null && c.Provincia.ToLower() == provinciaLower);
        }

        if (!string.IsNullOrWhiteSpace(sector))
        {
            var sectorLower = sector.ToLower();
            query = query.Where(c => c.Sector != null && c.Sector.ToLower() == sectorLower);
        }

        if (experienciaMinima.HasValue)
        {
            query = query.Where(c => c.Experiencia >= experienciaMinima.Value);
        }

        // Contar total
        var totalCount = await query.CountAsync(ct);

        // Paginación, ordenamiento y proyección
        var items = await query
            .OrderByDescending(c => c.FechaIngreso ?? DateTime.MinValue)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}
