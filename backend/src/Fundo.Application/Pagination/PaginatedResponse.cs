namespace Fundo.Application.Pagination;

public sealed class PaginatedResponse<T>
{
    private PaginatedResponse(IReadOnlyList<T> items, int pageNumber, int pageSize, long totalItems)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = pageSize == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);
    }

    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public long TotalItems { get; }
    public int TotalPages { get; }

    public bool HasPrevious => PageNumber > 1;
    public bool HasNext => PageNumber < TotalPages;
    public bool IsEmpty => TotalItems == 0;

    public static PaginatedResponse<T> Create(IReadOnlyList<T> items, int pageNumber, int pageSize, long totalItems)
        => new(items, pageNumber, pageSize, totalItems);

    public static PaginatedResponse<T> Empty(int pageNumber, int pageSize)
        => new(Array.Empty<T>(), pageNumber, pageSize, 0);
}