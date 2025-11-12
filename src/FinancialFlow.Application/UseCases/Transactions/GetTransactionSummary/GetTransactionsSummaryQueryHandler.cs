using MediatR;
using Microsoft.Extensions.Logging;
using FinancialFlow.Domain.Interfaces;
using FinancialFlow.Domain.Enums;
using FinancialFlow.Application.Common.Results;

namespace FinancialFlow.Application.UseCases.Transactions.GetTransactionSummary;

public class GetTransactionsSummaryQueryHandler
    : IRequestHandler<GetTransactionsSummaryQuery, Result<TransactionSummaryDto>>
{
    private readonly ITransactionRepository _repository;
    private readonly ILogger<GetTransactionsSummaryQueryHandler> _logger;

    public GetTransactionsSummaryQueryHandler(
        ITransactionRepository repository,
        ILogger<GetTransactionsSummaryQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<TransactionSummaryDto>> Handle(
        GetTransactionsSummaryQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var startDate = new DateTimeOffset(request.StartDate, TimeSpan.Zero);
            var endDate = new DateTimeOffset(request.EndDate, TimeSpan.Zero);

            // Get transactions for the period
            var transactions = await _repository.GetByPeriodAsync(
                request.UserId,
                startDate,
                endDate,
                cancellationToken);

            var transactionList = transactions.ToList();

            // Calculate totals
            var totalRevenue = transactionList
                .Where(t => t.Type == TransactionType.Revenue)
                .Sum(t => t.Value.Amount);

            var totalExpense = transactionList
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Value.Amount);

            var summary = new TransactionSummaryDto
            {
                TotalRevenue = totalRevenue,
                TotalExpense = totalExpense,
                Balance = totalRevenue - totalExpense,
                TransactionCount = transactionList.Count,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Currency = transactionList.FirstOrDefault()?.Value.Currency ?? "BRL"
            };

            _logger.LogInformation(
                "Generated transaction summary for user {UserId}: Revenue={Revenue}, Expense={Expense}, Balance={Balance}",
                request.UserId,
                totalRevenue,
                totalExpense,
                summary.Balance);

            return Result<TransactionSummaryDto>.Success(summary);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error generating transaction summary for user {UserId}",
                request.UserId);
            return Result<TransactionSummaryDto>.Failure(
                "An unexpected error occurred while generating the transaction summary");
        }
    }
}