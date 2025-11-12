using MediatR;
using Microsoft.Extensions.Logging;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Application.Common.Results;

namespace FinancialFlow.Application.UseCases.Transactions.GetTransactions;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, Result<PaginatedList<TransactionListDto>>>
{
    private readonly ITransactionRepository _repository;
    private readonly ILogger<GetTransactionsQueryHandler> _logger;

    public GetTransactionsQueryHandler(
        ITransactionRepository repository,
        ILogger<GetTransactionsQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<PaginatedList<TransactionListDto>>> Handle(
        GetTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Getting transactions for user {UserId}, Page {PageNumber}",
                request.UserId,
                request.PageNumber);

            // Get transactions from repository
            var transactions = await _repository.GetByUserIdAsync(request.UserId, cancellationToken);

            // Apply filters
            if (!string.IsNullOrEmpty(request.Type) &&
                Enum.TryParse<TransactionType>(request.Type, ignoreCase: true, out var type))
            {
                transactions = transactions.Where(t => t.Type == type);
            }

            if (!string.IsNullOrEmpty(request.Category))
            {
                transactions = transactions.Where(t =>
                    t.Category != null &&
                    t.Category.Equals(request.Category, StringComparison.OrdinalIgnoreCase));
            }

            if (request.StartDate.HasValue)
            {
                transactions = transactions.Where(t => t.TransactionDate.DateTime >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                transactions = transactions.Where(t => t.TransactionDate.DateTime <= request.EndDate.Value);
            }

            // Apply sorting
            transactions = request.SortBy?.ToLower() switch
            {
                "amount" => request.SortDescending
                    ? transactions.OrderByDescending(t => t.Value.Amount)
                    : transactions.OrderBy(t => t.Value.Amount),
                "description" => request.SortDescending
                    ? transactions.OrderByDescending(t => t.Description)
                    : transactions.OrderBy(t => t.Description),
                "type" => request.SortDescending
                    ? transactions.OrderByDescending(t => t.Type)
                    : transactions.OrderBy(t => t.Type),
                _ => request.SortDescending
                    ? transactions.OrderByDescending(t => t.TransactionDate)
                    : transactions.OrderBy(t => t.TransactionDate)
            };

            // Get total count
            var totalCount = transactions.Count();

            // Apply pagination
            var items = transactions
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new TransactionListDto
                {
                    Id = t.Id,
                    Description = t.Description,
                    Amount = t.Value.Amount,
                    Currency = t.Value.Currency,
                    Type = t.Type.ToString(),
                    TransactionDate = t.TransactionDate.DateTime,
                    Category = t.Category,
                    Notes = t.Notes
                })
                .ToList();

            var result = new PaginatedList<TransactionListDto>(
                items,
                totalCount,
                request.PageNumber,
                request.PageSize);

            _logger.LogInformation(
                "Retrieved {Count} transactions (page {Page} of {Total})",
                items.Count,
                result.PageNumber,
                result.TotalPages);

            return Result<PaginatedList<TransactionListDto>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transactions for user {UserId}", request.UserId);
            return Result<PaginatedList<TransactionListDto>>.Failure(
                "An unexpected error occurred while retrieving transactions");
        }
    }
}