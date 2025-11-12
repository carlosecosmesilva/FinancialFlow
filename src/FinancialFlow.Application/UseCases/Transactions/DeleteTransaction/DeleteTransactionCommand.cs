using MediatR;
using FinancialFlow.Application.Common.Results;

namespace FinancialFlow.Application.UseCases.Transactions.DeleteTransaction;

public sealed record DeleteTransactionCommand : IRequest<Result<Guid>>
{
    public Guid TransactionId { get; init; }
    public Guid UserId { get; init; }
}