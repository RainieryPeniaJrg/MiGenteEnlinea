namespace MiGenteEnLinea.Application.Common.Models;

/// <summary>
/// Clase genérica para representar una lista paginada de elementos.
/// </summary>
/// <typeparam name="T">El tipo de elementos en la lista.</typeparam>
public class PaginatedList<T>
{
    /// <summary>
    /// Los elementos de la página actual.
    /// </summary>
    public List<T> Items { get; }

    /// <summary>
    /// El índice de la página actual (1-based).
    /// </summary>
    public int PageIndex { get; }

    /// <summary>
    /// El número total de páginas.
    /// </summary>
    public int TotalPages { get; }

    /// <summary>
    /// El número total de elementos en todas las páginas.
    /// </summary>
    public int TotalCount { get; }

    /// <summary>
    /// El tamaño de página utilizado.
    /// </summary>
    public int PageSize { get; }

    /// <summary>
    /// Indica si existe una página anterior.
    /// </summary>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// Indica si existe una página siguiente.
    /// </summary>
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        PageSize = pageSize;
        Items = items;
    }
}
