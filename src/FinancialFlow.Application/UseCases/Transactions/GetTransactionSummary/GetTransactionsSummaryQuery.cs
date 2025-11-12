using MediatR;
using FinancialFlow.Application.Common.Results;

namespace FinancialFlow.Application.UseCases.Transactions.GetTransactionSummary;

public sealed record GetTransactionsSummaryQuery : IRequest<Result<TransactionSummaryDto>>
{
    public Guid UserId { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}