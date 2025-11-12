namespace FinancialFlow.Application.UseCases.Transactions.GetTransactions;

public sealed record TransactionListDto
{
    public Guid Id { get; init; }
    public string Description { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public DateTime TransactionDate { get; init; }
    public string? Category { get; init; }
    public string? Notes { get; init; }
}