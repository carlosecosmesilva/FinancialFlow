using MediatR;
using FinancialFlow.Application.Common.Results;
using FinancialFlow.Application.UseCases.Transactions.GetTransactions;

namespace FinancialFlow.Application.UseCases.Transactions.GetTransactionsByPeriod;

public sealed record GetTransactionsByPeriodQuery : IRequest<Result<List<TransactionListDto>>>
{
    public Guid UserId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public string? Type { get; init; }
}