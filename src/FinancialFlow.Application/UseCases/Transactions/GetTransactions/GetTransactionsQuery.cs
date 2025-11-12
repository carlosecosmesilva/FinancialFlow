using MediatR;
using FinancialFlow.Application.Common.Results;

namespace FinancialFlow.Application.UseCases.Transactions.GetTransactions;

public sealed record GetTransactionsQuery : IRequest<Result<PaginatedList<TransactionListDto>>>
{
    public Guid UserId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? Type { get; init; }
    public string? Category { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public string? SortBy { get; init; } = "TransactionDate";
    public bool SortDescending { get; init; } = true;
}

public class PaginatedList<T>
{
    public List<T> Items { get; init; } = new();
    public int PageNumber { get; init; }
    public int TotalPages { get; init; }
    public int TotalCount { get; init; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
    }
}