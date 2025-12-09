namespace ECommerce.ProductManagement.Application.Common;

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; }
    public int TotalCount { get; }
    public int PageIndex { get; }
    public int PageSize { get; }

    public bool HasNext { get; }

    public PagedResult(
        IReadOnlyList<T> items,
        int totalCount,
        int pageIndex,
        int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
        PageSize = pageSize;

        HasNext = pageIndex * pageSize < totalCount;
    }
}
