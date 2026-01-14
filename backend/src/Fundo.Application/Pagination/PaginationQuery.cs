namespace Fundo.Application.Pagination;

public sealed class PaginationQuery
{
    private int _pageSize;

    public PaginationQuery(int pageNumber = 1, int pageSize = 20)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int Skip => (PageNumber - 1) * PageSize;
}