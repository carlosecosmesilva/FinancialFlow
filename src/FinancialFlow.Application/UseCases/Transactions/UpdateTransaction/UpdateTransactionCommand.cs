using MediatR;
using FinancialFlow.Application.Common.Results;

namespace FinancialFlow.Application.UseCases.Transactions.UpdateTransaction;

public sealed record UpdateTransactionCommand : IRequest<Result<Guid>>
{
    public Guid TransactionId { get; init; }
    public Guid UserId { get; init; }
    public required string Description { get; init; }
    public decimal Amount { get; init; }
    public required string Currency { get; init; }
    public required string Type { get; init; }
    public DateTime TransactionDate { get; init; }
    public string? Category { get; init; }
    public string? Notes { get; init; }
}