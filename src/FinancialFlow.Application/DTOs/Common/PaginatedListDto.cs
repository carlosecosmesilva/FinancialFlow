using System.Collections.Generic;

namespace FinancialFlow.Application.DTOs.Common;

public sealed record PaginatedListDto<T>
{
    public List<T> Items { get; init; } = new();
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => PageSize == 0 ? 0 : (int)System.Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedListDto() { }

    public PaginatedListDto(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = new List<T>(items);
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
