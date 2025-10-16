using Microsoft.EntityFrameworkCore;

namespace MiGenteEnLinea.Application.Common.Models;

/// <summary>
/// Extension methods para PaginatedList
/// </summary>
public static class PaginatedListExtensions
{
    /// <summary>
    /// Convierte un IQueryable a PaginatedList de forma asíncrona
    /// </summary>
    public static async Task<PaginatedList<T>> ToPaginatedListAsync<T>(
        this IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        pageSize = pageSize <= 0 ? 10 : pageSize;

        var count = await source.CountAsync(cancellationToken);
        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }
}
