using MediatR;
using Microsoft.Extensions.Logging;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Application.Common.Results;
using FinancialFlow.Application.UseCases.Transactions.GetTransactions;

namespace FinancialFlow.Application.UseCases.Transactions.GetTransactionsByPeriod;

public class GetTransactionsByPeriodQueryHandler
    : IRequestHandler<GetTransactionsByPeriodQuery, Result<List<TransactionListDto>>>
{
    private readonly ITransactionRepository _repository;
    private readonly ILogger<GetTransactionsByPeriodQueryHandler> _logger;

    public GetTransactionsByPeriodQueryHandler(
        ITransactionRepository repository,
        ILogger<GetTransactionsByPeriodQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<List<TransactionListDto>>> Handle(
        GetTransactionsByPeriodQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var startDate = new DateTimeOffset(request.StartDate, TimeSpan.Zero);
            var endDate = new DateTimeOffset(request.EndDate, TimeSpan.Zero);

            var transactions = await _repository.GetByPeriodAsync(
                request.UserId,
                startDate,
                endDate,
                cancellationToken);

            // Apply type filter if specified
            if (!string.IsNullOrEmpty(request.Type) &&
                Enum.TryParse<TransactionType>(request.Type, ignoreCase: true, out var type))
            {
                transactions = transactions.Where(t => t.Type == type);
            }

            var result = transactions
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

            _logger.LogInformation(
                "Retrieved {Count} transactions for user {UserId} between {StartDate} and {EndDate}",
                result.Count,
                request.UserId,
                request.StartDate,
                request.EndDate);

            return Result<List<TransactionListDto>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving transactions for user {UserId} in period {StartDate} to {EndDate}",
                request.UserId,
                request.StartDate,
                request.EndDate);
            return Result<List<TransactionListDto>>.Failure(
                "An unexpected error occurred while retrieving transactions");
        }
    }
}